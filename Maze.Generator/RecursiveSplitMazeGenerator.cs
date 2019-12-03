using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGenerator
{
    public class RecursiveSplitMazeGenerator
    {
        private IRand rand;

        public RecursiveSplitMazeGenerator(IRand rand)
        {
            this.rand = rand;
        }

        delegate void Gen(Maze maze, Area area);

        public void Generate(Maze maze, Area area)
        {
            Gen  gen = Generate;
            List<Area> todo = new List<Area>();
            var walls = new List<(int x, int y)>();

            if (area.Width == 2 && area.Height == 2)
            {
                DealWithSquare2(maze, area);
                return;
            }

            if (area.Width > area.Height)
            {
                List<int> possibilities = GetPossibilitiesForWidth(maze, area);
                var raw = possibilities[rand.Next() % possibilities.Count()];
                Area areaLeft = new Area(area.X, area.Y, raw - area.X, area.Height);
                var isAreaLeftDone = false;
                Area areaRight = new Area(raw + 1, area.Y, area.Width - (raw - area.X) - 1, area.Height);
                var isAreaRightDone = false;

                if (areaLeft.Width == 2 && areaLeft.Height == 2)
                {
                    var squarePossibilities = GetSquare2PossibilitiesX(maze, areaLeft, raw);
                    if (squarePossibilities.Count == 1)
                    {
                        var wall = squarePossibilities.First();
                        maze.Board[wall.y, wall.x] = CaseType.Wall;
                        isAreaLeftDone = true;
                    }
                }
                if (areaRight.Width == 2 && areaRight.Height == 2)
                {
                    var squarePossibilities = GetSquare2PossibilitiesX(maze, areaRight, raw);
                    if (squarePossibilities.Count == 1)
                    {
                        var wall = squarePossibilities.First();
                        isAreaRightDone = true;
                        maze.Board[wall.y, wall.x] = CaseType.Wall;
                    }
                }

                if (isAreaLeftDone && isAreaRightDone) { }
                else
                {
                    walls = GetHeightWalls(maze, area, raw);
                }

                if (!isAreaLeftDone && areaLeft.Width >= 2)
                {
                    todo.Add(areaLeft);
                }
                if (!isAreaRightDone && areaRight.Width >= 2)
                {
                    todo.Add(areaRight);
                }
            }
            else
            {
                List<int> possibilities = GetPossibilitiesForHeight(maze, area);
                var raw = possibilities[rand.Next(possibilities.Count() -1)];
                Area areaUp = new Area(area.X, area.Y, area.Width, raw - area.Y);
                var isAreaUpDone = false;
                Area areaDown = new Area(area.X, raw + 1, area.Width, area.Height - (raw - area.Y) - 1);
                var isAreaDownDone = false;

                if (areaUp.Width == 2 && areaUp.Height == 2)
                {
                    var squarePossibilities = GetSquare2PossibilitiesY(maze, areaUp, raw);
                    if (squarePossibilities.Count == 1)
                    {
                        var wall = squarePossibilities.First();
                        maze.Board[wall.y, wall.x] = CaseType.Wall;
                        isAreaUpDone = true;
                    }
                }
                if (areaDown.Width == 2 && areaDown.Height == 2)
                {
                    var squarePossibilities = GetSquare2PossibilitiesY(maze, areaDown, raw);
                    if (squarePossibilities.Count == 1)
                    {
                        var wall = squarePossibilities.First();
                        isAreaDownDone = true;
                        maze.Board[wall.y, wall.x] = CaseType.Wall;
                    }
                }

                if (isAreaDownDone && isAreaUpDone) { }
                else
                {
                    walls = GetWidthWalls(maze, area, raw);
                }

                if (!isAreaUpDone && areaUp.Height >= 2)
                {
                    todo.Add(areaUp);
                }
                if (!isAreaDownDone && areaDown.Height >= 2)
                {
                    todo.Add(areaDown);
                }
            }

            foreach ((int x, int y) in walls)
            {
                maze.Board[y, x] = CaseType.Wall;
            }

            if (todo.Count == 2)
            {
                Parallel.Invoke(() => Generate(maze, todo.First()), () => Generate(maze, todo.Last()));
            }
            else
            {
                foreach (var areaToProcess in todo)
                {
                    Generate(maze, areaToProcess);
                }
            }
        }

        private void DealWithSquare2(Maze maze, Area area)
        {
            List<(int x, int y)> possibilities = GetSquare2Possibilities(maze, area);

            if (possibilities.Any())
            {
                (int x , int y) = possibilities[rand.Next(possibilities.Count() - 1)];
                maze.Board[y, x] = CaseType.Wall;
            }
        }

        public static List<(int x, int y)> GetSquare2Possibilities(Maze maze, Area area)
        {
            var possibilities = new List<(int x, int y)>();
            if (maze[area.Y - 1, area.X] == CaseType.Wall && maze[area.Y, area.X - 1] == CaseType.Wall)
            {
                possibilities.Add((area.X, area.Y));
            }
            if (maze[area.Y - 1, area.X + 1] == CaseType.Wall && maze[area.Y, area.X + 2] == CaseType.Wall)
            {
                possibilities.Add((area.X + 1, area.Y));
            }
            if (maze[area.Y + 1, area.X - 1] == CaseType.Wall && maze[area.Y + 2, area.X] == CaseType.Wall)
            {
                possibilities.Add((area.X, area.Y + 1));
            }
            if (maze[area.Y + 1, area.X + 2] == CaseType.Wall && maze[area.Y + 2, area.X + 1] == CaseType.Wall)
            {
                possibilities.Add((area.X + 1, area.Y + 1));
            }

            return possibilities;
        }

        public static List<(int x, int y)> GetSquare2PossibilitiesY(Maze maze, Area area, int forbiddenY)
        {
            var possibilities = new List<(int x, int y)>();
            if ((forbiddenY == area.Y - 1 || maze[area.Y - 1, area.X] == CaseType.Wall) && maze[area.Y, area.X - 1] == CaseType.Wall)
            {
                possibilities.Add((area.X, area.Y));
            }
            if ((forbiddenY == area.Y - 1 || maze[area.Y - 1, area.X + 1] == CaseType.Wall) && maze[area.Y, area.X + 2] == CaseType.Wall)
            {
                possibilities.Add((area.X + 1, area.Y));
            }
            if (maze[area.Y + 1, area.X - 1] == CaseType.Wall && (forbiddenY == area.Y + 2 || maze[area.Y + 2, area.X] == CaseType.Wall))
            {
                possibilities.Add((area.X, area.Y + 1));
            }
            if (maze[area.Y + 1, area.X + 2] == CaseType.Wall && (forbiddenY == area.Y + 2 || maze[area.Y + 2, area.X + 1] == CaseType.Wall))
            {
                possibilities.Add((area.X + 1, area.Y + 1));
            }

            return possibilities;
        }

        public static List<(int x, int y)> GetSquare2PossibilitiesX(Maze maze, Area area, int forbiddenX)
        {
            var possibilities = new List<(int x, int y)>();
            if (maze[area.Y - 1, area.X] == CaseType.Wall && (forbiddenX == area.X - 1 || maze[area.Y, area.X - 1] == CaseType.Wall))
            {
                possibilities.Add((area.X, area.Y));
            }
            if (maze[area.Y - 1, area.X + 1] == CaseType.Wall && (forbiddenX == area.X + 2 || maze[area.Y, area.X + 2] == CaseType.Wall))
            {
                possibilities.Add((area.X + 1, area.Y));
            }
            if ((forbiddenX == area.X - 1 || maze[area.Y + 1, area.X - 1] == CaseType.Wall) && maze[area.Y + 2, area.X] == CaseType.Wall)
            {
                possibilities.Add((area.X, area.Y + 1));
            }
            if ((forbiddenX == area.X + 2 || maze[area.Y + 1, area.X + 2] == CaseType.Wall) && maze[area.Y + 2, area.X + 1] == CaseType.Wall)
            {
                possibilities.Add((area.X + 1, area.Y + 1));
            }

            return possibilities;
        }

        public static List<int> GetPossibilitiesForWidth(Maze maze, Area area)
        {
            var possibilities = new List<int>();
            for (int x = area.X + 1; x < area.X + area.Width - 1; x++)
            {
                if (maze[area.Y - 1, x] == CaseType.Wall || maze[area.Y + area.Height, x] == CaseType.Wall)
                {
                    possibilities.Add(x);
                }
            }
            if (!possibilities.Any())
            {
                possibilities.Add(area.X);
                possibilities.Add(area.X + area.Width - 1);
            }

            return possibilities;
        }

        public static List<int> GetPossibilitiesForHeight(Maze maze, Area area)
        {
            var possibilities = new List<int>();
            for (int y = area.Y + 1; y < area.Y + area.Height - 1; y++)
            {
                if (maze[y, area.X - 1] == CaseType.Wall || maze[y, area.X + area.Width] == CaseType.Wall)
                {
                    possibilities.Add(y);
                }
            }
            if (!possibilities.Any())
            {
                possibilities.Add(area.Y);
                possibilities.Add(area.Y + area.Height - 1);
            }

            return possibilities;
        }

        public List<(int x, int y)> GetHeightWalls(Maze maze, Area area, int x)
        {
            List<(int x, int y)> walls = new List<(int x, int y)>();

            if (maze[area.Y - 1, x] == CaseType.Wall && !IsCaseAgainWallWithHole(maze, area, x, area.Y))
            {
                walls.Add((x, area.Y));
            }

            if (maze[area.Y + area.Height, x] == CaseType.Wall && !IsCaseAgainWallWithHole(maze, area, x, area.Y + area.Height - 1))
            {
                walls.Add((x, area.Y + area.Height - 1));
            }

            for (int y = area.Y + 1; y < area.Y + area.Height - 1; y++)
            {
                if (!IsCaseAgainWallWithHole(maze, area, x, y))
                {
                    walls.Add((x, y));
                }
            }

            if (walls.Count() == area.Height)
            {
                walls.Remove(walls[rand.Next(walls.Count() - 1)]);
            }

            return walls;
        }

        private static bool IsCaseAgainWallWithHole(Maze maze, Area area, int x, int y)
        {
            return x == area.X && maze[y, x - 1] != CaseType.Wall ||
                  (x == area.X + area.Width - 1 && maze[y, x + 1] != CaseType.Wall);
        }

        public List<(int x, int y)> GetWidthWalls(Maze maze, Area area, int y)
        {
            List<(int x, int y)> walls = new List<(int x, int y)>();
            var holeNeeded = true;

            for (int x = area.X; x < area.X + area.Width; x++)
            {
                if (y == area.Y && maze[y - 1, x] != CaseType.Wall)
                {
                    holeNeeded = false;
                }
                else if ((y == area.Y + area.Height - 1 && maze[y + 1, x] != CaseType.Wall))
                {
                    holeNeeded = false;
                }
                else
                {
                    walls.Add((x, y));
                }
            }

            if (walls.Any())
            {

                if (maze[y, area.X - 1] == CaseType.Unknow && walls.First() == (area.X, y))
                {
                    walls.Remove(walls.First());
                }
                else if (maze[y, area.X + area.Width] == CaseType.Unknow && walls.Last() == (area.X + area.Width - 1, y))
                {
                    walls.Remove(walls.Last());
                }
                else if (holeNeeded)
                {
                    walls.Remove(walls[rand.Next() % walls.Count()]);
                }
            }

            return walls;
        }

        internal Maze Generate(Dimension dimension)
        {
            var maze = Maze.Build(dimension);
            maze.FillBoarderWith(CaseType.Wall);
            var entrance = (0, 1);
            var exit = (maze.Dimension.X - 1, maze.Dimension.Y - 2);
            OpenEntranceAndExit(maze, entrance, exit);
            Generate(maze, new Area(1, 1, maze.Dimension.X - 2, maze.Dimension.Y - 2));
            SetAllUnknowToPath(maze);
            return maze;
        }

        private void SetAllUnknowToPath(Maze maze)
        {
            for (int x = 0; x < maze.Dimension.X; x++)
            {
                for (int y = 0; y < maze.Dimension.Y; y++)
                {
                    if (maze[y, x] == CaseType.Unknow)
                    {
                        maze.Board[y, x] = CaseType.Path;
                    }
                }
            }
        }

        private void OpenEntranceAndExit(Maze maze, (int x, int y) entrance, (int x, int y) exit)
        {
            maze.Board[entrance.y, entrance.x] = CaseType.Unknow;
            maze.Entrance = entrance;
            maze.Board[exit.y, exit.x] = CaseType.Unknow;
            maze.Exit = exit;
        }
    }
}
