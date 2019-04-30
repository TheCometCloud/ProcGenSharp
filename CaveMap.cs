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
        protected char?[,] DevelopGrid;

        // Parameterized Constructor
        public CaveMap(int height, int width) : base(height, width)
        {
            // Initialize development grid
            DevelopGrid = new char?[height + 2, width + 2];

            // Randomly fill the grid with walls and empty spaces
            TraverseWith((tile) => {tile.character = Rng.Next(100) < 40 ? Wall : Empty;});
            WallGrid();
            Excavate();
        }

        public virtual void Excavate()
        {
            // Iterate with the cellular automata pattern 4 times.
            for (int i = 0; i < 4; i++)
            {
                // Perform the transformations
                TraverseWith(PrimeDevelop);

                // Transfer the Development Grid to the Actual Grid
                TraverseWith((tile) => {tile.character = DevelopGrid[tile.y, tile.x];});
            }

            for (int i = 0; i < 3; i++)
            {
                // Perform the transformations
                TraverseWith(SecondaryDevelop);

                // Transfer the Development Grid to the Actual Grid
                TraverseWith((tile) => {tile.character = DevelopGrid[tile.y, tile.x];}); 
            }
        }

        // Develops a point via the cellular automata pattern
        public virtual void PrimeDevelop(Tile tile)
        {
            // Count the number of walls surrounding (and including) the point
            int closeCount = 0;
            int farCount = 0;

            // Count the number of walls in close range.
            TraverseNeighborsWith(tile, (neighbor) =>
            {
                closeCount += (Grid[neighbor.y, neighbor.x] == Wall || Grid[neighbor.y, neighbor.x] == null ? 1 : 0);
            });

            // Count the number of walls within 2 spaces.
            TraverseNeighborsWith(tile, (neighbor) =>
            {
                if (neighbor.y >= Grid.GetLength(0) || neighbor.x >= Grid.GetLength(1) || neighbor.x < 0 || neighbor.y < 0)
                    farCount++;
                else if ((neighbor.x == tile.x - 2 || neighbor.x == tile.x + 2) && (neighbor.y == tile.y - 2 || neighbor.y == tile.y + 2))
                {
                    // Do nothing
                }
                else
                    farCount += (Grid[neighbor.y, neighbor.x] == Wall || Grid[neighbor.y, neighbor.x] == null ? 1 : 0);
            }, 2);

            // If there are many walls or very few walls, become a wall.
            DevelopGrid[tile.y, tile.x] = closeCount > 4 || farCount < 3 ? Wall : Empty;
        }

        // Similar to primary develop, but only the initial algorithm
        public virtual void SecondaryDevelop(Tile tile)
        {
            // Count the number of walls surrounding (and including) the point
            int wallCount = 0;
            TraverseNeighborsWith(tile, (neighbor) =>
            {
                wallCount += (Grid[neighbor.y, neighbor.x] == Wall || Grid[neighbor.y, neighbor.x] == null ? 1 : 0);
            }, 1);

            // If there are many walls or very few walls, become a wall.
            DevelopGrid[tile.y, tile.x] = wallCount > 4 ? Wall : Empty;
        }
    }
}

