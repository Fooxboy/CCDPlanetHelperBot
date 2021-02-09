namespace CCDPlanetHelper.Database
{
    public class Report
    {
        public long ReportId { get; set; }
        public string Message { get; set; }
        public long OwnerId { get; set; }
        public long ModeratorId { get; set; }
        public bool IsAnswered { get; set; }
        public string ModeratorReply { get; set; }
    }
}