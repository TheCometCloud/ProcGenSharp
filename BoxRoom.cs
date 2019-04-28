// Houston Tyler Webb
// 
// Child class of Room for rectangular rooms.
using System;
using System.Collections.Generic;

namespace ProcGenSharp
{
    public class BoxRoom : Room
    {
        // Room Data Fields
        public int Height {get; private set;}
        public int Width {get; private set;}
        public Tile TopLeft {get; private set;}

        // Room failed to be created due to the SourceException.
        public class FailedRoomInitException : Exception 
        {
            public Exception SourceException {get;}

            public FailedRoomInitException(Exception ex) : base()
            {
                SourceException = ex;
            }
        }

        // Parameterized Constructor
        public BoxRoom (Map parentMap, int height, int width, Tile topLeft)
        {
            this.Points = new List<Tile>();
            this.ParentMap = parentMap;
            this.Height = height;
            this.Width = width;
            this.TopLeft = topLeft;

            // Add each Tile in the box to the Points list
            for (int i = topLeft.y; i < topLeft.y + Height; i++)
            {
                for (int j = topLeft.x; j < topLeft.x + Width; j++)
                {
                    try
                    {
                        Points.Add(new Tile(j, i, ParentMap));
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        throw new FailedRoomInitException(ex);
                    }
                }
            }
        }

        // Returns a list of points that border the room. 
        public override List<Tile> GetPerimeter()
        {
            List<Tile> perimeterPoints = new List<Tile>();
        
            // Add tile to the perimeter if one of its dimensions are part of the edge.-
            TraverseWith( (tile) =>
            {
                if (tile.x == TopLeft.x || tile.y == TopLeft.y|| tile.x == TopLeft.x + Width - 1 || tile.y == TopLeft.y + Height - 1)
                {
                    perimeterPoints.Add(tile);
                }
            });
        
            return perimeterPoints;
        }
    }
}
