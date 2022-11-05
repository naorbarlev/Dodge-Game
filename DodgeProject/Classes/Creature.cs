namespace DodgeProject
{
    public class Creature
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

        public double GetMiddleX()
        {
            return this.x + this.height / 2;
        }
        public double GetMiddleY()
        {
            return this.y + this.Width / 2;
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
        
        public bool overlapRectangles1(Creature ctr)
        {
            /*Top left*/
            //check if ctr overlap this obj's top left
            if (ctr.X + ctr.width >= this.X && 
                ctr.X + ctr.width <= this.X + this.width &&
                ctr.Y + ctr.height >= this.Y &&
                ctr.Y + ctr.height <= this.Y + this.height)
            {
                return true;
            }
            /*Top right*/
            if (ctr.X >= this.X &&
                ctr.X <= this.X + this.width &&
                ctr.Y + ctr.height >= this.Y &&
                ctr.Y + ctr.height <= this.Y + this.height)
            {
                return true;
            }
            /*bottom right*/
            if (ctr.X + ctr.width >= this.X &&
                ctr.X + ctr.width <= this.X + this.width &&
                ctr.Y >= this.Y &&
                ctr.Y <= this.Y + this.height)
            {
                return true;
            }
            /*bottom left*/
            if (ctr.X >= this.X &&
                ctr.X <= this.X + this.width &&
                ctr.Y >= this.Y &&
                ctr.Y <= this.Y + this.height)
            {
                return true;
            }
            return false;

        }
        public bool overlapRectangles(Creature ctr)
        {
            if (ctr.X + ctr.Width >= this.X &&     // ctr right edge past this.obj left
                ctr.X <= this.X + this.Width &&       // ctr left edge past this.obj right
                ctr.Y + ctr.Height >= this.Y &&       // ctr top edge past this.obj bottom
                ctr.Y <= this.Y + this.Height)      // ctr bottom edge past this.objs top
            {
                return true;
            }
            return false;
        }
    }

}
