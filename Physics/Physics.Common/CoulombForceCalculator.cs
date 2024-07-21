namespace Physics.Common;

// Given an array of charges in a 1-dimensional lattice,
// we need to compute the total force on each particle due to all other particles in the lattice.
// The force between two particles i and j is inversely proportional to the square of the distance between them.
// The formula for the force between two particles is given
// by the Coulomb's law: F = k * q1 * q2 / r^2, where k is a constant, q1 and q2 are the charges of the particles,
// and r is the distance between the particles.
// The force on a particle is the sum of the forces between that particle and all other particles in the lattice.
// The force on a particle i due to all other particles is given by the formula:
// F(j) = Σ(i<j) (k * q(i) * q(j) / (i - j)^2) - Σ(i>j) (k * q(i) * q(j) / (i - j)^2)
public class CoulombForceCalculator(double constant, double[] charges)
{
    public double[] ComputeForces()
    {
        var forces = new double[charges.Length];

        CalculateForceRecursive(forces, 0, charges.Length - 1);

        return forces;
    }

    private void CalculateForceRecursive(double[] forces, int left, int right)
    {
        // Base case: if the range contains only one particle, the force is zero
        if (left == right)
        {
            forces[left] = 0.0;
            return;
        }

        // Calculate the middle index
        var middle = (left + right) / 2;

        // Calculate the force on the left half of the range
        CalculateForceRecursive(forces, left, middle);

        // Calculate the force on the right half of the range
        CalculateForceRecursive(forces, middle + 1, right);

        // Combine the forces from the left and right halves
        CombineForces(forces, left, middle, right);
    }

    private void CombineForces(double[] forces, int left, int middle, int right)
    {
        // Calculate the force on the left half of the range
        for (int i = left; i <= middle; i++)        
            forces[i] -= CalculateForce(middle + 1, right, i);    

        // Calculate the force on the right half of the range
        for (int i = middle + 1; i <= right; i++)        
            forces[i] += CalculateForce(left, middle, i);        
    }

    private double CalculateForce(int left, int right, int current)
    {
        var force = 0.0;

        // calculate force between the current particle and all particles in the given range
        for (int i = left; i <= right; i++)
        {
            var distance = (i - current);
            var charge1 = charges[i];
            var charge2 = charges[current];

            // Calculate the force between the current particle and particle i
            var currentForce = constant * charge1 * charge2 / Math.Pow(distance, 2);

            force += currentForce;
        }

        return force;
    }
}
