using System;
using ProcGenSharp;

class Demo
{
    static void Main(string[] args)
    {   
        CaveMap cave = new CaveMap(25, 70);
        Console.WriteLine("A Cave Map:");
        Console.WriteLine(cave);
        var rooms = cave.GetFloodRooms();
        Console.WriteLine($"There are {rooms.Count} rooms.");
    }
}