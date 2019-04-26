// Child class of Room for rectangular rooms.
using System;
using System.Collections.Generic;

namespace ProcGenSharp
{
    public class BoxRoom : Room
    {
        public bool isDirty;

        public int Height {get; private set;}
        public int Width {get; private set;}
        public Tile TopLeft {get; private set;}

        // 
        public class FailedRoomInitException : Exception {};

        // Parameterized Constructor
        public BoxRoom (Map parentMap, int height, int width, Tile topLeft)
        {
            this.Points = new List<Tile>();
            this.ParentMap = parentMap;
            this.Height = height;
            this.Width = width;
            this.TopLeft = topLeft;

            for (int i = topLeft.y; i < topLeft.y + Height; i++)
            {
                for (int j = topLeft.x; j < topLeft.x + Width; j++)
                {
                    try
                    {
                        Points.Add(new Tile(j, i, ParentMap.Grid[i, j]));
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        throw new FailedRoomInitException();
                    }
                }
            }
        }

        // Returns a list of points that border the room. 
        public override List<Tile> GetPerimeter()
        {
            List<Tile> perimeterPoints = new List<Tile>();
        
            TraverseWith( (x, y) =>
            {
                if (x == TopLeft.x || y == TopLeft.y|| x == TopLeft.x + Width - 1 || y == TopLeft.y + Height - 1)
                {
                    perimeterPoints.Add(new Tile(x, y, ParentMap.Grid[y, x]));
                }
            });
        
            return perimeterPoints;
        }
    }
}
