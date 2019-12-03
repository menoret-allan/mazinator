using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGenerator
{
    public class RecursiveSplitMazeGenerator
    {
        private IRand rand;
        private Maze maze;

        public RecursiveSplitMazeGenerator(IRand rand, Dimension dimension)
        {
            this.rand = rand;
            maze = Maze.Build(dimension);
        }

        public void Generate(Area area)
        {
            List<Area> todo = new List<Area>();

            if (area.Width == 2 && area.Height == 2)
            {
                DealWithSquare2(maze, area);
                return;
            }

            if (area.Width > area.Height)
            {
                var raw = GetPossibilitiesForWidth(maze, area);
                Area areaLeft = new Area(area.X, area.Y, (ushort)(raw - area.X), area.Height);
                var isAreaLeftDone = false;
                Area areaRight = new Area((ushort)(raw + 1), area.Y, (ushort)(area.Width - (raw - area.X) - 1), area.Height);
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
                    DrawHeightWalls(maze, area, raw);
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
                var raw = GetPossibilitiesForHeight(maze, area);
                Area areaUp = new Area(area.X, area.Y, area.Width, (ushort)(raw - area.Y));
                var isAreaUpDone = false;
                Area areaDown = new Area(area.X, (ushort)(raw + 1), area.Width, (ushort)(area.Height - (raw - area.Y) - 1));
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
                    DrawWidthWalls(maze, area, raw);
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

            if (todo.Count == 2 && area.Height + area.Width > 40)
            {
                Parallel.Invoke(() => Generate(todo.First()), () => Generate(todo.Last()));
            }
            else
            {
                foreach (var item in todo)
                {
                    Generate(item);
                }
            }
        }

        private void DealWithSquare2(Maze maze, Area area)
        {
            List<(int x, int y)> possibilities = GetSquare2Possibilities(maze, area);

            if (possibilities.Any())
            {
                (int x, int y) = possibilities[rand.Next(possibilities.Count() - 1)];
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

        public ushort GetPossibilitiesForWidth(Maze maze, Area area)
        {
            var possibilities = new List<ushort>();
            for (ushort x = (ushort)(area.X + 1); x < area.X + area.Width - 1; x++)
            {
                if (maze[area.Y - 1, x] == CaseType.Wall || maze[area.Y + area.Height, x] == CaseType.Wall)
                {
                    possibilities.Add(x);
                }
            }
            if (!possibilities.Any())
            {
                possibilities.Add(area.X);
                possibilities.Add((ushort)(area.X + area.Width - 1));
            }

            return possibilities[this.rand.Next() % possibilities.Count()];
        }

        public ushort GetPossibilitiesForHeight(Maze maze, Area area)
        {
            var possibilities = new List<ushort>();
            for (ushort y = (ushort)(area.Y + 1); y < area.Y + area.Height - 1; y++)
            {
                if (maze[y, area.X - 1] == CaseType.Wall || maze[y, area.X + area.Width] == CaseType.Wall)
                {
                    possibilities.Add(y);
                }
            }
            if (!possibilities.Any())
            {
                possibilities.Add(area.Y);
                possibilities.Add((ushort)(area.Y + area.Height - 1));
            }

            return possibilities[this.rand.Next(possibilities.Count() - 1)];
        }

        public void DrawHeightWalls(Maze maze, Area area, ushort x)
        {
            List<ushort> walls = new List<ushort>();

            if (maze[area.Y - 1, x] == CaseType.Wall && !IsCaseAgainWallWithHole(maze, area, x, area.Y))
            {
                walls.Add(area.Y);
            }

            if (maze[area.Y + area.Height, x] == CaseType.Wall && !IsCaseAgainWallWithHole(maze, area, x, area.Y + area.Height - 1))
            {
                walls.Add((ushort)(area.Y + area.Height - 1));
            }

            for (ushort y = (ushort)(area.Y + 1); y < area.Y + area.Height - 1; y++)
            {
                if (!IsCaseAgainWallWithHole(maze, area, x, y))
                {
                    walls.Add(y);
                }
            }

            if (walls.Count() == area.Height)
            {
                walls.Remove(walls[rand.Next(walls.Count() - 1)]);
            }

            foreach (ushort y in walls)
            {
                maze.Board[y, x] = CaseType.Wall;
            }
        }

        private static bool IsCaseAgainWallWithHole(Maze maze, Area area, int x, int y)
        {
            return x == area.X && maze[y, x - 1] != CaseType.Wall ||
                  (x == area.X + area.Width - 1 && maze[y, x + 1] != CaseType.Wall);
        }

        public void DrawWidthWalls(Maze maze, Area area, ushort y)
        {
            List<ushort> walls = new List<ushort>();
            var holeNeeded = true;

            for (ushort x = area.X; x < area.X + area.Width; x++)
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
                    walls.Add(x);
                }
            }

            if (walls.Any())
            {

                if (maze[y, area.X - 1] == CaseType.Unknow && walls.First() == area.X)
                {
                    walls.Remove(walls.First());
                }
                else if (maze[y, area.X + area.Width] == CaseType.Unknow && walls.Last() == area.X + area.Width - 1)
                {
                    walls.Remove(walls.Last());
                }
                else if (holeNeeded)
                {
                    walls.Remove(walls[rand.Next() % walls.Count()]);
                }
            }

            foreach (ushort x in walls)
            {
                maze.Board[y, x] = CaseType.Wall;
            }
        }

        internal Maze Generate()
        {
            maze.FillBoarderWith(CaseType.Wall);
            var entrance = ((ushort)0, (ushort)1);
            var exit = ((ushort)(maze.Dimension.X - 1), (ushort)(maze.Dimension.Y - 2));
            OpenEntranceAndExit(maze, entrance, exit);
            Generate(new Area(1, 1, (ushort)(maze.Dimension.X - 2), (ushort)(maze.Dimension.Y - 2)));
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

        private void OpenEntranceAndExit(Maze maze, (ushort x, ushort y) entrance, (ushort x, ushort y) exit)
        {
            maze.Board[entrance.y, entrance.x] = CaseType.Unknow;
            maze.Entrance = entrance;
            maze.Board[exit.y, exit.x] = CaseType.Unknow;
            maze.Exit = exit;
        }
    }
}
