using System;

namespace MazeGenerator
{
    public class SplitMazeGenerator
    {
        public SplitMazeGenerator()
        {
        }

        public void Generate(Maze maze, Area area)
        {
            if (area.Width <= 1 || area.Height <= 1 || (area.Width <= 2 && area.Height <= 2))
            { return; }

            var rand = new Random();

            // i'm building the line on the wall that I just destroy...
            if (area.Width > area.Height)
            {
                var xBreak = GetMiddleLinePosition(area.Width, rand);
                var yHole = GetHoleInTheLine(area.Height, rand) + area.Y;

                for (int y = area.Y; y < area.Y + area.Height; y++)
                {
                    if (y != yHole)
                    {
                        maze.Board[y, area.X + xBreak] = CaseType.Wall;
                    }
                }

                var areaRight = new Area(area.X, area.Y, xBreak - 1, area.Height);
                var areaLeft = new Area(area.X + xBreak + 1, area.Y, area.Width - xBreak - 1, area.Height);
                Generate(maze, areaRight);
                Generate(maze, areaLeft);
            }
            else
            {
                var yBreak = GetMiddleLinePosition(area.Height, rand);
                var XHole = GetHoleInTheLine(area.Width, rand)+ area.X;

                for (int x = area.X; x < area.X + area.Width; x++)
                {
                    if (x != XHole)
                    {
                        maze.Board[area.Y + yBreak, x] = CaseType.Wall;
                    }
                }

                var areaUp = new Area(area.X, area.Y, area.Width, yBreak);
                var areaDown = new Area(area.X, area.Y + yBreak + 1, area.Width, area.Height - yBreak-2);
                Generate(maze, areaUp);
                Generate(maze, areaDown);
            }
        }

        internal Maze Generate(Dimension dimension)
        {
            var maze = Maze.Build(dimension);
            maze.FillWith(CaseType.Path);
            Generate(maze, new Area(0, 0, maze.Dimension.X, maze.Dimension.Y));
            return maze;
        }

        private int GetHoleInTheLine(int size, Random rand)
        {
            return rand.Next() % size;
        }

        private static int GetMiddleLinePosition(int distance, Random rand)
        {
            return rand.Next() % (distance - 2) + 1;
        }
    }
}
