namespace MathAlgo.Common
{
    internal static class KaratsubaMult
    {
        internal static int Mult(int x, int y)
        {
            return RecMult(x, y);
        }

        private static int RecMult(int x, int y)
        {
            if (x < 10 && y < 10)
            {
                return x * y;
            }

            string xStr = x.ToString();
            string yStr = y.ToString();

            string a, b, c, d;

            if (x < 10)
            {
                a = "0";
                b = xStr;
            }
            else
            {
                a = xStr.Substring(0, xStr.Length / 2);
                b = xStr.Substring(xStr.Length / 2);
            }

            if (y < 10)
            {
                c = "0";
                d = yStr;
            }
            else
            {
                c = yStr.Substring(0, yStr.Length / 2);
                d = yStr.Substring(yStr.Length / 2);
            }

            int p = int.Parse(a) + int.Parse(b);
            int q = int.Parse(c) + int.Parse(d);

            int n = Math.Max(xStr.Length, yStr.Length);
            int m = n / 2 + n % 2;

            Console.WriteLine($"x {a}.{b} : y {c}.{d} |{n}:{n / 2 + n % 2}");

            int ac = RecMult(int.Parse(a), int.Parse(c));
            int bd = RecMult(int.Parse(b), int.Parse(d));
            int pq = RecMult(p, q);

            int abcd = pq - ac - bd;

            int res = (int)Math.Pow(10, n) * ac + (int)Math.Pow(10, m) * abcd + bd;

            return res;
        }
    }
}
