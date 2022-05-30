namespace TowersOfHanoi;

public class Tower
{
    public Tower(int[] items, int quantity)
    {
        Quantity = quantity;
        Items = items;
    }

    public int[] Items { get; init; }

    public int Quantity { get; set; }

    public void Add(int item)
    {
        Items[Quantity] = item;
        Quantity++;
    }

    public void Remove()
    {
        Items[Quantity - 1] = 0;
        Quantity--;
    }

    public bool IsLast()
    {
        return Quantity == 1;
    }
    
    public int GetHigher()
    {
        return Items[Quantity - 1];
    }
}

