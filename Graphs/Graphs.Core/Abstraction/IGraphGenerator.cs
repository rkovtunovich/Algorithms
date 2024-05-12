using Graphs.Core.Model.Graphs;

namespace Graphs.Core.Abstraction;

public interface IGraphGenerator
{
    public GraphBase Generate(string name);
}
