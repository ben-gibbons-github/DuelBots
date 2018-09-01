using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuelBots
{
    public class BackgroundBasic
    {
        public List<BackgroundLayer> Layers = new List<BackgroundLayer>();
        public int BackgroundNumber = 0;

        public static BackgroundBasic ReturnBackground(int i)
        {
            switch(i)
            {
                case 0:
                return new SpaceBack().Create(i);
                case 1:
                return new ForestBack().Create(i);
                case 2:
                return new CaveBack().Create(i);
                default:
                return new SpaceBack().Create(i);
            }
        }

        public virtual BackgroundBasic Create(int i)
        {
            BackgroundNumber = i;
            return this;
        }

        public void AddLayers(BackgroundLayer Layer)
        {
            Layers.Add(Layer);
        }

        public void Draw()
        {
            foreach (BackgroundLayer Layer in Layers)
                Layer.Draw();
        }

    }
}
