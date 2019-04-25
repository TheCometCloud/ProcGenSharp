using System;

public class Map
{
    public char?[,] Grid{get; set;}
    protected Random Rng;

    public Map(int height, int width)
    {
        Grid = new char?[height + 2, width + 2];
        Rng = new Random();
        FillGrid();
        Console.WriteLine(this);
    }

    public void FillGrid(char c = '?')
    {
        TraverseWith((x, y) => {Grid[x, y] = c;});
    }

    public void TraverseWith(Action<int, int> innerAction, Action<int> outerAction = null)
    {
        for (int i = 1; i < Grid.GetLength(0) -1; i++)
        {
            for (int j = 1; j < Grid.GetLength(1) - 1; j++)
                innerAction(j, i);

            if (outerAction != null)
                outerAction(i);
        }
    }

    public void SubTraverseWith(int x1, int x2, int y1, int y2, Action<int, int> innerAction, Action<int> outerAction = null)
    {
        for (int i = y1; i <= y2; i++)
        {
            for (int j = x1; j <= x2; j++)
            {
                innerAction(j, i);
            }

            outerAction(i);
        }
    }

    public override string ToString()
    {
        string output = "";
        TraverseWith((x, y) => {output += Grid[x, y] + " ";}, (x) => {output += '\n';});

        return output;
    }
}