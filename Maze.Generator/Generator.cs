using System;

namespace MazeGenerator
{
    public interface IGenerator
    {
        Maze Generate(int x, int y, GeneratorType generatorType);
    }

    public class Generator : IGenerator
    {
        private Random rand;

        public Generator(Random rand)
        {
            this.rand = rand;
        }

        public Maze Generate(int x, int y, GeneratorType generatorType)
        {
            var dimension = new Dimension(x, y);

            switch (generatorType)
            {
                case GeneratorType.Split:
                    return new SplitMazeGenerator(this.rand).Generate(dimension);
                case GeneratorType.RecursiveSplit:
                    return new RecursiveSplitMazeGenerator(this.rand).Generate(dimension);
                case GeneratorType.Random:
                    return new RandomMazeGenerator(this.rand).Generate(dimension);
                default:
                    break;
            }

            throw new NotImplementedException();
        }
    }
}
