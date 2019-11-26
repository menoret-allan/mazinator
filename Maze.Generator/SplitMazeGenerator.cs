using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator
{
    public class SplitMazeGenerator
    {
        public SplitMazeGenerator()
        {
        }

        public void Generate(Maze maze, Area area)
        {
            if (area.Width <= 1 || area.Height <= 1)
            { return; }
            var rand = new Random();

            if (area.Width == 2 && area.Height == 2)
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

                if (possibilities.Any())
                {
                    var result = possibilities[rand.Next() % possibilities.Count()];
                    maze.Board[result.y, result.x] = CaseType.Wall;
                }
                return;
            }


            if (area.Width == 2)
            {
                var possibilities = Enumerable.Range(area.Y + 1, area.Height - 2).ToList();
                int posToExtract = rand.Next() % possibilities.Count();
                var y = possibilities[posToExtract];

                var doubleChoice = rand.Next() % 2 == 0;
                if (doubleChoice && TrySetWallForWidth(maze, (area.X, y), -1))
                {
                }
                else if (TrySetWallForWidth(maze, (area.X + 1, y), 1))
                {
                }
                else
                {
                    TrySetWallForWidth(maze, (area.X, y), -1);
                }

                Generate(maze, new Area(area.X, area.Y, area.Width, posToExtract+1));
                Generate(maze, new Area(area.X, y + 1, area.Width, area.Height - posToExtract - 2));

                return;
            }

            if (area.Height == 2)
            {
                var possibilities = Enumerable.Range(area.X + 1, area.Width - 2).ToList();
                int posToExtract = rand.Next() % possibilities.Count();
                var x = possibilities[posToExtract];

                var doubleChoice = rand.Next() % 2 == 0;
                if (doubleChoice && TrySetWallForHeight(maze, (x, area.Y), -1))
                {
                }
                else if (TrySetWallForHeight(maze, (x, area.Y + 1), 1)) { }
                else
                {
                    TrySetWallForHeight(maze, (x, area.Y), -1);
                }

                Generate(maze, new Area(area.X, area.Y, posToExtract+1, area.Height));
                Generate(maze, new Area(x + 1, area.Y, area.Width - posToExtract -2, area.Height));

                return;
            }

            (int x, int y) pos = (0, 0);
            if (area.Height > 2)
            {
                List<int> heightPossibility = new List<int>();
                for (int y = area.Y + 1; y < area.Y - 1 + area.Height; y++)
                {
                    heightPossibility.Add(y);
                }

                if (heightPossibility.Any())
                {
                    pos.y = heightPossibility[rand.Next() % heightPossibility.Count];
                }
            }

            if (area.Width > 2)
            {
                List<int> widthPossibility = new List<int>();
                for (int x = area.X + 1; x < area.X - 1 + area.Width; x++)
                {
                    widthPossibility.Add(x);
                }

                if (widthPossibility.Any())
                {
                    pos.x = widthPossibility[rand.Next() % widthPossibility.Count];
                }
            }

            List<CaseType> crossBorder = new List<CaseType>
            {
                maze[area.Y - 1, pos.x],
                maze[area.Y + area.Height, pos.x],
                maze[pos.y, area.X - 1],
                maze[pos.y, area.X + area.Width],
            };
            var walls = GetWalls(area, pos, crossBorder, rand);
            foreach (var wall in walls)
            {
                maze.Board[wall.y, wall.x] = CaseType.Wall;
            }

            var newAreas = GetAreas(area, pos);
            foreach (var newArea in newAreas)
            {
                Generate(maze, newArea);
            }
        }

        private bool TrySetWallForWidth(Maze maze, (int x, int y) pos, int move)
        {
            if (maze[pos.y, pos.x + move] == CaseType.Wall)
            {
                maze.Board[pos.y, pos.x] = CaseType.Wall;
                return true;
            }
            return false;
        }

        private bool TrySetWallForHeight(Maze maze, (int x, int y) pos, int move)
        {
            if (maze[pos.y + move, pos.x] == CaseType.Wall)
            {
                maze.Board[pos.y, pos.x] = CaseType.Wall;
                return true;
            }
            return false;
        }

        private IEnumerable<Area> GetAreas(Area area, (int x, int y) pos)
        {
            yield return new Area(area.X, area.Y, pos.x - area.X, pos.y - area.Y); // areaUpLeft
            yield return new Area(pos.x + 1, area.Y, area.Width - (pos.x - area.X) - 1, pos.y - area.Y); // areaUpRight
            yield return new Area(area.X, pos.y + 1, pos.x - area.X, area.Height - (pos.y - area.Y) - 1); // areaDownLeft
            yield return new Area(pos.x + 1, pos.y + 1, area.Width - (pos.x - area.X) - 1, area.Height - (pos.y - area.Y) - 1); // areaDownRight
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
            for (int x = 1; x < maze.Dimension.X - 1; x++)
            {
                for (int y = 1; y < maze.Dimension.Y - 1; y++)
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
            maze.Board[entrance.y, entrance.x] = CaseType.Path;
            maze.Entrance = entrance;
            maze.Board[exit.y, exit.x] = CaseType.Path;
            maze.Exit = exit;
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
