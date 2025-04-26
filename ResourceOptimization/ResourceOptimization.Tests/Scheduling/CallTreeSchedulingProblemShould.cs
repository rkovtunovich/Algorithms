using Graphs.Core.Model;
using Graphs.Core.Model.Graphs;
using ResourceOptimization.Scheduling;

namespace ResourceOptimization.Tests.Scheduling;

public class CallTreeSchedulingProblemShould
{
    [Fact]
    public void GetOptimalSchedule_WhenSingleNode_ReturnsEmptyCalls()
    {
        // Arrange
        var graph = new OrientedGraph("SingleNode", trackIncomeEdges: true);

        // Create a single node (ranking officer)
        var root = new Vertex(1);
        graph.AddVertex(root);

        // Fill incoming edges
        graph.FillIncomeEdges();

        // Act
        var calls = CallTreeSchedulingProblem.GetOptimalSchedule(graph);

        // Assert
        // No subordinates => no calls
        calls.Should().BeEmpty("with only one node, no phone calls are needed.");
    }

    [Fact]
    public void GetOptimalSchedule_WhenTwoLevelHierarchy_ReturnsCorrectSchedule()
    {
        // Arrange
        var graph = new OrientedGraph("TwoLevel", trackIncomeEdges: true);

        // Create a root (ranking officer) and two children
        var root = new Vertex(1);
        var childA = new Vertex(2);
        var childB = new Vertex(3);

        graph.AddVertices(root, childA, childB);
        // Root calls childA and childB
        graph.AddEdge(root, childA);
        graph.AddEdge(root, childB);

        // Act
        var calls = CallTreeSchedulingProblem.GetOptimalSchedule(graph);

        // Assert
        calls.Should().HaveCount(2, "the root must call each child exactly once.");
        calls.Should().ContainInOrder((root, childA), (root, childB));
    }

    [Fact]
    public void GetOptimalSchedule_WhenBalancedTree_GeneratesMinRounds()
    {
        // Arrange
        // Create a balanced tree:
        //      (1)
        //     /   \
        //   (2)   (3)
        //   / \   / \
        // (4)(5) (6)(7)
        var graph = new OrientedGraph("BalancedTree", trackIncomeEdges: true);

        var v1 = new Vertex(1);
        var v2 = new Vertex(2);
        var v3 = new Vertex(3);
        var v4 = new Vertex(4);
        var v5 = new Vertex(5);
        var v6 = new Vertex(6);
        var v7 = new Vertex(7);

        graph.AddVertices(v1, v2, v3, v4, v5, v6, v7);

        graph.AddEdge(v1, v2);
        graph.AddEdge(v1, v3);
        graph.AddEdge(v2, v4);
        graph.AddEdge(v2, v5);
        graph.AddEdge(v3, v6);
        graph.AddEdge(v3, v7);

        // Act
        var calls = CallTreeSchedulingProblem.GetOptimalSchedule(graph);

        // Assert
        // Total calls = number of edges = 6
        calls.Should().HaveCount(6);
        calls.Should().BeEquivalentTo([
            (v1, v2), (v1, v3),
            (v2, v4), (v2, v5),
            (v3, v6), (v3, v7)]
        );
    }

    [Fact]
    public void GetOptimalSchedule_WhenUnbalancedTree_PrioritizesHeavierSubtree()
    {
        // Arrange
        // For example:
        //       (1)
        //       /  \
        //     (2)  (5)
        //     / \
        //   (3) (4)
        // Suppose (2)->(3)->(4) is a deeper chain.
        var graph = new OrientedGraph("UnbalancedTree", trackIncomeEdges: true);

        var v1 = new Vertex(1);
        var v2 = new Vertex(2);
        var v3 = new Vertex(3);
        var v4 = new Vertex(4);
        var v5 = new Vertex(5);

        graph.AddVertices(v1, v2, v3, v4, v5);

        // Edges
        graph.AddEdge(v1, v2);
        graph.AddEdge(v1, v5);
        graph.AddEdge(v2, v3);
        graph.AddEdge(v3, v4);

        // Act
        var calls = CallTreeSchedulingProblem.GetOptimalSchedule(graph);

        // Assert
        // Check total calls
        calls.Should().HaveCount(4);

        // The minimal number of rounds depends on calling the deeper chain first (2->3->4),
        // verifying the largest-subtree-first principle.
        calls.Should().ContainInOrder((v1, v2), (v2, v3), (v3, v4), (v1, v5));
    }

    [Fact]
    public void GetOptimalSchedule_WhenComplexHierarchy_CompletesAllCalls()
    {
        // Arrange
        // Construct a more complex hierarchy with root = v1
        var graph = new OrientedGraph("ComplexHierarchy", trackIncomeEdges: true);
        var v1 = new Vertex(1);
        var v2 = new Vertex(2);
        var v3 = new Vertex(3);
        var v4 = new Vertex(4);
        var v5 = new Vertex(5);
        var v6 = new Vertex(6);
        var v7 = new Vertex(7);

        graph.AddVertices(v1, v2, v3, v4, v5, v6, v7);

        // Suppose:
        // 1 -> 2, 3, 4
        // 2 -> 5, 6
        // 3 -> 7
        graph.AddEdge(v1, v2);
        graph.AddEdge(v1, v3);
        graph.AddEdge(v1, v4);
        graph.AddEdge(v2, v5);
        graph.AddEdge(v2, v6);
        graph.AddEdge(v3, v7);

        // Act
        var calls = CallTreeSchedulingProblem.GetOptimalSchedule(graph);

        // Assert
        // # calls = # edges = 6
        calls.Should().HaveCount(6);

        // Optionally verify calls contain each edge exactly once:
        var edgesSet = calls.ToHashSet();
        edgesSet.Should().BeEquivalentTo(new HashSet<(Vertex, Vertex)> {
            (v1, v2), (v1, v3), (v1, v4),
            (v2, v5), (v2, v6),
            (v3, v7)
        });
    }
}
