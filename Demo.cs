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

        GuidedCaveMap guidedCaveMap = new GuidedCaveMap(25, 70);
        Console.WriteLine("A Guided Cave Map:");
        Console.WriteLine(guidedCaveMap);

        var guidedRooms = guidedCaveMap.GetFloodRooms();
        Console.WriteLine($"There are {guidedRooms.Count} rooms.");

        BoxPipeMap boxPipe = new BoxPipeMap(25, 70, 5, 10, -100, 0);
        Console.WriteLine("A Box-Pipe Map:");
        Console.WriteLine(boxPipe);

        var pipeRooms = boxPipe.GetFloodRooms();
        Console.WriteLine($"There are {pipeRooms.Count} rooms.");

        MazeMap mazeMap = new MazeMap(25, 70);
        Console.WriteLine("A Maze Map:");
        Console.WriteLine(mazeMap);
    }
}