using System;

namespace Maze_Shamilova
{
    internal class Maze
    {
        const int Width = 50;
        const int Height = 20;

        /// <summary>
        /// Генерирует лабиринт, устанавливает выход, создает проходы к нему.
        /// </summary>
        public char[,] GenerateMaze()
        {
            var maze = new char[Height, Width];

            Random random = new Random();

            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    maze[i, j] = '█';
                }
            }

            GenerateMazeRecursive(maze, 1, 1, random);

            maze[Height - 2, Width - 2] = 'E';

            for (var i = 1; i < Height - 2; i++)
            {
                maze[i, 1] = ' ';
            }

            for (var j = 1; j < Width - 2; j++)
            {
                if (random.NextDouble() > 0.7)
                {
                    maze[Height - 2, j] = '█';
                }
                else
                {
                    maze[Height - 2, j] = ' ';
                }
            }

            if (Width > 2 && maze[Height - 2, Width - 3] == '█')
                maze[Height - 2, Width - 3] = ' ';


            return maze;
        }

        /// <summary>
        /// Алгоритм для генерации лабиринта, создания поворотов и тупиков.
        /// </summary>
        private void GenerateMazeRecursive(char[,] maze, int x, int y, Random random)
        {
            maze[y, x] = ' ';

            var directions = new[]
            {
                new { Dx = 0, Dy = -2 },
                new { Dx = 0, Dy = 2 },
                new { Dx = -2, Dy = 0 },
                new { Dx = 2, Dy = 0 }
            };

            Shuffle(directions, random);

            foreach (var direction in directions)
            {
                var newX = x + direction.Dx;
                var newY = y + direction.Dy;

                if (newX > 0 && newX < maze.GetLength(1) - 1 && newY > 0 && newY < maze.GetLength(0) - 1 && maze[newY, newX] == '█')
                {
                    maze[y + direction.Dy / 2, x + direction.Dx / 2] = ' ';

                    GenerateMazeRecursive(maze, newX, newY, random);
                }
            }
        }

        /// <summary>
        /// Используется для перемешивания направлений движения при генерации лабиринта.
        /// </summary>
        private void Shuffle<T>(T[] array, Random rng)
        {
            var n = array.Length;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }

        /// <summary>
        /// Проверяет, существует ли решение пути из точки S к точке E в лабиринте.
        /// </summary>
        public bool IsSolvable(char[,] maze)
        {
            var height = maze.GetLength(0);
            var width = maze.GetLength(1);

            Queue<(int x, int y)> queue = new Queue<(int x, int y)>();
            var visited = new bool[height, width];

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    if (maze[i, j] == 'S')
                    {
                        queue.Enqueue((j, i));
                        visited[i, j] = true;
                        break;
                    }
                }
                if (queue.Count > 0) break;
            }

            int[][] directions = { [0, 1], [0, -1], [1, 0], [-1, 0] };

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                if (maze[y, x] == 'E') return true;

                foreach (var dir in directions)
                {
                    var newX = x + dir[0];
                    var newY = y + dir[1];

                    if (newX >= 0 && newX < width && newY >= 0 && newY < height &&
                        maze[newY, newX] != '█' && !visited[newY, newX])
                    {
                        queue.Enqueue((newX, newY));
                        visited[newY, newX] = true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Запускает игровой процесс в лабиринте, управляет движением игрока, завершает игру.
        /// </summary>
        public void PlayGame(char[,] maze)
        {
            var playerX = 1;
            var playerY = 1;

            while (true)
            {
                DrawMaze(maze, playerX, playerY);

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (playerY > 1 && maze[playerY - 1, playerX] != '█')
                            playerY--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (playerY < maze.GetLength(0) - 2 && maze[playerY + 1, playerX] != '█')
                            playerY++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (playerX > 1 && maze[playerY, playerX - 1] != '█')
                            playerX--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (playerX < maze.GetLength(1) - 2 && maze[playerY, playerX + 1] != '█')
                            playerX++;
                        break;
                }

                if (maze[playerY, playerX] == 'E')
                {
                    Console.WriteLine("Поздравляем! Вы прошли лабиринт!");
                    break;
                }
            }
        }

        /// <summary>
        /// Отображает лабиринт в консоли.
        /// </summary>
        private void DrawMaze(char[,] maze, int playerX, int playerY)
        {
            Console.SetCursorPosition(0, 0);

            maze[1, 1] = 'S';

            for (var y = 0; y < maze.GetLength(0); y++)
            {
                for (var x = 0; x < maze.GetLength(1); x++)
                {
                    if (x == playerX && y == playerY)
                        Console.Write('P');
                    else
                        Console.Write(maze[y, x]);
                }
                Console.WriteLine();
            }
        }

    }
}
