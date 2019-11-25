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

            if (area.Width == 2 || area.Height == 2)
            {
                return; // special case need to have just one split
            }

            (int x, int y) pos = (0, 0);
            if (area.Height > 2)
            {
                List<int> heightPossibility = new List<int>();
                for (int y = area.Y + 1; y < area.Y - 1 + area.Height; y++)
                {
                    if (maze[y, area.X - 1] != CaseType.Path && maze[y, area.X + area.Width] != CaseType.Path) // out of board
                    {
                        heightPossibility.Add(y);
                    }
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
                    if (maze[area.Y - 1, x] != CaseType.Path && maze[area.Y + area.Height, x] != CaseType.Path)
                    {
                        widthPossibility.Add(x);
                    }
                }

                if (widthPossibility.Any())
                {
                    pos.x = widthPossibility[rand.Next() % widthPossibility.Count];
                }
            }

            // generate the 3 holes
            var walls = GetWalls(area, pos, rand);
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

        private IEnumerable<Area> GetAreas(Area area, (int x, int y) pos)
        {
            yield return new Area(area.X, area.Y, pos.x - area.X, pos.y - area.Y); // areaUpLeft
            yield return new Area(pos.x + 1, area.Y, area.Width - (pos.x - area.X) - 1, pos.y - area.Y); // areaUpRight
            yield return new Area(area.X, pos.y + 1, pos.x - area.X, area.Height - (pos.y - area.Y) - 1); // areaDownLeft
            yield return new Area(pos.x + 1, pos.y + 1, area.Width - (pos.x - area.X) - 1, area.Height - (pos.y - area.Y) - 1); // areaDownRight
        }

        public IEnumerable<(int x, int y)> GetWalls(Area area, (int x, int y) pos, Random rand)
        {
            var result = new List<(int x, int y)>();
            var top = Enumerable.Range(area.Y, pos.y - area.Y).Select(y => (pos.x, y)).ToList();
            var down = Enumerable.Range(pos.y + 1, area.Y + area.Height - pos.y-1).Select(y => (pos.x, y)).ToList();
            var left = Enumerable.Range(area.X, pos.x - area.X).Select(x => (x, pos.y)).ToList();
            var right = Enumerable.Range(pos.x + 1, area.X + area.Width - pos.x-1).Select(x => (x, pos.y)).ToList();

            var dict = new Dictionary<int, List<(int x, int y)>>
            {
                {0, top },
                {1, down },
                {2, left },
                {3, right }
            };

            // Todo: check for the position depanding on the number of items
            var posNoHole = rand.Next() % 4;
            var noHole = dict[posNoHole];
            foreach (var toYield in noHole) yield return toYield;
            dict.Remove(posNoHole);

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
            return maze;
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
