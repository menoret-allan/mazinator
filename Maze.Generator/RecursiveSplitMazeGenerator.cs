using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator
{
    public class RecursiveSplitMazeGenerator
    {
        private Random rand;

        public RecursiveSplitMazeGenerator()
        {
            this.rand = new Random();
        }

        public void Generate(Maze maze, Area area)
        {
            List<Area> todo = new List<Area>();
            var walls = new List<(int x, int y)>();

            if (area.Width <= 1 || area.Height <= 1)
            { return; }

            if (area.Width == 2 && area.Height == 2)
            {
                DealWithSquare2(maze, area);
                return;
            }

            if (area.Width > area.Height)
            {
                List<int> possibilities = GetPossibilitiesForWidth(maze, area);
                var pos = rand.Next() % possibilities.Count();
                var raw = possibilities[pos];
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
                        testShit(maze);
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
                        testShit(maze);
                    }
                }

                if (isAreaLeftDone && isAreaRightDone) { }
                else
                {
                    walls = GetHeightWalls(maze, area, raw);
                    foreach (var wall in walls)
                    {
                        maze.Board[wall.y, wall.x] = CaseType.Wall;
                    }
                    testShit(maze);
                }

                if (!isAreaLeftDone)
                {
                    todo.Add(areaLeft);
                }
                if (!isAreaRightDone)
                {
                    todo.Add(areaRight);
                }
            }
            else
            {
                List<int> possibilities = GetPossibilitiesForHeight(maze, area);

                var pos = rand.Next() % possibilities.Count();
                var raw = possibilities[pos];
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
                        testShit(maze);
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
                        testShit(maze);
                    }
                }

                if (isAreaDownDone && isAreaUpDone) { }
                else
                {
                    walls = GetWidthWalls(maze, area, raw);
                    foreach (var wall in walls)
                    {
                        maze.Board[wall.y, wall.x] = CaseType.Wall;
                    }
                    testShit(maze);
                }

                if (!isAreaUpDone)
                {
                    todo.Add(areaUp);
                }
                if (!isAreaDownDone)
                {
                    todo.Add(areaDown);
                }
            }

          


            foreach (var areaToProcess in todo)
            {
                Generate(maze, areaToProcess);
            }
        }

        private void DealWithSquare2(Maze maze, Area area)
        {
            List<(int x, int y)> possibilities = GetSquare2Possibilities(maze, area);

            if (possibilities.Any())
            {
                var result = possibilities[rand.Next() % possibilities.Count()];
                maze.Board[result.y, result.x] = CaseType.Wall;
            }
            else
            {
                //throw new Exception("I poop in the glue!!!");
            }
        }

        private static List<(int x, int y)> GetSquare2Possibilities(Maze maze, Area area)
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

        private static List<(int x, int y)> GetSquare2PossibilitiesY(Maze maze, Area area, int forbiddenY)
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

        private static List<(int x, int y)> GetSquare2PossibilitiesX(Maze maze, Area area, int forbiddenX)
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
            if ((forbiddenX == area.X - 1 || maze[area.Y, area.X - 1] == CaseType.Wall) && maze[area.Y + 2, area.X] == CaseType.Wall)
            {
                possibilities.Add((area.X, area.Y + 1));
            }
            if ((forbiddenX == area.X + 2 || maze[area.Y, area.X + 2] == CaseType.Wall) && maze[area.Y + 2, area.X + 1] == CaseType.Wall)
            {
                possibilities.Add((area.X + 1, area.Y + 1));
            }

            return possibilities;
        }

        public static List<int> GetPossibilitiesForWidth(Maze maze, Area area)
        {
            var possibilities = new List<int>();
            for (int x = 1; x < area.Width - 1; x++)
            {
                if (maze[area.Y - 1, area.X + x] == CaseType.Wall || maze[area.Y + area.Height, area.X + x] == CaseType.Wall)
                {
                    possibilities.Add(area.X + x);
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
            for (int y = 1; y < area.Height - 1; y++)
            {
                if (maze[area.Y + y, area.X - 1] == CaseType.Wall || maze[area.Y + y, area.X + area.Width] == CaseType.Wall)
                {
                    possibilities.Add(area.Y + y);
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
            var holeNeeded = true;

            for (int y = 0; y < area.Height; y++)
            {
                if (x == area.X && maze[y, x - 1] != CaseType.Wall)
                {
                    holeNeeded = false;
                }
                else if ((x == area.X + area.Width - 1 && maze[y, x + 1] != CaseType.Wall))
                {
                    holeNeeded = false;
                }
                else
                {
                    walls.Add((x, area.Y + y));
                }
            }

            if (walls.Any())
            {
                if (maze[area.Y - 1, x] == CaseType.Unknow && walls.First() == (x, area.Y))
                {
                    walls.Remove(walls.First());
                }
                else if (maze[area.Y + area.Height, x] == CaseType.Unknow && walls.Last() == (x, area.Y + area.Height - 1))
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
        public List<(int x, int y)> GetWidthWalls(Maze maze, Area area, int y)
        {
            List<(int x, int y)> walls = new List<(int x, int y)>();
            var holeNeeded = true;

            for (int x = 0; x < area.Width; x++)
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
                    walls.Add((area.X + x, y));
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

        public IEnumerable<(int x, int y)> GetWalls(Area area, (int x, int y) pos, List<CaseType> crossBorder, Random rand)
        {
            var dict = new Dictionary<int, List<(int x, int y)>>();
            var count = 0;

            var top = Enumerable.Range(area.Y, pos.y - area.Y).Select(y => (pos.x, y)).ToList();
            if (crossBorder[0] != CaseType.Wall)
            {
                foreach (var toYield in top.Skip(1)) yield return toYield;
            }
            else
            {
                dict[count++] = top;
            }

            var down = Enumerable.Range(pos.y + 1, area.Y + area.Height - pos.y - 1).Select(y => (pos.x, y)).ToList();
            if (crossBorder[1] != CaseType.Wall)
            {
                foreach (var toYield in down.Take(down.Count - 1)) yield return toYield;
            }
            else
            {
                dict[count++] = down;
            }

            var left = Enumerable.Range(area.X, pos.x - area.X).Select(x => (x, pos.y)).ToList();
            if (crossBorder[2] != CaseType.Wall)
            {
                foreach (var toYield in left.Skip(1)) yield return toYield;
            }
            else
            {
                dict[count++] = left;
            }

            var right = Enumerable.Range(pos.x + 1, area.X + area.Width - pos.x - 1).Select(x => (x, pos.y)).ToList();
            if (crossBorder[3] != CaseType.Wall)
            {
                foreach (var toYield in right.Take(right.Count - 1)) yield return toYield;
            }
            else
            {
                dict[count++] = right;
            }

            if (dict.Count > 0)
            {
                var posNoHole = rand.Next() % dict.Count;
                var noHole = dict[posNoHole];
                foreach (var toYield in noHole) yield return toYield;
                dict.Remove(posNoHole);
            }

            foreach (var needToRemoveCase in dict)
            {
                var holePos = rand.Next() % needToRemoveCase.Value.Count;
                for (var i = 0; i < needToRemoveCase.Value.Count; i++)
                {
                    if (i != holePos)
                    {
                        yield return needToRemoveCase.Value[i];
                    }
                }
            }
            yield return pos;
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
            for (int x = 0; x < maze.Dimension.X ; x++)
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

        private void testShit(Maze maze)
        {
            for (int x = 1; x < maze.Dimension.X - 2; x++)
            {
                for (int y = 1; y < maze.Dimension.Y - 2; y++)
                {
                    if (maze[y, x] == CaseType.Unknow &&
                        maze[y - 1, x] == CaseType.Wall &&
                        maze[y, x - 1] == CaseType.Wall &&
                        maze[y + 1, x] == CaseType.Wall &&
                        maze[y, x + 1] == CaseType.Wall)
                    {
                        throw new Exception("Fuck my ass");
                    }
                }
            }
        }
    }
}
