// Houston Tyler Webb
//
// This is a base class meant to be inherited from for procedural generation. It features
// a grid and a variety of ways to traverse it.
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcGenSharp
{
    public class Map
    {
        // Declare needed fields
        protected Random Rng;
        public char?[,] Grid{get; set;}

        // Flood fields
        protected List<Tile> FloodVerifier;

        // Default representers
        public char Unknown = '?';
        public char Empty = ' ';
        public char Wall = '#';

        // Dimension getters
        public int Height { get { return Grid.GetLength(0); } }
        public int Width { get { return Grid.GetLength(1); } }

        // Parameterized Constructor
        public Map(int height, int width)
        {
            // Initialize the grid with breathing room
            Grid = new char?[height, width];
            Rng = new Random();
            FloodVerifier = new List<Tile>();

            // Fill the grid with unknowns
            FillGrid();
        }

        // Fills the grid with a character, defaults to unknowns
        public void FillGrid(char c = '?')
        {
            TraverseWith((tile) => {tile.character = c;});
        }

        // Fills the borders of the map with walls
        public void WallGrid()
        {
            TraverseWith( (tile) =>
            {
                if (tile.x == 0 || tile.y == 0 || tile.x == Width - 1 || tile.y == Height - 1)
                    tile.character = Wall;
            });
        }

        // Loops through the grid, calls innerAction on each point, and outerAction at the end of each row. 
        public void TraverseWith(Action<Tile> innerAction, Action<int> outerAction = null)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                    innerAction(new Tile(j, i, this));

                if (outerAction != null)
                    outerAction(i);
            }
        }

        // Traverses a smaller region of the gird, calls innerAction on each point, and outerAction at the end of each row.
        // Throws an IndexOutOfRangeException.
        public void SubTraverseWith(int x1, int x2, int y1, int y2, Action<Tile> innerAction, Action<int> outerAction = null)
        {
            try 
            {
                for (int i = y1; i <= y2; i++)
                {
                    for (int j = x1; j <= x2; j++)
                    {
                        innerAction(new Tile(j, i, this));
                    }

                    outerAction(i);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw ex;
            }
        }

        // Loop through all neighbors of a Tile
        public void TraverseNeighborsWith(Tile target, Action<Tile> action, int range = 1)
        {
            for (int i = target.y - range; i <= target.y + range; i++)
            {
                for (int j = target.x - range; j <= target.x + range; j++)
                {
                    action(new Tile(j, i, this));
                }
            }
        }

        // Tile based subtraversal.
        public void SubTraverseWith(Tile topLeft, Tile bottomRight, Action<Tile> innerAction, Action<int> outerAction = null)
        {
            SubTraverseWith(topLeft.x, bottomRight.x, topLeft.y, bottomRight.y, innerAction, outerAction);
        }

        // Returns a list of isolated rooms in the Map
        public List<Room> GetFloodRooms()
        {
            List<Room> Rooms = new List<Room>();

            // Flood unflooded and empty tiles
            TraverseWith( (target) => 
            {
                if (target.character == Empty && !FloodVerifier.Contains(target))
                {
                    FloodVerifier.Add(target);
                    Rooms.Add(new Room(Flood(target), this));
                }
            });
            
            FloodVerifier = new List<Tile>();
            return Rooms;
        }

        // Recursive function to return all empty tiles connected to the input tile.
        private List<Tile> Flood(Tile tile)
        {
            List<Tile> EmptyNeighbors = new List<Tile>();

            // Find unchecked and empty neighboring tiles, excluding diagonals
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    if (x == 0 ^ y == 0)
                    {
                        Tile test = new Tile(tile.x + x, tile.y + y, this);

                        if (!test.IsOutOfBounds() && test.character == Empty && !FloodVerifier.Contains(test))
                            EmptyNeighbors.Add(test);
                    }
                }
            }

            // If no eligible neighbors are found, return null 
            if (EmptyNeighbors.Count == 0)
                return null;

            // Recurse for each neighbor
            List<Tile> output = new List<Tile>();
            foreach(Tile neighbor in EmptyNeighbors)
            {
                FloodVerifier.Add(neighbor);
                List<Tile> descendents = Flood(neighbor);
                if (descendents != null)
                    output = output.Union(descendents).ToList();
            }

            return output;
        }

        // String represenation of the class.
        public override string ToString()
        {
            string output = "";

            // Loops through the array and appends the character of each to output and newlines at the end of each row.
            TraverseWith((tile) => {output += tile.character + " ";}, (x) => {output += '\n';});

            return output;
        }
    }
}
