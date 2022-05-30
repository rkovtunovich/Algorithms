namespace Finding
{
    public static class LocalMatrixMinimum
    {
        public static List<int[]> Find(int[][] matrix)
        {
            var mins = new List<int[]>();

            FindRec(matrix, 0, matrix.Length - 1, 0, matrix.Length - 1, mins);

            return mins;
        }

        private static void FindRec(int[][] matrix, int rowStart, int rowEnd, int colStart, int colEnd, List<int[]> mins)
        {
            if(rowEnd - rowStart == 0 && colEnd - colStart == 0)
            {
                mins.Add(new[] { rowStart, colStart, matrix[rowStart][colStart] });
                return;
            }

            int middleRow = rowStart + rowEnd / 2;
            int middleCol = colStart + colEnd / 2;
            int row = 0, col = 0;
            int min = matrix[middleRow][middleCol];

            // finding the miminum value on the cross
            for (int i = colStart; i <= colEnd; i++)
            {
                if (matrix[middleRow][i] < min)
                {
                    min = matrix[middleRow][i];
                    col = i;
                    row = middleRow;
                }
            }

            for (int i = rowStart; i <= rowEnd; i++)
            {
                if (matrix[i][middleCol] < min)
                {
                    min = matrix[i][middleCol];
                    col = middleCol;
                    row = i;
                }
            }

            //finding the minimum value among neighbors

            // the center of the cross is local minimum
            if (matrix[middleRow][middleCol] == min)
            {
                mins.Add(new[] { middleRow, middleCol, min });
                return;
            }

            int quadrant = 0;

            // on the horizontal
            if (row == middleRow)
            {
                if (rowStart <= row - 1 && matrix[row - 1][col] < min)
                {
                    quadrant = col < middleCol ? 1 : 2;
                }
                else if (rowEnd >= row + 1 && matrix[row + 1][col] < min)
                {
                    quadrant = col < middleCol ? 3 : 4;
                }         
            }

            // on the vertical
            if (col == middleCol)
            {
                if (colStart <= col - 1 && matrix[row][col - 1] < min)
                {
                    quadrant = row < middleRow ? 1 : 3;
                } 
                else if (colEnd >= col + 1 && matrix[row][col + 1] < min)
                {
                    quadrant = row < middleRow ? 2 : 4;
                }
            }

            // local mimimum is on the cross
            if (quadrant == 0)
            {
                mins.Add(new[] { row, col, min });
                return;
            }

            switch (quadrant)
            {
                case 1:
                    {
                        FindRec(matrix, rowStart, middleRow - 1, colStart, middleCol - 1, mins);
                        break;
                    }
                case 2:
                    {
                        FindRec(matrix, rowStart, middleRow - 1, middleCol + 1, colEnd, mins);
                        break;
                    }
                case 3:
                    {
                        FindRec(matrix, middleRow + 1, rowEnd, colStart, middleCol - 1, mins);
                        break;
                    }
                case 4:
                    {
                        FindRec(matrix, middleRow + 1, rowEnd, middleCol + 1, colEnd, mins);
                        break;
                    }
            }
        }
    }
}