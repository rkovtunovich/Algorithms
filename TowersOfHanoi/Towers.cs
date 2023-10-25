using Helpers;

namespace TowersOfHanoi;

public class Towers
{
    public Towers(int hight)
    {
        First = new Tower(ArrayHelper.GetMonotonicArray(hight, 1, true), hight);
        Second = new Tower(new int[hight], 0);
        Third = new Tower(new int[hight], 0);
    }

    public Tower First { get; init; }

    public Tower Second { get; init; }

    public Tower Third { get; init; }

    public void Move()
    {
        MoveRec(First, Third, Second, First.Items.Length);
    }

    private void MoveRec(Tower from, Tower to, Tower aux, int count)
    {
        if(count == 0)
            return;
        else
        {
            MoveRec(from, aux, to, count - 1);
            to.Add(from.GetHigher());
            from.Remove();
            MoveRec(aux, to, from, count - 1);
        }     
    }
}
