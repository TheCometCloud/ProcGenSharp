using System;
using ProcGenSharp;

class Demo
{
    static void Main(string[] args)
    {   
        GuidedCaveMap cave = new GuidedCaveMap(25, 70);
        Console.WriteLine("A Guided Cave Map:");
        Console.WriteLine(cave);
        var rooms = cave.GetFloodRooms();
        Console.WriteLine($"There are {rooms.Count} rooms.");
    }
}