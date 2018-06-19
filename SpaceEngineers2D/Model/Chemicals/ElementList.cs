using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineers2D.Model.Chemicals
{
    public class ElementList
    {
        private readonly IList<Element> _all = new List<Element>();


        public Element Hydrogen { get; } = new Element("H", "Hydrogen", 1, 1, 1, 1.008);

        public Element Helium { get; } = new Element("He", "Helium", 2, 1, 2, 4.002);


        public Element Carbon { get; } = new Element("C", "Carbon", 14, 2, 6, 12.011);

        public Element Nitrogen { get; } = new Element("N", "Nitrogen", 15, 2, 7, 14.0067);

        public Element Oxygen { get; } = new Element("O", "Oxygen", 16, 2, 8, 15.999);


        public Element Iron { get; } = new Element("Fe", "Iron", 8, 4, 26, 55.845);

        public ElementList()
        {
            _all = GetType().GetProperties().Select(property => (Element)property.GetValue(this)).ToList();
        }

        public IList<Element> GetAll()
        {
            return _all;
        }
    }
}
