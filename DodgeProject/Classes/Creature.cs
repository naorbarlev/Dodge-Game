using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodgeProject
{
    public abstract class Creature
    {
        private double x;
        private double y;
        private int height;
        private int width;
        private int speed;
        private string imgUrl;


        public Creature(double x, double y , int height, int width)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
        }

        public string ImgUrl
        {
            get { return imgUrl; }
            set { imgUrl = value; }
        }

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }


        public int Width
        {
            get { return width; }
            set { width = value; }
        }


        public int Height
        {
            get { return height; }
            set { height = value; }
        }


        public double Y
        {
            get { return y; }
            set { y = value; }
        }


        public double X    
        {
            get { return x; }
            set { x = value; }
        }

        public bool overlapRectangles(Creature creature)

        /*I am representing 2 rectangles using two points for the rectangle is (a,b) (c,b) (a,d) (c,d))*/
        {
            //if (this.life <= 0 || gp1.life <= 0)
            //{
            //    return false;
            //}
            /*                     a      b              c                        d              */
            double[] thisRec = { this.X, this.Y, this.X + this.width, this.Y + this.height};
            /*                     a      b              c                        d              */
            double[] otherRec = { creature.X, creature.Y, creature.X + creature.width, creature.Y + creature.height};

            /*Top left*/
            if (otherRec[2] >= thisRec[0] && otherRec[2] <= thisRec[2] && otherRec[3] >= thisRec[1] && otherRec[3] <= thisRec[3])
            {
                return true;
            }
            /*Top right*/
            if (otherRec[0] >= thisRec[0] && otherRec[0] <= thisRec[2] && otherRec[3] >= thisRec[1] && otherRec[3] <= thisRec[3])
            {
                return true;
            }
            /*bottom right*/
            if (otherRec[2] >= thisRec[0] && otherRec[2] <= thisRec[2] && otherRec[1] >= thisRec[1] && otherRec[1] <= thisRec[3])
            {
                return true;
            }
            /*bottom left*/
            if (otherRec[0] >= thisRec[0] && otherRec[0] <= thisRec[2] && otherRec[1] >= thisRec[1] && otherRec[1] <= thisRec[3])
            {
                return true;
            }
            return false;
        }

    }
}
