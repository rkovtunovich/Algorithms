using Graphs.Core.Abstraction;
using Graphs.Core.Model;

namespace SalesmanProblem;

// The Bellman-Held-Karp(BHK) algorithm is a dynamic programming approach to solve the Traveling Salesman Problem(TSP) in polynomial time.
// The TSP is an optimization problem where, given a list of cities and the distances between each pair of cities,
// the goal is to find the shortest possible route that visits each city exactly once and returns to the origin city.

// The BHK algorithm is not the most efficient method to solve the TSP, but it provides a good understanding of dynamic programming techniques.
// It has a time complexity of O(n^2 * 2^n), where n is the number of cities.

// Here's an outline of how the Bellman-Held-Karp algorithm works:
// 1. Define the set of cities: Let the cities be represented as a set of nodes, with each node indexed from 1 to n, where n is the number of cities.
// 2. Initialize the memoization table: Create a memoization table to store the optimal subproblem solutions.
//    The table has dimensions (n, 2^n), with the rows representing the cities and the columns representing the subsets of cities.
// 3. Define a recursive function: The function takes two arguments, the current city and the subset of unvisited cities,
//    and returns the shortest path length from the current city, visiting all unvisited cities in the subset, and finally returning to the origin city.
// 4. Base case: If the subset of unvisited cities is empty, return the distance from the current city back to the origin city.
// 5. Recursive case: If the subset of unvisited cities is not empty, iterate through the unvisited cities,
//    recursively calling the function for each city, and update the memoization table with the minimum distance of the current city to the next city plus the recursive call result.
//    The memoization table is used to store the optimal solution of subproblems and to avoid recomputation.
// 6. Return the result: After filling in the memoization table,
//    the shortest path length for the TSP can be obtained by looking at the entry corresponding to the origin city and the full set of unvisited cities (excluding the origin city).

// The BHK algorithm only provides the length of the shortest path, not the actual path itself.
// To reconstruct the path, you can backtrack through the memoization table and keep track of the optimal decisions made at each step.

// The bitwise technique used in the C# example is a compact way to represent and manipulate sets of integers using binary numbers.
// In this case, it's used to represent the set of visited cities in the Traveling Salesman Problem.
// Each city is assigned a unique position in a binary number, where the value at that position represents whether the city has been visited(1) or not(0).
// For instance, for a problem with 4 cities(A, B, C, D), the binary number 1010 would represent a set where cities A and C have been visited(positions with a 1),
// and cities B and D have not(positions with a 0).

// Here's a breakdown of the bitwise operations used in the C# example:
// 1. visitedSet == (1 << cityNumber) - 1: This checks if all cities have been visited.
//    The expression(1 << cityNumber) left-shifts the number 1 by the number of cities, effectively creating a binary number with 1's at all positions up to the number of cities.
//    Subtracting 1 changes all the 1's to 0's except for the positions representing the cities.
//    If visitedSet is equal to this value, all cities have been visited.
// 2. visitedSet & (1 << nextCity): This checks if the nextCity is in the visitedSet.
//    The expression (1 << nextCity) creates a binary number with a single 1 at the position corresponding to the nextCity.
//    The bitwise AND operation & between visitedSet and this binary number results in a non-zero value if the nextCity is visited
//    (i.e., its corresponding position in visitedSet has a 1), and zero otherwise.
// 3. visitedSet | (1 << nextCity): This adds the nextCity to the visitedSet.
//    The expression (1 << nextCity) creates a binary number with a single 1 at the position corresponding to the nextCity.
//    The bitwise OR operation | between visitedSet and this binary number sets the nextCity's position in visitedSet to 1, effectively marking it as visited.

// These bitwise techniques allow for efficient manipulation and comparison of sets,
// especially when dealing with problems like the Traveling Salesman Problem, where the number of elements in the set (cities)may not be very large.

public static class BellmanHeldKarp
{
    private  static Dictionary<(int, int), double> memo = new();

    // distances as int array
    public static double TSP(int[,] distances, int city, int visitedSet)
    {
        memo.Clear();

        int cityNumber = distances.GetLength(0);

        // for set with 4 cities
        // 1 << cityNumber = 16 (0000 00001) - 1 = 15 (1111)
        // 1 - mean that city is visited
        if (visitedSet == (1 << cityNumber) - 1)
            return distances[city, 0];

        if (memo.ContainsKey((city, visitedSet)))
            return memo[(city, visitedSet)];

        double minDistance = int.MaxValue;

        for (int nextCity = 0; nextCity < cityNumber; nextCity++)
        {
            if ((visitedSet & (1 << nextCity)) == 0)
            {
                double newDistance = distances[city, nextCity] + TSP(distances, nextCity, visitedSet | (1 << nextCity));
                minDistance = Math.Min(minDistance, newDistance);
            }
        }

        memo[(city, visitedSet)] = minDistance;

        return minDistance;
    }

    // search on graph
    public static double TSP(Graph graph, Vertex start)
    {
        memo.Clear();

        int n = graph.Count;
        return TSP(graph, start, start, (1 << n) - 1);
    }

    private static double TSP(Graph graph, Vertex origin, Vertex current, int unvisited)
    {
        // in the graph vertex indexes are started from 1
        // 15 => 7 => 3 => 1
        if (unvisited == 1 << (origin.Index - 1))
            return graph.GetEdgeLength(current, origin);

        if (memo.TryGetValue((current.Index, unvisited), out double value))
            return value;

        double minDistance = double.MaxValue;

        var connections = graph.GetEdges(current).Where(next => (unvisited & (1 << (next.Index - 1))) != 0);
        foreach (var next in connections)
        {
            double dist = graph.GetEdgeLength(current, next) + TSP(graph, origin, next, unvisited ^ (1 << (next.Index - 1)));
            minDistance = Math.Min(minDistance, dist);
        }

        memo[(current.Index, unvisited)] = minDistance;

        return minDistance;
    }
}

