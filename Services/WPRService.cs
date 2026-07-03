using LaudaryMis.Models;
using LaudaryMis.Repositories;
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
                    ParameterName = ParameterNames[x.ParameterId - 1],
                    Score = x.Score
                }).ToList();

                await _repo.SaveWPRAsync(report, entry, details);

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

        public async Task<bool> CheckWeeklyVerification(int weekNo, int month, int year)
        {
            return await _repo.CheckWeeklyVerification(weekNo, month, year);
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
    }
}