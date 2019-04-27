// Houston Tyler Webb
//
// This is an algorithm for producing a series of rooms connected by
// Long pipes across the map.
using System;
using System.Collections.Generic;

namespace ProcGenSharp
{
    public class BoxPipeMap : Map
    {
        List<Room> Rooms;

        // Parameterized Constructor
        public BoxPipeMap(int height, int width) : base(height, width)
        {
            Rooms = new List<Room>();
            FillGrid('.');
            TraverseWith( (tile) =>
            {
                // 1% chance to generate a room at a location.
                if (Rng.Next() % 100 == 0)
                {
                    // Try to create a room and throw it away if its out of bounds or intersects another room.
                    Room newRoom;

                    try
                    {
                        newRoom = new BoxRoom(this, Rng.Next(10) + 1, Rng.Next(10) + 1, tile);
                    }
                    catch(BoxRoom.FailedRoomInitException ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                        return;
                    }

                    foreach (Room room in Rooms)
                    {
                        if (newRoom.CollidesWith(room))
                            return;
                    }

                    Rooms.Add(newRoom);
                }
            });

            // Draw each room.
            foreach (Room room in Rooms)
            {
                room.DrawToParentGrid();
            }
        }
    }   
}

