using SpaceEngineers2D.Chemistry.Quantities;

namespace SpaceEngineers2D.Chemistry
{
    public class MixtureComponent
    {
        public Compound Compound { get; }

        /// <summary>
        /// In mol.
        /// </summary>
        public AmountOfSubstance Amount { get; }

        /// <summary>
        /// Number greater than 0  and less or equal 1.
        /// </summary>
        public double Portion { get; }

        public Mass Mass => Amount * Compound.Mass;

        public Volume Volume => Amount * Compound.Volume;

        public MixtureComponent(Compound compound, AmountOfSubstance amount, double portion)
        {
            Compound = compound;
            Amount = amount;
            Portion = portion;
        }

        public AmountOfSubstance GetAmountOfElement(Element element)
        {
            var elementCount = Compound.GetElementCount(element);
            return new AmountOfSubstance(Amount.Value * elementCount);
        }

        protected bool Equals(MixtureComponent other)
        {
            return Equals(Compound, other.Compound) && Amount.Equals(other.Amount);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MixtureComponent) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Compound != null ? Compound.GetHashCode() : 0) * 397) ^ Amount.GetHashCode();
            }
        }
    }
}