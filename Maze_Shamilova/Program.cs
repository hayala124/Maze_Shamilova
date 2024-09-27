using System;

namespace maze_Shamilova
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char[,] main_maze;
            var maze = new Maze();

            do
            {
                main_maze = maze.GenerateMaze();
            }
            while (maze.IsSolvable(main_maze));

            maze.PlayGame(main_maze);
        }
    }
}