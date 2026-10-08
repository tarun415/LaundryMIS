using LaudaryMis.Models;
using LaudaryMis.Helpers;
using LaudaryMis.Repositories;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class WPRService : IWPRService
    {
        private readonly IWPRRepository _repo;


        private readonly LaudaryMis.Helpers.PortalCalendar _calendar;

        public WPRService(IWPRRepository repo, LaudaryMis.Helpers.PortalCalendar calendar)
        {
            _repo = repo;
            _calendar = calendar;
        }

        public async Task<List<AgreementVM>> GetHospitalAgreements(int hospitalId)
        {
            var data = await _repo.GetHospitalAgreements(hospitalId);
            return data?.ToList() ?? new List<AgreementVM>();
        }

        public async Task<(bool Success, string Message)> SubmitWPRAsync(WPRVM model)
        {
            try
            {
                // Portal shuru hone se pehle ke mahine offline ho chuke hain
                if (!int.TryParse(model.Month, out int wprMonth)
                    || _calendar.IsBeforeStart(wprMonth, model.Year))
                    return (false, _calendar.BeforeStartMessage("WPR"));

                // ✅ Duplicate check — same week, month, year, staff
                bool exists = await _repo.WPRExistsAsync(
                    model.HospitalId, model.Week, model.Month, model.Year, model.StaffName.Trim());

                if (exists)
                    return (false, $"WPR for Week {model.Week} - {model.Month} {model.Year} is already submitted for {model.StaffName}.");

                int paymentPct = CalculatePaymentPercentage(model.TotalScore);

                var report = new WeeklyPerformanceReport
                {
                    AgreementId = model.AgreementId,
                    HospitalId = model.HospitalId,
                    ProviderId = model.ProviderId,

                    Week = model.Week,
                    Month = model.Month,
                    Year = model.Year,

                    StaffName = model.StaffName.Trim(),
                    Remarks = model.Remarks,

                    TotalScore = model.TotalScore,
                    PaymentPercentage = paymentPct,
                    SubmittedAt = DateTime.Now
                };
                DateTime weekStart;
                DateTime weekEnd;

                int monthNumber = int.Parse(model.Month);

                switch (model.Week)
                {
                    case 1:
                        weekStart = new DateTime(model.Year, monthNumber, 1);
                        weekEnd = new DateTime(model.Year, monthNumber, 7);
                        break;

                    case 2:
                        weekStart = new DateTime(model.Year, monthNumber, 8);
                        weekEnd = new DateTime(model.Year, monthNumber, 14);
                        break;

                    case 3:
                        weekStart = new DateTime(model.Year, monthNumber, 15);
                        weekEnd = new DateTime(model.Year, monthNumber, 21);
                        break;

                    case 4:
                        weekStart = new DateTime(model.Year, monthNumber, 22);
                        weekEnd = new DateTime(model.Year, monthNumber, 28);
                        break;

                    default:
                        weekStart = new DateTime(model.Year, monthNumber, 29);
                        weekEnd = new DateTime(
                            model.Year,
                            monthNumber,
                            DateTime.DaysInMonth(model.Year, monthNumber));
                        break;
                       
                }
                string grade;

                if (model.TotalScore <= 20)
                    grade = "No Payment";
                else if (model.TotalScore <= 40)
                    grade = "40% Payment";
                else if (model.TotalScore <= 60)
                    grade = "60% Payment";
                else if (model.TotalScore <= 70)
                    grade = "80% Payment";
                else if (model.TotalScore <= 80)
                    grade = "90% Payment";
                else
                    grade = "100% Payment";
                var entry = new WPREntry
                {
                    AgreementId = model.AgreementId,
                    HospitalId = model.HospitalId,
                    ProviderId = model.ProviderId,

                    WeekStart = weekStart,
                    WeekEnd = weekEnd,

                   
                    TotalScore = model.TotalScore,

                    MonthNo = int.Parse(model.Month),
                    YearNo = model.Year,
                    WeekNo = model.Week,

                    PerformanceGrade = grade,
                    Remarks = model.Remarks
                };

                var details = model.Details.Select(x => new WPRDetail
                {
                    ParameterId = x.ParameterId,
                    ParameterName = LaudaryMis.Helpers.WprParameters.StoredName(x.ParameterId),
                    Score = x.Score
                }).ToList();

                await _repo.SaveWPRAsync(report, entry, details);

                return (true, "WPR submitted successfully. It is now with your CMS for verification and can no longer be changed.");
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        private static int CalculatePaymentPercentage(int totalScore) =>
            totalScore switch
            {
                <= 20 => 0,
                <= 40 => 40,
                <= 60 => 60,
                <= 70 => 80,
                <= 80 => 90,
                _ => 100
            };

        public async Task<bool> CheckWeeklyVerification(int hospitalId, int weekNo, int month, int year)
        {
            return await _repo.CheckWeeklyVerification(hospitalId, weekNo, month, year);
        }
        public async Task<List<WeeklyPerformanceVM>>
       GetWeeklyPerformanceData(
           int agreementId,
           int hospitalId,
           int weekNo,
           int month,
           int year)
        {
            return await _repo.GetWeeklyPerformanceData(
                agreementId,
                hospitalId,
                weekNo,
                month,
                year);
        }

        // ══════════════════════════════════════════════════════
        // CMS REVIEW
        // ══════════════════════════════════════════════════════

        public Task<List<WprListItemVM>> GetListAsync(
            int? hospitalId, int? providerId, string? status, int? month, int? year) =>
            _repo.GetWprListAsync(hospitalId, providerId, status, month, year);

        public async Task<WprReviewVM?> GetReviewAsync(int id)
        {
            var vm = await _repo.GetWprReviewAsync(id);
            if (vm == null) return null;

            // Older rows were saved with other parameter names; the contract's names are the truth
            foreach (var s in vm.Scores)
                s.ParameterName = WprParameters.NameOf(s.ParameterId);

            return vm;
        }

        public async Task<CmsDashboardVM> GetCmsDashboardAsync(int hospitalId)
        {
            var (pending, verified) = await _repo.GetStatusCountsAsync(hospitalId);
            var recent = (await _repo.GetWprListAsync(hospitalId, null, WprStatus.Pending, null, null))
                .Take(10).ToList();

            return new CmsDashboardVM { Pending = pending, Verified = verified, RecentPending = recent };
        }

        // CMS saves its edits of a pending WPR and, if asked, verifies it in the same step
        public async Task<(bool Success, string Message)> CmsSaveAsync(
            int hospitalId, int userId, WprReviewPost post)
        {
            var wpr = await _repo.GetWprReviewAsync(post.Id);
            if (wpr == null || wpr.HospitalId != hospitalId)
                return (false, "WPR not found.");
            if (!wpr.IsPending)
                return (false, "This WPR is already verified and can no longer be edited.");

            var (ok, message, changed) = await ApplyScoresAsync(
                wpr, post.Scores, userId, "CMS", post.EditRemarks, null);
            if (!ok) return (false, message);

            if (post.Action == "verify")
            {
                int n = await _repo.VerifyAsync(new[] { wpr.Id }, hospitalId, userId);
                if (n == 0) return (false, "The WPR could not be verified.");
                return (true, changed > 0
                    ? "WPR edited, verified and accepted."
                    : "WPR verified and accepted.");
            }

            return (true, changed > 0 ? "Changes saved. The WPR is still pending." : "No changes to save.");
        }

        public async Task<(bool Success, string Message)> CmsVerifyAsync(int hospitalId, int userId, int id)
        {
            int n = await _repo.VerifyAsync(new[] { id }, hospitalId, userId);
            return n > 0
                ? (true, "WPR verified and accepted.")
                : (false, "This WPR is not pending, or it is not your hospital's.");
        }

        // Verifies every pending WPR of one month in one go
        public async Task<(bool Success, string Message)> CmsVerifyMonthAsync(
            int hospitalId, int userId, int month, int year)
        {
            if (month is < 1 or > 12 || year < 2000)
                return (false, "Select a month and a year.");

            var ids = await _repo.GetPendingIdsAsync(hospitalId, month, year);
            if (ids.Count == 0)
                return (false, "There is no pending WPR in that month.");

            int n = await _repo.VerifyAsync(ids, hospitalId, userId);
            return (true, $"{n} WPR verified and accepted.");
        }

        // Applies new scores to a WPR: validates them, recalculates the total and payment band,
        // saves them and logs every changed score. Used by the CMS and by the admin (dispute fix).
        public async Task<(bool Success, string Message, int Changed)> ApplyScoresAsync(
            WprReviewVM wpr, List<WprScoreVM> posted, int editorId, string editorRole,
            string? remarks, int? disputeId)
        {
            foreach (var s in posted)
            {
                if (s.ParameterId < 1 || s.ParameterId > WprParameters.Names.Length)
                    return (false, "Unknown parameter.", 0);
                if (s.Score < 0 || s.Score > WprParameters.MaxScore)
                    return (false, $"A score must be between 0 and {WprParameters.MaxScore}.", 0);
            }

            var current = wpr.Scores.ToDictionary(x => x.ParameterId, x => x.Score);
            var latest = new Dictionary<int, int>(current);
            foreach (var s in posted) latest[s.ParameterId] = s.Score;

            var changes = posted
                .Where(s => !current.TryGetValue(s.ParameterId, out var old) || old != s.Score)
                .Select(s => new WprScoreChange
                {
                    ParameterId = s.ParameterId,
                    ParameterName = WprParameters.NameOf(s.ParameterId),
                    OldScore = current.TryGetValue(s.ParameterId, out var o) ? o : null,
                    NewScore = s.Score
                })
                .ToList();

            if (changes.Count == 0)
                return (true, "", 0);

            if (string.IsNullOrWhiteSpace(remarks))
                return (false, "Write the reason for the change.", 0);

            int newTotal = latest.Values.Sum();

            await _repo.ApplyEditAsync(new WprEditCommand
            {
                WprId = wpr.Id,
                EditorId = editorId,
                EditorRole = editorRole,
                Remarks = remarks.Trim(),
                DisputeId = disputeId,
                OldTotal = wpr.TotalScore,
                NewTotal = newTotal,
                NewPercentage = WprParameters.PaymentPercentage(newTotal),
                NewGrade = WprParameters.Grade(newTotal),
                Changes = changes
            });

            return (true, "", changes.Count);
        }
    }
}