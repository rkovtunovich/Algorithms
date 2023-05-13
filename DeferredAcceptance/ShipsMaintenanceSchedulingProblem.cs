namespace DeferredAcceptance;

// Peripatetic Shipping Lines, Inc., is a shipping company that owns n ships and provides service to n ports.
// Each of its ships has a schedule that says, for each day of the month, which of the ports it's currently visiting, or whether it's out at sea.
// (You can assume the "month" here has m days, for some m > n.)
// Each ship visits each port for exactly one day during the month. For safety reasons, PSL Inc. has the following strict requirement:
// (t) No two ships can be in the same port on the same day.
// The company wants to perform maintenance on all the ships this month, via the following scheme. They want to truncate each ship's schedule: for each ship Si, 
// there will be some day when it arrives in its scheduled port and simply remains there for the rest of the month (for maintenance). This means that Si
//  will not visit the remaining ports on its schedule (if any) that month, but this is okay.
//  So the truncation of Si schedule will simply consist of its original schedule up to a certain specified day on which it is in a port P
//  the remainder of the truncated schedule simply has it remain in port P
// 
// Now the company's question to you is the following:
// Given the schedule for each ship, find a truncation of each so that condition (t) continues to hold:
// no two ships are ever in the same port on the same day.
// Show that such a set of truncations can always be found, and give an algorithm to find them.
//
// Example. Suppose we have two ships and two ports, and the "month" has four days. Suppose the first ship's schedule is (P1, at sea, P2, at sea) 
// and the second ship's schedule is (at sea, P1, at sea P2)
// Then the (only) way to choose truncations would be to have the first ship remain in port P2 starting on day 3,
// and have the second ship remain in port P1 starting on day 2

public class ShipsMaintenanceSchedulingProblem
{
    // The main function to find the truncated schedule
    public static Dictionary<int, (int ship, int day)> FindTruncation(int[,] schedule)
    {
        // The length of the month
        int monthLength = schedule.GetLength(1);

        // The number of ports
        int portsNumber = schedule.GetLength(0);

        // The number of ships
        int shipsNumber = schedule.GetLength(0);

        // The dictionary to keep track of which ship is in which port for maintenance
        var ported = new Dictionary<int, (int ship, int day)>();

        // The queue to manage the ships that have not yet been assigned to a port for maintenance
        var notPorted = new Queue<(int ship, int day)>();

        // Initially, all ships are not ported, and their current day is 0
        for (int ship = 0; ship < shipsNumber; ship++)
        {
            notPorted.Enqueue((ship, 0));
        }

        // While there are ships that have not been assigned to a port for maintenance
        while (notPorted.Count > 0)
        {
            // Get the first ship and its current day from the queue
            (int ship, int currentDay) = notPorted.Dequeue();

            // Get the port for the ship on the current day
            var portOnDay = schedule[ship, currentDay];

            // If the ship is at sea on the current day (i.e., not in a port)
            if (portOnDay is 0)
            {
                // The ship is still not ported, increment the day and put it back to the queue
                notPorted.Enqueue((ship, currentDay + 1));
                continue;
            }

            // If the ship is in a port on the current day
            int currentPort = schedule[ship, currentDay];

            // If the port is already occupied by another ship
            if (ported.ContainsKey(currentPort))
            {
                // Get the other ship and its current day
                (int otherShip, int otherDay) = ported[currentPort];

                // The other ship is now not ported, increment the day and put it back to the queue
                notPorted.Enqueue((otherShip, otherDay + 1));
            }

            // Assign the ship to the port for maintenance
            ported[currentPort] = (ship, currentDay);
        }

        // Return the final truncated schedule
        return ported;
    }
}
