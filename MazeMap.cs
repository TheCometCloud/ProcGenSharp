// Houston Tyler Webb
// 
// This is a child class of Map that generates a perfect Maze.
using System;
using System.Collections.Generic;

namespace ProcGenSharp
{
    public class MazeMap : Map
    {
        // Development Fields
        char Processing = 'P';
        List<Tile> Frontier = new List<Tile>();
        float BranchRate = 0;

        // Markers for which edge a tile is on respective to another tile
        [Flags]
        enum Edge
        {
            None = 0x0,
            Right = 0x1,
            Top = 0x2,
            Left = 0x4,
            Bottom = 0x8,
        }

        // Parameterized Constructor
        public MazeMap(int height, int width, float BranchRate = 0) : base(height, width)
        {
            // Initialize our Branch Rate
            this.BranchRate = BranchRate;

            // Generate the Maze
            FillGrid(Unknown);
            WallGrid();
            InitializeMaze();
        }

        // Create the Maze
        public void InitializeMaze()
        {
            Tile start;

            // Start at a random, unknown tile
            do
                start = new Tile(Rng.Next(Width), Rng.Next(Height), this);
            while (start.character != Unknown);

            Carve(start);

            // Chug through the Map
            while(Frontier.Count > 0)
            {
                double position = Rng.NextDouble();
                position = MathF.Pow((float) position, (MathF.Pow((float) Math.E, -1 * BranchRate)));

                Tile choice = Frontier[(int) (position * Frontier.Count)];

                if (ShouldBeEmpty(choice))
                    Carve(choice);
                else
                    choice.character = Wall;

                Frontier.Remove(choice);
            }
            
            // Set the remaining, unexposed cells to walls
            TraverseWith( (tile) =>
            {
                if (tile.character == Unknown)
                    tile.character = Wall;
            });
        }

        // Empties a tile and adds its neighbors to the Frontier
        public void Carve(Tile tile)
        {
            var extra = new List<Tile>();
            tile.character = Empty;

            // Process any unknown neighbors
            foreach (Tile neighbor in tile.GetNeighbors(false))
            {
                if (neighbor.character == Unknown)
                {
                    neighbor.character = Processing;
                    extra.Add(neighbor);
                }
            }

            // Randomize the extras
            for (int i = 0; i < extra.Count; i++)
            {
                Tile temp = extra[i];
                int randomIndex = Rng.Next(i, extra.Count);
                extra[i] = extra[randomIndex];
                extra[randomIndex] = temp;
            }

            // Add the rest of them
            foreach(Tile ex in extra)
                Frontier.Add(ex);
        }

        // Determines if a Tile should be traversed into
        public bool ShouldBeEmpty(Tile tile)
        {
            var edges = Edge.None;

            var neighbors = tile.GetNeighbors(true);
            if (!neighbors[1].IsOutOfBounds() && neighbors[1].character == Empty)
                edges |= Edge.Top; 

            if (!neighbors[3].IsOutOfBounds() && neighbors[3].character == Empty)
                edges |= Edge.Left;

            if (!neighbors[4].IsOutOfBounds() && neighbors[4].character == Empty)
                edges |= Edge.Right;

            if (!neighbors[6].IsOutOfBounds() && neighbors[6].character == Empty)
                edges |= Edge.Bottom;

            switch(edges)
            {
                case Edge.Right:
                    return 
                    !(
                        (neighbors[0].character == Empty && !neighbors[0].IsOutOfBounds()) || 
                        (neighbors[5].character == Empty && neighbors[5].IsOutOfBounds())
                    );

                case Edge.Bottom:
                    return 
                    !(
                        (neighbors[0].character == Empty && !neighbors[0].IsOutOfBounds()) || 
                        (neighbors[2].character == Empty && !neighbors[2].IsOutOfBounds())
                    );
                
                case Edge.Left:
                    return 
                    !(
                        (neighbors[2].character == Empty && !neighbors[2].IsOutOfBounds()) || 
                        (neighbors[7].character == Empty && !neighbors[7].IsOutOfBounds())
                    );

                case Edge.Top:
                    return 
                    !(
                        (neighbors[5].character == Empty && !neighbors[5].IsOutOfBounds()) || 
                        (neighbors[7].character == Empty && !neighbors[7].IsOutOfBounds())
                    );
                
                default:
                    return false;
            }
        }
    }
}
