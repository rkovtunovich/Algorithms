namespace MathAlgo.Common;

public static class Power
{
    public static int FastPower(int a, int b)
    {
        if (b == 1)
            return a;

        int c = a * a;
        int halfB = b >> 1;
        int res = FastPower(c, halfB);

        if (b % 2 == 0)
            return res;
        else
            return a * res;
    }
}

