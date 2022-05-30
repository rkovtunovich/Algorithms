using Models.Space;

namespace Helpers.Space;

public static class SpaceHelper
{
    public static Point2D[] GetSetOf2DPoints(int count, int maxX, int maxY)
    {
        var set = new HashSet<Point2D>();

        var points = new Point2D[count];

        var random = new Random();

        for (int i = 0; i < count; i++)
        {
            var point = new Point2D(random.Next(maxX), random.Next(maxY));

            if(set.Contains(point))
            {
                i--;
                continue;
            }

            points[i] = point;
            set.Add(point);
        }

        return points;
    }
    
    public static void Show2DPoints(Point2D[] points)
    {
        foreach (var point in points)
        {
            Console.WriteLine(point);
        }

        Console.WriteLine();
    }
}
