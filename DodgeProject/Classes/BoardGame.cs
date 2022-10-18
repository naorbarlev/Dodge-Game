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
                enemies[i] = new Enemy(rnd.Next(50,500) , rnd.Next(50, 500), 30 ,30);
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
            return true;
        }

        public bool EnemiesColiision(Enemy enemy)
        {

            for (int i = 0; i < enemies.Length; i++)
            {
                if(enemy.Index != i && enemies[i].overlapRectangles(enemy) && !(enemy.overlapRectangles(user) || enemies[i].overlapRectangles(user)))
                {
                    return true;
                }
            }
            return false;
        }

        public void ResumeGame()
        {

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
                            user.Y = 0;
                        break;

                    case "Down":
                        if (user.Y < height - user.Height)
                            user.Y += user.Speed;
                        else
                            user.Y = height - user.Height;
                        break;

                    case "Right":
                        if (user.X < width - user.Width)
                            user.X += user.Speed;
                        else
                            user.X = width - user.Width;
                        break;

                    case "Left":
                        if (user.X > 0)
                            user.X -= user.Speed;
                        else
                            user.X = 0;
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

        public void MakeEnemyMove(Enemy enemy)
        {
            enemy.Speed = 1;//דריסה רק כדי לבדוק את המהירויות


            if (enemy.X > user.X)
            {
                enemy.X -= enemy.Speed;
            }
            else
                if (enemy.X < user.X)
                {
                    enemy.X += enemy.Speed;
                }

            if (enemy.Y> user.Y)
            {
                enemy.Y -= enemy.Speed;
            }
            else 
                if (enemy.Y < user.Y)
                {
                    enemy.Y += enemy.Speed;
                }
        }

        public bool userCollision()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                
                if ( enemies[i].overlapRectangles(user))
                    //אם יהיה שדרוג למספר חיים שיש לשחקן כאן המקום להויד בערך
                    return true;
            }
            return false;
        }
    }
}
