using TowersOfHanoi;
using View;

namespace ExemplesRunning;

internal static class HanoiTowersExample
{
    public static void Run()
    {
        Towers towers = new(3);
        towers.Move();

        Viewer.ShowArray(towers.Third.Items); 
    }
}

