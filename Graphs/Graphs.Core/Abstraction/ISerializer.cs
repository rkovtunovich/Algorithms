using Graphs.Core.Model.Graphs;

namespace Graphs.Core.Abstraction;

public interface ISerializer
{
    public string Serialize(GraphBase graph);

    public GraphBase Deserialize(string serializedGraph);
}