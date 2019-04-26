// Houston Tyler Webb
//
// This is a child class of Map that creates a 2D map of a cave.
// The algorithm featured here was inspired by RogueBasin.com
using System;

namespace ProcGenSharp
{
    public class CaveMap : Map
    {
        // Nullable grid for storing our changes as we traverse
        private char?[,] DevelopGrid;

        // Parameterized Constructor
        public CaveMap(int height, int width) : base(height, width)
        {
            // Initialize development grid
            DevelopGrid = new char?[height + 2, width + 2];

            // Randomly fill the grid with walls and empty spaces
            TraverseWith((x, y) => {Grid[y, x] = Rng.Next() % 2 == 0 ? '.' : '#';});

            // Iterate with the cellular automata pattern 4 times.
            for (int i = 0; i < 4; i++)
            {
                TraverseWith(PrimeDevelop);
                TraverseWith((x, y) => {Grid[y, x] = DevelopGrid[y, x];});
            }
        }

        // Is called on each grid point
        private void PrimeDevelop(int x, int y)
        {
            // Count the number of walls surrounding (and including) the point
            int wallCount = 0;
            for (int i = y - 1; i < y + 1; i++)
            {
                for (int j = x - 1; j < x + 1; j++)
                    wallCount += (Grid[i,j] == '#' || Grid[i,j] == null ? 1 : 0);
            }

            // If there are many walls or very few walls, become a wall.
            DevelopGrid[y, x] = wallCount > 4 || wallCount <= 2 ? '#' : '.';
        }
    }
}

