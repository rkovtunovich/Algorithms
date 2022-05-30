using Helpers;
using System.Diagnostics;

namespace MathAlgo
{
    public class Matrix
    {
        public static void Mult()
        {
            var m1 = MatrixHelper.CreateQuadratische<int>(512);
            var m2 = MatrixHelper.CreateQuadratische<int>(512);

            //var m1 = MatrixHelper.CreateSimple2x2(1, 4, 1, 3);
            //var m2 = MatrixHelper.CreateSimple2x2(3, 0, 2, 2);

            //var m1 = MatrixHelper.CreateSimple3x3(new[] { 1, 4, 5 }, new[] { 1, 3, 2}, new[] { 0, 2, 1 });
            //var m2 = MatrixHelper.CreateSimple3x3(new[] { 3, 0, 1 }, new[] { 2, 2, 0 }, new[] { 0, 1, 0 });

            MatrixHelper.FillRandomly(ref m1, 0, 5);
            MatrixHelper.FillRandomly(ref m2, 0, 5);

            //MatrixHelper.Show(m1);

            //MatrixHelper.Show(m2);

            //int[][] multilied = Mult(m1, m2);

            //int[][] multilied = MultRec(m1, m2);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int[][] multilied = MultStassen(m1, m2);
            stopWatch.Stop();

            Console.WriteLine(stopWatch.ElapsedMilliseconds);

            stopWatch.Restart();
            int[][] mu = Mult(m1, m2);
            stopWatch.Stop();

            Console.WriteLine(stopWatch.ElapsedMilliseconds);

            //MatrixHelper.Show(multilied);
        }

        public static int[][] Mult(int[][] m1, int[][] m2)
        {
            int size = m2.Length;
            int[][] matrix = new int[size][];

            for (int i = 0; i < m1.Length; i++)
            {
                matrix[i] = new int[size];
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        matrix[i][j] += m1[i][k] * m2[k][j];
                    }
                }
            }

            return matrix;
        }
        
        public static int[][] MultRec(int[][] m1, int[][] m2)
        {
            int size = m1.Length;
            
            if(size == 1)
            {
                int[][] res = new int[size][];
                res[0] = new int[size];
                res[0][0] = m1[0][0] * m2[0][0];
                return res;
            }

            int middle = size / 2;

            int[][] A = MatrixHelper.SubMatrix(m1, 0, 0, middle);
            int[][] B = MatrixHelper.SubMatrix(m1, 0, middle, size - middle);
            int[][] C = MatrixHelper.SubMatrix(m1, middle, 0, size - middle);
            int[][] D = MatrixHelper.SubMatrix(m1, middle, middle, size - middle);

            int[][] E = MatrixHelper.SubMatrix(m2, 0, 0, middle);
            int[][] F = MatrixHelper.SubMatrix(m2, 0, middle, size - middle);
            int[][] G = MatrixHelper.SubMatrix(m2, middle, 0, size - middle);
            int[][] H = MatrixHelper.SubMatrix(m2, middle, middle, size - middle);

            int[][] AE = MultRec(A, E);
            int[][] BG = MultRec(B, G);
            int[][] CE = MultRec(C, E);
            int[][] DG = MultRec(D, G);

            int[][] AF = MultRec(A, F);
            int[][] BH = MultRec(B, H);
            int[][] CF = MultRec(C, F);
            int[][] DH = MultRec(D, H);

            int[][] AEBG = Sum(AE, BG);
            int[][] CEDG = Sum(CE, DG);
            int[][] AFBH = Sum(AF, BH);
            int[][] CFDH = Sum(CF, DH);

            int[][] combined = MatrixHelper.Combime(AEBG, AFBH, CEDG, CFDH);

            return combined;
        }

        public static int[][] MultStassen(int[][] m1, int[][] m2)
        {
            int size = m1.Length;

            if (size == 1)
            {
                int[][] res = new int[size][];
                res[0] = new int[size];
                res[0][0] = m1[0][0] * m2[0][0];
                return res;
            }

            int middle = size / 2;

            int[][] A = MatrixHelper.SubMatrix(m1, 0, 0, middle);
            int[][] B = MatrixHelper.SubMatrix(m1, 0, middle, size - middle);
            int[][] C = MatrixHelper.SubMatrix(m1, middle, 0, size - middle);
            int[][] D = MatrixHelper.SubMatrix(m1, middle, middle, size - middle);

            int[][] E = MatrixHelper.SubMatrix(m2, 0, 0, middle);
            int[][] F = MatrixHelper.SubMatrix(m2, 0, middle, size - middle);
            int[][] G = MatrixHelper.SubMatrix(m2, middle, 0, size - middle);
            int[][] H = MatrixHelper.SubMatrix(m2, middle, middle, size - middle);

            int[][] FmH = Sub(F, H);
            int[][] ApB = Sum(A, B);
            int[][] CpD = Sum(C, D);
            int[][] GmE = Sub(G, E);
            int[][] ApD = Sum(A, D);
            int[][] EpH = Sum(E, H);
            int[][] BmD = Sub(B, D);
            int[][] GpH = Sum(G, H);
            int[][] AmC = Sub(A, C);
            int[][] EpF = Sum(E, F);

            int[][] P1 = MultStassen(A, FmH);
            int[][] P2 = MultStassen(ApB, H);
            int[][] P3 = MultStassen(CpD, E);
            int[][] P4 = MultStassen(D, GmE);
            int[][] P5 = MultStassen(ApD, EpH);
            int[][] P6 = MultStassen(BmD, GpH);
            int[][] P7 = MultStassen(AmC, EpF);

            int[][] qudrant11 = Sum(P5, P4);
            qudrant11 = Sub(qudrant11, P2);
            qudrant11 = Sum(qudrant11, P6);

            int[][] qudrant12 = Sum(P1, P2);

            int[][] qudrant21 = Sum(P3, P4);

            int[][] qudrant22 = Sum(P1, P5);
            qudrant22 = Sub(qudrant22, P3);
            qudrant22 = Sub(qudrant22, P7);

            int[][] combined = MatrixHelper.Combime(qudrant11, qudrant12, qudrant21, qudrant22);

            return combined;
        }

        public static int[][] Sum(int[][] m1, int[][] m2)
        {
            int size = m1.Length;
            int[][] res = new int[size][];

            for (int i = 0; i < m1.Length; i++)
            {
                res[i] = new int[size];
                for (int j = 0; j < size; j++)
                {
                    res[i][j] = m1[i][j] + m2[i][j];    
                }
            }

            return res;
        }

        public static int[][] Sub(int[][] m1, int[][] m2)
        {
            int size = m1.Length;
            int[][] res = new int[size][];

            for (int i = 0; i < m1.Length; i++)
            {
                res[i] = new int[size];
                for (int j = 0; j < size; j++)
                {
                    res[i][j] = m1[i][j] - m2[i][j];
                }
            }

            return res;
        }
    }
}
