namespace SpaceEngineers2D.Model.Chemicals
{
    public class Element
    {
        public string Symbol { get; }

        public string Name { get; }

        public int Group { get; }

        public int Period { get; }

        public int Electrons { get; }

        /// <summary>
        /// In g/mol
        /// </summary>
        public double Mass { get; }

        public Element(string symbol, string name, int @group, int period, int electrons, double mass)
        {
            Symbol = symbol;
            Name = name;
            Group = @group;
            Period = period;
            Electrons = electrons;
            Mass = mass;
        }
    }
}
