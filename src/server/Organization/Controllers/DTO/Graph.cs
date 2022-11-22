namespace Organization.Controllers.DTO;

public record Graph
{
    public Graph(Models.Organization.Graphs.GraphAggregator graph)
    {
        ChartValues = graph.ChartValues;
        XLabels     = graph.XLabels;
        YValues     = graph.YLabels;
        GraphType   = graph.GraphType;
    }

    public List<double> ChartValues { get; }
    public List<string> XLabels     { get; }
    public List<double> YValues     { get; }
    public string       GraphType   { get; }
}