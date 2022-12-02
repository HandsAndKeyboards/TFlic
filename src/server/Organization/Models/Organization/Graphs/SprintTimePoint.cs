using Organization.Models.Organization.Project;

namespace Organization.Models.Organization.Graphs
{
    public class SprintTimePoint
    {
        public SprintTimePoint(Sprint sprint, ulong estimated, ulong real)
        {
            this.Sprint = sprint;
            this.Estimated = estimated;
            this.Real = real;
        }

        public Sprint Sprint { get; set; }
        public ulong Estimated { get; set; }
        public ulong Real { get; set; }
    }
}
