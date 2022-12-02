using Organization.Models.Organization.Graphs;
using Organization.Models.Organization.Project;

namespace Organization.Controllers.DTO;

 public record Graph
 {
    public Graph(Models.Organization.Graphs.GraphAggregator graph)
    {
        DateChartValues = graph.GraphType == "Burndown" ? graph.DateChartValues.ToList() : null;
        SprintChartValues = graph.GraphType == "TeamSpeed" ? graph.SprintChartValues.ToList() : null;
        GraphType   = graph.GraphType;
    }

    public List<DateTimePoint> DateChartValues { get; }
    public List<SprintTimePoint> SprintChartValues { get; }
    public string       GraphType   { get; }
}