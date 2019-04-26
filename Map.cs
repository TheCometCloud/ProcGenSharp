// Houston Tyler Webb
//
// This is a base class meant to be inherited from for procedural generation. It features
// a grid and a variety of ways to traverse it.
using System;

namespace ProcGenSharp
{
    public class Map
    {
        // Declare needed fields
        protected Random Rng;
        public char?[,] Grid{get; set;}

        // Default representers
        public char Unknown = '?';
        public char Empty = '.';
        public char Wall = '#';

        // Dimension getters
        public int Height { get { return Grid.GetLength(0); } }
        public int Width { get { return Grid.GetLength(1); } }

        // Parameterized Constructor
        public Map(int height, int width)
        {
            // Initialize the grid with breathing room
            Grid = new char?[height + 2, width + 2];
            Rng = new Random();

            // Fill the grid with unknowns
            FillGrid();
        }

        // Fills the grid with a character, defaults to unknowns
        public void FillGrid(char c = '?')
        {
            TraverseWith((x, y) => {Grid[x, y] = c;});
        }

        // Loops through the grid, calls innerAction on each point, and outerAction at the end of each row. 
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

        // Traverses a smaller region of the gird, calls innerAction on each point, and outerAction at the end of each row.
        // Throws an IndexOutOfRangeException.
        public void SubTraverseWith(int x1, int x2, int y1, int y2, Action<int, int> innerAction, Action<int> outerAction = null)
        {
            try 
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
            catch (IndexOutOfRangeException ex)
            {
                throw ex;
            }
        }

        // Tile based subtraversal.
        public void SubTraverseWith(Tile topLeft, Tile bottomRight, Action<int, int> innerAction, Action<int> outerAction = null)
        {
            SubTraverseWith(topLeft.x, bottomRight.x, topLeft.y, bottomRight.y, innerAction, outerAction);
        }

        // String represenation of the class.
        public override string ToString()
        {
            string output = "";

            // Loops through the array and appends the character of each to output and newlines at the end of each row.
            TraverseWith((x, y) => {output += Grid[x, y] + " ";}, (x) => {output += '\n';});

            return output;
        }
    }
}
