using DataStructures.Lists;

namespace Graphs.Application.Search;

// The Havel-Hakimi algorithm is a mathematical theorem that determines whether a given degree sequence can be represented by a simple graph.
// A degree sequence is a list of numbers that represent the degree of each node in a graph.
// Time Complexity: O(n^2)
// Space Complexity: O(1)
// The algorithm works as follows:
// 1. Sort the Degree Sequence: Start by sorting the degree sequence in non-increasing order.
// 2. Remove the Largest Degree: Remove the largest degree 𝑑 from the sequence.
//    This degree corresponds to a node that must be connected to other nodes.
// 3. Reduce the Remaining Degrees: Reduce the next degrees by 1, representing the connections made with the removed node.
//    If any of these degrees become negative, the sequence is not graphical.
// 4. Repeat: Repeat the process with the remaining sequence until either the sequence is empty (indicating a successful construction of the graph)
//    or a contradiction is found (indicating that the sequence is not graphical).
public class HavelHakimiAlgorithm
{
    public static bool IsGraphical(SequentialList<int> sequence)
    {
        // Sort the degree sequence in non-increasing order.
        sequence.Sort(false);

        // Repeat the process until the sequence is empty.
        while (sequence.Count > 0)
        {
            // Remove the largest degree from the sequence.
            int degree = sequence[0];
            sequence.RemoveAt(0);

            // If the degree is negative, the sequence is not graphical.
            if (degree < 0)
                return false;

            // Reduce the next degrees by 1.
            for (int i = 0; i < degree; i++)
            {
                if (sequence.Count == 0)
                    return false;

                sequence[i]--;
            }

            // Sort the sequence in non-increasing order.
            sequence.Sort(false);
        }

        return true;
    }
}
