using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator
{
    public class SolverPerfectMaze
    {
        public SolverPerfectMaze(Maze maze)
        {
            this.maze = maze;
            path = new Stack<(int x, int y)>();
            maze.Board[maze.Entrance.y, maze.Entrance.x] = CaseType.OnMyWay;
            path.Push(maze.Entrance);
        }

        private Maze maze;
        private Stack<(int x, int y)> path;

        public IEnumerable<(int x, int y, CaseType newState)> NextStep()
        {
            yield return (maze.Entrance.x, maze.Entrance.y, CaseType.OnMyWay);
            for (var pos = path.Peek();pos != maze.Exit; pos = path.Peek())
            {
                var nextPosssibilities = GetNeighboors(pos.x, pos.y);

                if (nextPosssibilities.Any())
                {
                    var nextPos = nextPosssibilities.First();
                    maze.Board[nextPos.y, nextPos.x] = CaseType.OnMyWay;
                    path.Push(nextPos);
                    yield return (nextPos.x, nextPos.y, CaseType.OnMyWay);
                }
                else
                {
                    var goBack = path.Pop();
                    maze.Board[goBack.y, goBack.x] = CaseType.DeadEnd;
                    yield return (goBack.x, goBack.y, CaseType.DeadEnd);
                }
            }
        }

        private List<(int x, int y)> GetNeighboors(int x, int y)
        {
            List<(int x, int y)> result = new List<(int x, int y)>();
            if (x > 0)
            {
                if (maze[y, x - 1] == CaseType.Path)
                {
                    result.Add( (x - 1, y));
                }
            }

            if (x < maze.Dimension.X - 1)
            {
                if (maze[y, x + 1] == CaseType.Path)
                {
                    result.Add((x + 1, y));
                }
            }

            if (y > 0)
            {
                if (maze[y - 1, x] == CaseType.Path)
                {
                    result.Add((x, y - 1));
                }
            }

            if (y < maze.Dimension.Y - 1)
            {
                if (maze[y + 1, x] == CaseType.Path)
                {
                    result.Add((x, y + 1));
                }
            }
            return result;
        }
    }
}
