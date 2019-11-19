using System;

namespace MazeGenerator
{
    public interface IGenerator
    {
        Maze Generate(int x, int y, GeneratorType generatorType);
    }

    public class Generator : IGenerator
    {
        public Maze Generate(int x, int y, GeneratorType generatorType)
        {
            var dimension = new Dimension(x, y);

            switch (generatorType)
            {
                case GeneratorType.Split:
                    return new SplitMazeGenerator().Generate(dimension);
                case GeneratorType.Random:
                    return new RandomMazeGenerator().Generate(dimension);
                default:
                    break;
            }

            throw new NotImplementedException();
        }
    }
}
