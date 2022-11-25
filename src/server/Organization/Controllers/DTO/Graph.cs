using Organization.Models.Organization.Graphs;
using Organization.Models.Organization.Project;

namespace Organization.Controllers.DTO;

 public record Graph
 {
    public Graph(Models.Organization.Graphs.GraphAggregator graph)
    {
        ChartValues = graph.ChartValues.ToList();
        GraphType   = graph.GraphType;
    }

     public List<DateTimePoint> ChartValues { get; }
     public string       GraphType   { get; }
}