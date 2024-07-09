namespace DataStructures.Heaps;

public class YoungTableauMin<TValue> where TValue : INumber<TValue>
{
    private int _n;
    private int _m;

    private readonly TValue _infinity;
    private readonly TValue[,] _tableau;

    public YoungTableauMin(int n, int m, TValue infinity)
    {
        _n = n;
        _m = m;
        _infinity = infinity;
        _tableau = new TValue[_n, _m];

        Initialize();
    }

    public void Insert(TValue value)
    {
        if (_tableau[_n - 1, _m - 1] != _infinity)       
            throw new InvalidOperationException("Young tableau is full.");
        
        _tableau[_n - 1, _m - 1] = value;

        BubbleUp(_n - 1, _m - 1);
    }

   
    public TValue ExtractMin()
    {
        if (_tableau[0, 0] == _infinity)
            throw new InvalidOperationException("Young tableau is empty.");

        var min = _tableau[0, 0];
        _tableau[0, 0] = _infinity;

        BubbleDown(0, 0);

        return min;
    }

    public bool Search(TValue value)
    {
        var x = 0;
        var y = _m - 1;

        while (x < _n && y >= 0)
        {
            if (_tableau[x, y] == value)
                return true;

            if (_tableau[x, y] < value)
                x++;
            else
                y--;
        }

        return false;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var i = 0; i < _n; i++)
        {
            for (var j = 0; j < _m; j++)         
                sb.Append(_tableau[i, j] + " ");
           
            sb.AppendLine();
        }

        return sb.ToString();
    }

    #region Private Methods
   
    private void Initialize()
    {
        for (var i = 0; i < _n; i++)
            for (var j = 0; j < _m; j++)
                _tableau[i, j] = _infinity;
    }

    private void BubbleUp(int i, int j)
    {
        int x = i, y = j;
        while (true)
        {
            int smallestX = x, smallestY = y;

            if (x > 0 && _tableau[x - 1, y] > _tableau[smallestX, smallestY])
            {
                smallestX = x - 1;
                smallestY = y;
            }

            if (y > 0 && _tableau[x, y - 1] > _tableau[smallestX, smallestY])
            {
                smallestX = x;
                smallestY = y - 1;
            }

            if (smallestX == x && smallestY == y)
                break;

            (_tableau[smallestX, smallestY], _tableau[x, y]) = (_tableau[x, y], _tableau[smallestX, smallestY]);
            x = smallestX;
            y = smallestY;
        }
    }

    private void BubbleDown(int i, int j)
    {
        int x = i, y = j;
        while (true)
        {
            int largestX = x, largestY = y;

            if (x + 1 < _m && _tableau[x + 1, y] < _tableau[largestX, largestY])
            {
                largestX = x + 1;
                largestY = y;
            }

            if (y + 1 < _n && _tableau[x, y + 1] < _tableau[largestX, largestY])
            {
                largestX = x;
                largestY = y + 1;
            }

            if (largestX == x && largestY == y)
                break;

            (_tableau[largestX, largestY], _tableau[x, y]) = (_tableau[x, y], _tableau[largestX, largestY]);
            x = largestX;
            y = largestY;
        }
    }

    #endregion
}
