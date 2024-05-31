using Graphs.Application.SpanningTrees;

namespace Graphs.Tests.Application.SpanningTrees;

public class ConstrainedSpanningTreeShould
{
    [Fact]
    public void ReturnFalseAndNullWhenNoEdges()
    {
        // Arrange
        var graph = new UndirectedVariableEdgeLengthGraph("empty");

        // Act
        (bool isExist, UndirectedVariableEdgeLengthGraph? tree) = ConstrainedSpanningTree.GetConstrainedSpanningTree(graph, 1);

        // Assert
        isExist.Should().BeFalse();
        tree.Should().BeNull();    
    }

    // strict graph Constrained_spanning_tree { 
    // 	"1" -- 		"3" [label = "1,00"];	 
    // 	"2" -- 		"4" [label = "2,00"];	"2" -- 		"7" [label = "2,00"];	 
    // 	"3" -- 		"1" [label = "1,00"];	"3" -- 		"5" [label = "1,00"];	"3" -- 		"6" [label = "2,00"];	"3" -- 		"7" [label = "2,00"];	 
    // 	"4" -- 		"2" [label = "2,00"];	"4" -- 		"7" [label = "1,00"];	 
    // 	"5" -- 		"3" [label = "1,00"];	 
    // 	"6" -- 		"3" [label = "2,00"];	 
    // 	"7" -- 		"2" [label = "2,00"];	"7" -- 		"4" [label = "1,00"];	"7" -- 		"3" [label = "2,00"];	 
    // }
    [Fact]
    public void ReturnTrueAndConstrainedSpanningTree()
    {
        // Arrange
        var graph = new UndirectedVariableEdgeLengthGraph("graph");
        graph.AddEdge(new(1) , new(3), 1);
        graph.AddEdge(new(2) , new(4), 2);
        graph.AddEdge(new(2) , new(7), 2);
        graph.AddEdge(new(3) , new(1), 1);
        graph.AddEdge(new(3) , new(5), 1);
        graph.AddEdge(new(3) , new(6), 2);
        graph.AddEdge(new(3) , new(7), 2);
        graph.AddEdge(new(4) , new(2), 2);
        graph.AddEdge(new(4) , new(7), 1);
        graph.AddEdge(new(5) , new(3), 1);
        graph.AddEdge(new(6) , new(3), 2);
        graph.AddEdge(new(7) , new(2), 2);
        graph.AddEdge(new(7) , new(4), 1);
        graph.AddEdge(new(7) , new(3), 2);

        var k = 3;

        // Act
        (bool isExist, UndirectedVariableEdgeLengthGraph? tree) = ConstrainedSpanningTree.GetConstrainedSpanningTree(graph, k);

        // Assert
        isExist.Should().Be(true); 
        tree.Should().NotBeNull();
        tree!.GetAllEdges().Should().HaveCount(12);
        tree!.GetAllEdges().Where(e => tree.GetEdgeLength(e.Item1, e.Item2) == 1).Should().HaveCount(6);
        tree!.GetAllEdges().Where(e => tree.GetEdgeLength(e.Item1, e.Item2) == 2).Should().HaveCount(6);
    }

    [Fact]
    public void ReturnFalseAndNullWhenKMoreThanLabeledEdges()
    {
        // Arrange
        var graph = new UndirectedVariableEdgeLengthGraph("graph");
        graph.AddEdge(new(1), new(3), 1);
        graph.AddEdge(new(2), new(4), 2);
        graph.AddEdge(new(2), new(7), 2);
        graph.AddEdge(new(3), new(1), 1);
        graph.AddEdge(new(3), new(5), 1);
        graph.AddEdge(new(3), new(6), 2);
        graph.AddEdge(new(3), new(7), 2);
        graph.AddEdge(new(4), new(2), 2);
        graph.AddEdge(new(4), new(7), 1);
        graph.AddEdge(new(5), new(3), 1);
        graph.AddEdge(new(6), new(3), 2);
        graph.AddEdge(new(7), new(2), 2);
        graph.AddEdge(new(7), new(4), 1);
        graph.AddEdge(new(7), new(3), 2);

        var k = 4;

        // Act
        (bool isExist, UndirectedVariableEdgeLengthGraph? tree) = ConstrainedSpanningTree.GetConstrainedSpanningTree(graph, k);

        // Assert
        isExist.Should().BeFalse();
        tree.Should().BeNull();
    }

    [Fact]
    public void ReturnFalseAndNullWhenKIsLessThanOne()
    {
        // Arrange
        var graph = new UndirectedVariableEdgeLengthGraph("empty");

        // Act
        (bool isExist, UndirectedVariableEdgeLengthGraph? tree) = ConstrainedSpanningTree.GetConstrainedSpanningTree(graph, 0);

        // Assert
        isExist.Should().BeFalse();
        tree.Should().BeNull();    
    } 

    [Fact]
    public void ReturnTrueAndConstrainedSpanningTreeWhenAllEdgesAreLabeledWithOne()
    {
        // Arrange
        var graph = new UndirectedVariableEdgeLengthGraph("graph");
        graph.AddEdge(new(1), new(3), 1);
        graph.AddEdge(new(2), new(4), 1);
        graph.AddEdge(new(2), new(7), 1);
        graph.AddEdge(new(3), new(1), 1);
        graph.AddEdge(new(3), new(5), 1);
        graph.AddEdge(new(3), new(6), 1);
        graph.AddEdge(new(3), new(7), 1);
        graph.AddEdge(new(4), new(2), 1);
        graph.AddEdge(new(4), new(7), 1);
        graph.AddEdge(new(5), new(3), 1);
        graph.AddEdge(new(6), new(3), 1);
        graph.AddEdge(new(7), new(2), 1);
        graph.AddEdge(new(7), new(4), 1);
        graph.AddEdge(new(7), new(3), 1);

        var k = 6;

        // Act
        (bool isExist, UndirectedVariableEdgeLengthGraph? tree) = ConstrainedSpanningTree.GetConstrainedSpanningTree(graph, k);

        // Assert
        isExist.Should().Be(true); 
        tree.Should().NotBeNull();
        tree!.GetAllEdges().Should().HaveCount(12);
        tree!.GetAllEdges().Where(e => tree.GetEdgeLength(e.Item1, e.Item2) == 1).Should().HaveCount(12);
    }

    [Fact]
    public void ReturnFalseAndNullWhenGraphIsNotConnected()
    {
        // Arrange
        var graph = new UndirectedVariableEdgeLengthGraph("graph");
        graph.AddEdge(new(1), new(3), 1);
        graph.AddEdge(new(2), new(4), 2);
        graph.AddEdge(new(2), new(7), 2);
        graph.AddEdge(new(3), new(1), 1);
        graph.AddEdge(new(3), new(5), 1);
        graph.AddEdge(new(3), new(6), 2);
        graph.AddEdge(new(4), new(2), 2);
        graph.AddEdge(new(4), new(7), 1);
        graph.AddEdge(new(5), new(3), 1);
        graph.AddEdge(new(6), new(3), 2);
        graph.AddEdge(new(7), new(2), 2);
        graph.AddEdge(new(7), new(4), 1);

        var k = 3;

        // Act
        (bool isExist, UndirectedVariableEdgeLengthGraph? tree) = ConstrainedSpanningTree.GetConstrainedSpanningTree(graph, k);

        // Assert
        isExist.Should().BeFalse();
        tree.Should().BeNull();
    }
}
