using System;

public class CaveMap : Map
{
    private char?[,] DevelopGrid;

    public CaveMap(int height, int width) : base(height, width)
    {
        DevelopGrid = new char?[height + 2, width + 2];

        TraverseWith((x, y) => {Grid[y, x] = Rng.Next() % 2 == 0 ? '.' : '#';});
        Console.WriteLine(this);

        for (int i = 0; i < 4; i++)
        {
            TraverseWith(PrimeDevelop);
            TraverseWith((x, y) => {Grid[y, x] = DevelopGrid[y, x];});
        }
    }

    private void PrimeDevelop(int x, int y)
    {
        int wallCount = 0;
        for (int i = y - 1; i < y + 1; i++)
        {
            for (int j = x - 1; j < x + 1; j++)
                wallCount += (Grid[i,j] == '#' || Grid[i,j] == null ? 1 : 0);
        }
        DevelopGrid[y, x] = wallCount > 4 || wallCount <= 2 ? '#' : '.';
    }
}
