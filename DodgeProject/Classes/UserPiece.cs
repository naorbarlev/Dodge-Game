using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodgeProject
{
    public class UserPiece : Creature
    {
        protected const int USER_DIMENTION = 75;
        protected const int USER_SPEED = 25;

        private int life;

        public int Life
        {
            get { return life; }
            set { life = value; }
        }


        public UserPiece(double x, double y,  int height,  int width) : base(x, y, height, width)
        {
            this.Speed = USER_SPEED;
            this.life = 3;
            this.ImgUrl = "ms-appx:///Assets/face.png"; 
        }
    }
}
