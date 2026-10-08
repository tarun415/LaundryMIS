namespace LaudaryMis.Helpers
{
    // The ten WPR parameters of the contract (ParameterId 1..10) and the payment bands of the total score
    public static class WprParameters
    {
        public const int MaxScore = 10;

        public static readonly string[] Names =
        {
            "Hospital Linen Quality After Wash",
            "Timely Supply of Clean & Washed Linen",
            "Washing Procedure/Formula & Quality Consumables Followed",
            "Laundry Personnel Displaying Photo ID Card",
            "Behavior of Laundry Personnel Towards Hospital Staff/Patient",
            "Personnel Found Smoking/Drinking/Sleeping During Duty",
            "Personnel Found on Duty Other Than Approved List",
            "Breakdown of Equipment",
            "Alternate Arrangement During Equipment Breakdown",
            "Number of Warning Letters Issued by Hospital Authority"
        };

        public static string NameOf(int parameterId) =>
            parameterId >= 1 && parameterId <= Names.Length ? Names[parameterId - 1] : $"Parameter {parameterId}";

        // WPRDetail.ParameterName holds 50 characters; screens always show the full NameOf()
        public static string StoredName(int parameterId)
        {
            var name = NameOf(parameterId);
            return name.Length <= 50 ? name : name[..50];
        }

        // 0-20 nil, 21-40 40%, 41-60 60%, 61-70 80%, 71-80 90%, 81-100 100%
        public static int PaymentPercentage(int totalScore) => totalScore switch
        {
            <= 20 => 0,
            <= 40 => 40,
            <= 60 => 60,
            <= 70 => 80,
            <= 80 => 90,
            _ => 100
        };

        public static string Grade(int totalScore) =>
            PaymentPercentage(totalScore) == 0 ? "No Payment" : PaymentPercentage(totalScore) + "% Payment";

        // Weeks the hospital has to report for a month: days 1-7, 8-14, 15-21, 22-28 and, when the month has
        // more than 28 days, a fifth week for the rest. Same split the WPR form uses.
        public static int WeeksInMonth(int month, int year) =>
            DateTime.DaysInMonth(year, month) > 28 ? 5 : 4;
    }
}
