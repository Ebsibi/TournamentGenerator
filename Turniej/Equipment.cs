using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turniej
{
    public class Equipment
    {
        public string rarity;
        public int lvl;
        public double factor;

        public Equipment(string rarity, int lvl)
        {
            this.rarity = rarity;
            this.lvl = lvl;
        }

        public string Print()
        {
            return "("+this.lvl+") *"+this.rarity+"*";
        }

        public void CalculateFactor()
        {
            switch (this.rarity)
            {
                case "legendary":
                    factor = 0.5 * lvl;
                        break;
                case "heroic":
                    factor = 0.4 * lvl;
                        break;
                case "upgraded":
                    factor = 0.35 * lvl;
                        break;
                case "unique":
                    factor = 0.3 * lvl;
                        break;
                case "common":
                    factor = 0.2 * lvl;
                        break;
            }

        }
    }
}
