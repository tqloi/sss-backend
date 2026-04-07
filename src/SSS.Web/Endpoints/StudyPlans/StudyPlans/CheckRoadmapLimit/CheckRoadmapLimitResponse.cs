namespace SSS.Web.Endpoints.StudyPlans.StudyPlans.CheckRoadmapLimit
{
    public class CheckRoadmapLimitResponse
    {
        public int MaxRoadmaps { get; set; }
        public int JoinedRoadmaps { get; set; }
        public bool HasReachedLimit { get; set; }
    }
}
