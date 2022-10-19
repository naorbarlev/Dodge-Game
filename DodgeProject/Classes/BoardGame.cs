using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodgeProject.Model
{
    class BoardGame
    {

        public const int ENEMIES_COUNT = 10;
        public const double START_SPEED = 1;


        public Enemy[] enemies;
        public UserPiece user;
        private int width;
        private int height;
        private double enemySpeed;
        private bool isGameRunning;

        Random rnd = new Random();
        public BoardGame(int height, int width)
        {
            this.height = height;
            this.width = width;

            enemies = new Enemy[ENEMIES_COUNT];

            //יש להבין איזה גבולות לשים לאויב במספר האקראי
            for (int i = 0; i < ENEMIES_COUNT; i++)
            {
                enemies[i] = new Enemy(rnd.Next(1,width) , rnd.Next(1, height), 30 ,30);
                enemies[i].Index = i;
            }

            //להבין איזה מספרים כדאי ולהתאים לגבולות
            user = new UserPiece(200, 200, 50, 50);
            isGameRunning = true;
        }

        public void NewGame()
        {

        }

        public bool IsWin()
        {
            int aliveEnemies = 0;
            if (user.Life <= 0)
            {
                return false;
            }

            for (int i = 0; i < ENEMIES_COUNT; i++)
            {
                if (enemies[i].IsAlive)
                {
                    aliveEnemies++;
                }
            }
            return aliveEnemies < 2;
        }

        public bool EnemiesColiision(Enemy enemy)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemy.Index == i)
                    return false;

                if(enemy.overlapRectangles(enemies[i]) && !(enemy.overlapRectangles(user) || enemies[i].overlapRectangles(user)))
                {
                    return true;
                }
            }
            return false;
        }

        public void ResumeGame()
        {

        }
        public void MakeEnemyMove(Enemy enemy)
        {
            //enemy.Speed = 1;//דריסה רק כדי לבדוק את המהירויות


            if (enemy.X > user.X)
            {
                enemy.X -= enemy.Speed;
            }
            else
                if (enemy.X < user.X)
            {
                enemy.X += enemy.Speed;
            }

            if (enemy.Y > user.Y)
            {
                enemy.Y -= enemy.Speed;
            }
            else
                if (enemy.Y < user.Y)
            {
                enemy.Y += enemy.Speed;
            }
        }

        public void userCollision()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].IsAlive && enemies[i].overlapRectangles(user))
                {
                    if(user.Life > 0)
                        user.Life--;
                }
            }
        }

        public void MakeMove(String dir)
        {


            if(isGameRunning)
            {
                switch (dir)
                {
                    case "Up":
                        if (user.Y > 0)
                            user.Y -= user.Speed;
                        else
                            //user.Y = 0;//למצב בו אין אפשרות לצאת מהגבולות
                            user.Y = height - user.Height;
                        break;

                    case "Down":
                        if (user.Y < height - user.Height)
                            user.Y += user.Speed;
                        else
                            //user.Y = height - user.Height; //למצב בו אין אפשרות לצאת מהגבולות
                            user.Y = 0;
                        break;

                    case "Right":
                        if (user.X < width - user.Width)
                            user.X += user.Speed;
                        else
                            //user.X = width - user.Width;למצב בו אין אפשרות לצאת מהגבולות//
                            user.X = 0;
                        break;

                    case "Left":
                        if (user.X > 0)
                            user.X -= user.Speed;
                        else
                            //user.X = 0;//למצב בו אין אפשרות לצאת מהגבולות
                            user.X = width - user.Width;
                        break;

                    case "Space":
                        user.X = rnd.Next(1, this.width);
                        user.Y = rnd.Next(1, this.height);
                        break;
                }
            }
            
        }
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        public double EnemySpeed
        {
            get { return enemySpeed; }
            set { enemySpeed = value; }
        }


        public bool IsGameRunning
        {
            get { return isGameRunning; }
            set { isGameRunning = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public bool IsGameOver()
        {
            return true;
        }
    }
}
