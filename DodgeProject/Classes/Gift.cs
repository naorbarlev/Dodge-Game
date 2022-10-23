using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodgeProject.Classes
{
     public class Gift : Creature
    {
        private int life;
        private bool isUsed;

        public bool IsUsed
        {
            get { return isUsed; }
            set { isUsed = value; }
        }


        public int Life
        {
            get { return life; }
            set { life = value; }
        }
        public Gift(double x, double y, int height, int width) : base(x, y, height, width)
        {
            Random rnd = new Random();
            this.ImgUrl = "ms-appx:///Assets/heart.png";
            this.life = rnd.Next(1, 3);
            isUsed = false;
        }
    }
}
