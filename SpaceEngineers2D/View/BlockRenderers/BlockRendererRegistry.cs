using System;
using System.Collections.Generic;
using System.Linq;
using SpaceEngineers2D.Model.Blocks;

namespace SpaceEngineers2D.View.BlockRenderers
{
    public class BlockRendererRegistry
    {
        private readonly List<Tuple<Type, IBlockRenderer>> _renderers = new List<Tuple<Type, IBlockRenderer>>();

        public IBlockRenderer Get(Type type)
        {
            return this._renderers.First(r => r.Item1.IsAssignableFrom(type)).Item2;
        }

        public void Add<TBlock, TRenderedBlock>(BlockRenderer<TRenderedBlock> renderer)
            where TBlock : TRenderedBlock
            where TRenderedBlock : Block
        {
            this._renderers.Add(new Tuple<Type, IBlockRenderer>(typeof(TBlock), renderer));
        }
    }
}
