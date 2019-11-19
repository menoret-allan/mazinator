using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator
{
    public class RandomMazeGenerator
    {
        private Random rand;

        public RandomMazeGenerator()
        {
            this.rand = new Random();
        }

        private void Test(Maze maze, List<(int x, int y)> possibilities)
        {
            while (possibilities.Count > 0)
            {
                var nextPath = this.rand.Next() % possibilities.Count;
                var (x, y) = possibilities[nextPath];

                maze.Board[y, x] = CaseType.Path;
                possibilities.Remove((x, y));
                var newPossibilities = GetPossibilities(x, y, maze).Where(pos => maze[pos.y, pos.x] == CaseType.Unknow).ToList();
                var toBeRemove = possibilities.Where(pos => newPossibilities.Contains(pos)).ToList();
                foreach (var remove in toBeRemove)
                {
                    possibilities.Remove(remove);
                    newPossibilities.Remove(remove);
                    maze.Board[remove.y, remove.x] = CaseType.Wall;
                }
                possibilities.AddRange(newPossibilities);
            }
        }

        private IEnumerable<(int x, int y)> GetPossibilities(int x, int y, Maze maze)
        {
            if (y != 0)
            {
                if (x > 1)
                {
                    yield return (x - 1, y);
                }

                if (x < maze.Dimension.X - 2)
                {
                    yield return (x + 1, y);
                }
            }

            if (x != 0)
            {
                if (y > 1)
                {
                    yield return (x, y - 1);
                }

                if (y < maze.Dimension.Y - 2)
                {
                    yield return (x, y + 1);
                }
            }
        }

        internal Maze Generate(Dimension dimension)
        {
            var maze = Maze.Build(dimension);
            maze.FillBoarderWith(CaseType.Wall);
            var entrance = (0, 1);
            this.OpenEntrance(maze, entrance);
            this.Test(maze, new List<(int x, int y)>() { entrance });
            var exit = this.FindExit(maze);
            this.OpenExit(maze, exit);

            return maze;
        }

        private void OpenEntrance(Maze maze, (int x, int y) entrance)
        {
            maze.Board[entrance.y, entrance.x] = CaseType.Path;
            maze.Entrance = entrance;
        }

        private void OpenExit(Maze maze, (int x, int y) exit)
        {
            maze.Board[exit.y, exit.x] = CaseType.Path;
            maze.Exit = exit;
        }

        private (int x, int y) FindExit(Maze maze)
        {
            List<(int x, int y)> possibilities = new List<(int x, int y)>();

            for (int rightSide = 1; rightSide < maze.Dimension.Y -1; rightSide++)
            {
                if (maze.Board[rightSide, maze.Dimension.X - 2] == CaseType.Path)
                {
                    possibilities.Add((maze.Dimension.X - 1, rightSide));
                }
            }
            for (int downSide = 1; downSide < maze.Dimension.X - 1; downSide++)
            {
                if (maze.Board[maze.Dimension.Y - 2, downSide] == CaseType.Path)
                {
                    possibilities.Add((downSide, maze.Dimension.Y - 1));
                }
            }
            
            return possibilities[this.rand.Next() % possibilities.Count];
        }
    }
}
