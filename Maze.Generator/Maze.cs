
using System;

namespace MazeGenerator
{
    public class Maze
    {
        private Maze(Dimension dimension, CaseType[,] board)
        {
            Dimension = dimension;
            Board = board;
        }

        public Dimension Dimension { get; internal set; }
        public CaseType[,] Board { get; internal set; }
        public (int x, int y) Entrance { get; internal set; }
        public (int x, int y) Exit { get; internal set; }

        public static Maze Build(Dimension dimension)
        {
            var board = new CaseType[dimension.Y, dimension.X];
            return new Maze(dimension, board);
        }

        internal void FillWith(CaseType caseType)
        {
            for (int x = 0; x < Dimension.X; x++)
            {
                for (int y = 0; y < Dimension.Y; y++)
                {
                    Board[y, x] = caseType;
                }
            }
        }

        public CaseType this[int y, int x] => this.Board[y, x];

        internal void FillBoarderWith(CaseType caseType)
        {
            for (int x = 0; x < Dimension.X; x++)
            {
                Board[0, x] = caseType;
                Board[Dimension.Y -1, x] = caseType;
            }
            for (int y = 1; y < Dimension.Y -1; y++)
            {
                Board[y, 0] = caseType;
                Board[y, Dimension.X - 1] = caseType;
            }
        }
    }

    public enum CaseType
    {
        Unknow,
        Path,
        Wall,
        OnMyWay,
        DeadEnd,
        Debug
    }
}