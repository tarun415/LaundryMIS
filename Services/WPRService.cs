using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class WPRService : IWPRService
    {
        private readonly IWPRRepository _repo;

        private static readonly string[] ParameterNames =
        {
            "Collection Timeliness",
            "Delivery Timeliness",
            "Linen Quality",
            "Infection Control",
            "Stain / Odour Removal",
            "Damage / Loss",
            "SOP Compliance",
            "Staff Behavior",
            "Overall Satisfaction",
            "Warning Letters"
        };

        public WPRService(IWPRRepository repo)
        {
            _repo = repo;
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
                // ✅ Duplicate check — same week, month, year, staff
                bool exists = await _repo.WPRExistsAsync(
                    model.Week, model.Month, model.Year, model.StaffName.Trim());

                if (exists)
                    return (false, $"WPR for Week {model.Week} - {model.Month} {model.Year} is already submitted for {model.StaffName}.");

                int paymentPct = CalculatePaymentPercentage(model.TotalScore);

                var wpr = new WeeklyPerformanceReport
                {
                    Week = model.Week,
                    Month = model.Month,
                    Year = model.Year,
                    StaffName = model.StaffName.Trim(),
                    Remarks = model.Remarks?.Trim(),
                    TotalScore = model.TotalScore,
                    PaymentPercentage = paymentPct,
                    SubmittedAt = DateTime.Now
                };

                int wprId = await _repo.InsertWPRAsync(wpr);

                var details = model.Details.Select(d => new WPRDetail
                {
                    WPRId = wprId,
                    ParameterId = d.ParameterId,
                    ParameterName = ParameterNames[d.ParameterId - 1],
                    Score = d.Score
                });

                await _repo.InsertWPRDetailsAsync(details);

                return (true, "WPR submitted successfully.");
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
    }
}