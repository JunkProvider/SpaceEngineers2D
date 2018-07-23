using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using SpaceEngineers2D.Model.BlockBlueprints;
using SpaceEngineers2D.Model.Blueprints;
using SpaceEngineers2D.Chemistry;
using SpaceEngineers2D.Chemistry.Quantities;

namespace SpaceEngineers2D.Model.Blocks
{
    using System.Windows.Media;

    using Items;

    public class BlockTypes
    {
        public readonly StandardBlockType Rock;

        public readonly StandardBlockType Ore;

        public readonly ConcreteBlockType Concrete;

        public readonly GrassBlockType Grass;

        public BlockTypes(ElementList elements, CompoundList compounds)
        {
            Rock = new StandardBlockType(
                image: LoadImage("rock"),
                getDroppedItemsFunc: () => new List<IItem>
                {
                    new MixtureItem(Mixture.FromSingleCompound(compounds.Fe3O4, Volume.FromLiters(10), Temperature.FromKelvin(295 * 6)))
                });

            Ore = new StandardBlockType(
                image: LoadImage("ore"),
                getDroppedItemsFunc: () => new List<IItem>
                {
                    new MixtureItem(Mixture.FromSingleCompound(compounds.GetForElement(elements.Carbon), Volume.FromLiters(10), Temperature.FromKelvin(295)))
                });

            var concreteBlueprint = new BlockBlueprint(new List<BlockBlueprintComponent>
            {
                new BlockBlueprintComponent(
                    new MixtureBlueprintComponent(
                        "200L Magnetite",
                        Volume.FromLiters(10), 
                        mixture =>
                            {
                                return mixture.Components.Count == 1 && mixture.Components.Single().Compound == compounds.Fe3O4;
                            }),
                    100)
            });

            Concrete = new ConcreteBlockType(
                LoadImage("stone_slab_side"),
                concreteBlueprint);

            Grass = new GrassBlockType();
        }

        private static ImageSource LoadImage(string file)
        {
            var x = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new BitmapImage(new Uri(x + "\\Assets\\Images\\" + file + ".png"));
        }
    }
}
