using System;

namespace MazeGenerator
{
    public interface IGenerator
    {
        Maze Generate(int x, int y, GeneratorType generatorType);
    }

    public class Generator : IGenerator
    {
        private IRand rand;

        public Generator(IRand rand)
        {
            this.rand = rand;
        }

        public Maze Generate(int x, int y, GeneratorType generatorType)
        {
            var dimension = new Dimension( (ushort)x,(ushort)y);

            switch (generatorType)
            {
                case GeneratorType.RecursiveSplit:
                    return new RecursiveSplitMazeGenerator(this.rand, dimension).Generate();
                case GeneratorType.Random:
                    return new RandomMazeGenerator(this.rand).Generate(dimension);
                default:
                    break;
            }

            throw new NotImplementedException();
        }
    }
}
