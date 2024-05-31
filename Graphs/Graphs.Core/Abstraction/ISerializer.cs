using Graphs.Core.Model.Graphs;
using Graphs.Core.Model.Serialization;

namespace Graphs.Core.Abstraction;

public interface ISerializer
{
    public string Serialize(GraphBase graph);

    public GraphBase Deserialize(string serializedGraph, DeserializationOptions? options = null);
}