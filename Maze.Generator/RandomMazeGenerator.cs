using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator
{
    public class RandomMazeGenerator
    {
        private IRand rand;

        public RandomMazeGenerator(IRand rand)
        {
            this.rand = rand;
        }

        private void BuildInsideMaze(Maze maze, HashSet<(ushort x, ushort y)> possibilities)
        {
            while (possibilities.Count > 0)
            {
                var nextPath = this.rand.Next(0, possibilities.Count - 1);
                var (x, y) = possibilities.ElementAt(nextPath);

                maze.Board[y, x] = CaseType.Path;
                possibilities.Remove((x, y));
                foreach (var neighbour in GetPossibilities(x, y, maze))
                {
                    if (possibilities.Contains(neighbour))
                    {
                        possibilities.Remove(neighbour);
                        maze.Board[neighbour.y, neighbour.x] = CaseType.Wall;
                    }
                    else
                    {
                        possibilities.Add(neighbour);
                    }
                }
            }
        }

        private IEnumerable<(ushort x, ushort y)> GetPossibilities(ushort x, ushort y, Maze maze)
        {
            if (y != 0)
            {
                if (x > 1)
                {
                    if (maze[y, x - 1] == CaseType.Unknow)
                    {
                        yield return ((ushort)(x - 1), y);
                    }
                }

                if (x < maze.Dimension.X - 2)
                {
                    if (maze[y, x + 1] == CaseType.Unknow)
                    {
                        yield return ((ushort)(x + 1), y);
                    }
                }
            }

            if (x != 0)
            {
                if (y > 1)
                {
                    if (maze[y - 1, x] == CaseType.Unknow)
                    {
                        yield return (x, (ushort)(y - 1));
                    }
                }

                if (y < maze.Dimension.Y - 2)
                {
                    if (maze[y + 1, x] == CaseType.Unknow)
                    {
                        yield return (x, (ushort)(y + 1));
                    }
                }
            }
        }

        internal Maze Generate(Dimension dimension)
        {
            var maze = Maze.Build(dimension);
            maze.FillBoarderWith(CaseType.Wall);
            var entrance = ((ushort)0, (ushort)1);
            this.OpenEntrance(maze, entrance);
            this.BuildInsideMaze(maze, new HashSet<(ushort x, ushort y)> { entrance });
            for (int x = 1; x < maze.Dimension.X-1; x++)
            {
                for (int y = 1; y < maze.Dimension.Y-1; y++)
                {
                    if (maze[y, x] == CaseType.Unknow) maze.Board[y, x] = CaseType.Wall;
                }
            }


            var exit = this.FindExit(maze);
            this.OpenExit(maze, exit);

            return maze;
        }

        private void OpenEntrance(Maze maze, (ushort x, ushort y) entrance)
        {
            maze.Board[entrance.y, entrance.x] = CaseType.Path;
            maze.Entrance = entrance;
        }

        private void OpenExit(Maze maze, (ushort x, ushort y) exit)
        {
            maze.Board[exit.y, exit.x] = CaseType.Path;
            maze.Exit = exit;
        }

        private (ushort x, ushort y) FindExit(Maze maze)
        {
            List<(ushort x, ushort y)> possibilities = new List<(ushort x, ushort y)>();

            for (ushort rightSide = 1; rightSide < maze.Dimension.Y - 1; rightSide++)
            {
                if (maze.Board[rightSide, maze.Dimension.X - 2] == CaseType.Path)
                {
                    possibilities.Add(((ushort)(maze.Dimension.X - 1), rightSide));
                }
            }
            for (ushort downSide = 1; downSide < maze.Dimension.X - 1; downSide++)
            {
                if (maze.Board[maze.Dimension.Y - 2, downSide] == CaseType.Path)
                {
                    possibilities.Add((downSide, (ushort)(maze.Dimension.Y - 1)));
                }
            }

            return possibilities[this.rand.Next() % possibilities.Count];
        }
    }
}
