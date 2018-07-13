using SpaceEngineers2DCodeGeneration.Templates;

namespace SpaceEngineers2DCodeGeneration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            QuantityGenerator.Generate(
                "..\\..\\..\\SpaceEngineers2D\\Chemistry\\Quantities",
                "SpaceEngineers2D.Chemistry.Quantities",
                Quantities.Get(),
                Convertions.Get());
        }
    }
}
