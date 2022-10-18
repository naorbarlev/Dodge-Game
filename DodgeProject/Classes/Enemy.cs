using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodgeProject
{
    public class Enemy : Creature
    {
        private int index;

        public Enemy(double x, double y, int height, int width) : base(x, y, height, width)
        {
            Random rnd = new Random();
            this.Speed = rnd.Next(1,3);
            this.ImgUrl = "ms-appx:///Assets/goast.png";
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

    }
}
