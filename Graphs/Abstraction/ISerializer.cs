namespace Graphs.Abstraction;

public interface ISerializer
{
    public string Serialize(Graph graph); 

    public Graph Deserialize(string serializedGraph);
}