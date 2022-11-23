namespace Organization.Controllers.DTO;

 public record Graph
 {
     public Graph(Models.Organization.Graphs.GraphAggregator graph)
     {
         ChartValues = graph.ChartValues.ToList();
         XLabels     = graph.XLabels.ToList();
         YValues     = graph.YValues.ToList();
         GraphType   = graph.GraphType;

        foreach (var chartValue in ChartValues) { ChartValues.Add(chartValue); }
    }

     public List<double> ChartValues { get; }
     public List<string> XLabels     { get; }
     public List<double> YValues     { get; }
     public string       GraphType   { get; }
}