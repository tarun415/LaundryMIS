namespace LaudaryMis.Models
{
    // One saved edit of a WPR by the CMS (or, for a dispute, by the admin)
    public class WprEditCommand
    {
        public int WprId { get; set; }
        public int EditorId { get; set; }
        public string EditorRole { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public int? DisputeId { get; set; }

        public int OldTotal { get; set; }
        public int NewTotal { get; set; }
        public int NewPercentage { get; set; }
        public string NewGrade { get; set; } = string.Empty;

        public List<WprScoreChange> Changes { get; set; } = new();
    }

    public class WprScoreChange
    {
        public int ParameterId { get; set; }
        public string ParameterName { get; set; } = string.Empty;
        public int? OldScore { get; set; }
        public int NewScore { get; set; }
    }
}
