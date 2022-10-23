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
        private bool isAlive;

        public Enemy(double x, double y, int height, int width) : base(x, y, height, width)
        {
            Random rnd = new Random();
            this.Speed = rnd.Next(1,4);
            this.ImgUrl = "ms-appx:///Assets/goast.png";
            isAlive = true;
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public Boolean IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

       
    }
}
