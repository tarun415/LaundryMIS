using LaudaryMis.Helpers;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class DisputeService : IDisputeService
    {
        private const long MaxLetterBytes = 5 * 1024 * 1024; // 5 MB

        private readonly IDisputeRepository _repo;
        private readonly IBillService _bills;
        private readonly IWPRService _wpr;
        private readonly string _letterDir;

        public DisputeService(IDisputeRepository repo, IBillService bills, IWPRService wpr, IWebHostEnvironment env)
        {
            _repo = repo;
            _bills = bills;
            _wpr = wpr;
            // Outside wwwroot: letters are only served through the controller, to the people allowed to see them
            _letterDir = Path.Combine(env.ContentRootPath, "App_Data", "DisputeLetters");
        }

        public async Task<(bool Success, string Message)> RaiseAsync(int providerId, int userId, RaiseDisputeVM model)
        {
            var allowed = await _bills.GetDisputableMonthsAsync(providerId, model.HospitalId, model.Year);
            if (!allowed.Any(m => m.Month == model.Month))
                return (false, "You can raise a dispute only for a month whose bill you have printed, and which has no open dispute.");

            var ok = await _repo.InsertAsync(providerId, model.HospitalId, model.Month, model.Year,
                model.Remarks.Trim(), userId);

            return ok
                ? (true, "Your dispute has been sent to the CMS and the Admin.")
                : (false, "A dispute for this month is already open.");
        }

        public Task<List<DisputeListItemVM>> GetListAsync(int? providerId, int? hospitalId, string? status) =>
            _repo.GetListAsync(providerId, hospitalId, status);

        public Task<DisputeListItemVM?> GetAsync(int id) => _repo.GetByIdAsync(id);

        public Task<int> CountOpenAsync(int? hospitalId) => _repo.CountOpenAsync(hospitalId);

        public async Task<DisputeResolveVM?> GetResolveViewAsync(int id)
        {
            var dispute = await _repo.GetByIdAsync(id);
            if (dispute == null) return null;

            var weeks = new List<WprReviewVM>();
            var list = await _wpr.GetListAsync(dispute.HospitalId, dispute.ProviderId, null, dispute.BillMonth, dispute.BillYear);
            foreach (var item in list.OrderBy(w => w.Week))
            {
                var review = await _wpr.GetReviewAsync(item.Id);
                if (review != null) weeks.Add(review);
            }

            return new DisputeResolveVM { Dispute = dispute, Weeks = weeks };
        }

        public async Task<(bool Success, string Message)> ResolveAsync(int adminId, DisputeResolvePost post)
        {
            var view = await GetResolveViewAsync(post.DisputeId);
            if (view == null) return (false, "Dispute not found.");
            if (!view.Dispute.IsOpen) return (false, "This dispute is already closed.");

            var remarks = (post.Remarks ?? "").Trim();
            if (remarks.Length == 0) return (false, "Write what was corrected and why.");

            // The CMS letter is mandatory for a correction
            var letterError = await UploadValidator.ValidateAsync(post.Letter, UploadValidator.DocumentExtensions, MaxLetterBytes);
            if (letterError != null) return (false, "CMS letter: " + letterError);

            // Only the weeks of this hospital + month can be changed here
            var plan = new List<(WprReviewVM Wpr, WprReviewPost Post)>();
            int totalChanges = 0;
            foreach (var p in post.Weeks)
            {
                var wpr = view.Weeks.FirstOrDefault(w => w.Id == p.Id);
                if (wpr == null) return (false, "A WPR that does not belong to this dispute was submitted.");

                foreach (var s in p.Scores)
                {
                    if (s.ParameterId < 1 || s.ParameterId > WprParameters.Names.Length
                        || s.Score < 0 || s.Score > WprParameters.MaxScore)
                        return (false, $"Scores must be between 0 and {WprParameters.MaxScore}.");
                }

                var current = wpr.Scores.ToDictionary(x => x.ParameterId, x => x.Score);
                totalChanges += p.Scores.Count(s => !current.TryGetValue(s.ParameterId, out var old) || old != s.Score);
                plan.Add((wpr, p));
            }

            if (totalChanges == 0)
                return (false, "No score was changed. If the bill is correct, reject the dispute instead.");

            // Keep the letter, then change the WPRs, then close the dispute
            Directory.CreateDirectory(_letterDir);
            var ext = Path.GetExtension(post.Letter!.FileName).ToLowerInvariant();
            var stored = $"{Guid.NewGuid():N}{ext}";
            await using (var fs = File.Create(Path.Combine(_letterDir, stored)))
                await post.Letter.CopyToAsync(fs);

            foreach (var (wpr, p) in plan)
            {
                var (ok, message, _) = await _wpr.ApplyScoresAsync(wpr, p.Scores, adminId, "Admin", remarks, post.DisputeId);
                if (!ok) return (false, message);
            }

            var original = Path.GetFileName(post.Letter.FileName);
            if (original.Length > 200) original = original[^200..];

            var closed = await _repo.ResolveAsync(post.DisputeId, adminId, remarks, stored, original);
            return closed
                ? (true, "The WPR was corrected. The service provider can now print the revised bill.")
                : (false, "The WPR was corrected, but the dispute had already been closed by someone else.");
        }

        public async Task<(bool Success, string Message)> RejectAsync(int adminId, int id, string? remarks)
        {
            if (string.IsNullOrWhiteSpace(remarks))
                return (false, "A reason is required to reject a dispute.");

            var closed = await _repo.RejectAsync(id, adminId, remarks.Trim());
            return closed
                ? (true, "The dispute was rejected.")
                : (false, "This dispute is already closed.");
        }

        public string? LetterPath(DisputeListItemVM dispute)
        {
            if (!dispute.HasLetter) return null;

            // The stored name is a GUID + extension; never trust anything else
            var name = Path.GetFileName(dispute.LetterFile!);
            var full = Path.Combine(_letterDir, name);
            return File.Exists(full) ? full : null;
        }
    }
}
