using DodgeProject.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodgeProject.Model
{
    public class BoardGame
    {
        public const int ENEMIES_COUNT = 10;
        public const int GIFTS_COUNT = 5;
        public const double START_SPEED = 1;

        private Enemy[] enemies;
        private UserPiece user;
        private Gift[] gifts;
        private GameState gameState;

        private int width;
        private int height;
        private double enemySpeed;
        private bool isGameRunning;
        private bool keepCheckUserCollision;

        Random rnd = new Random();

        public BoardGame(int height, int width)
        {
            
            this.height = height;
            this.width = width;
            keepCheckUserCollision = true; // used for delying 500ms user collision after a user collision

            enemies = new Enemy[ENEMIES_COUNT];
            gifts = new Gift[GIFTS_COUNT];

            int size; // random number that will be used for H & W to create square

            for (int i = 0; i < GIFTS_COUNT; i++)
            {
                size = rnd.Next(30, 55);
                gifts[i] = new Gift(rnd.Next(1, width - size), rnd.Next(1, height - size), size, size);
                gifts[i].IsUsed = false;
                gifts[i].Index = i;
            }

            for (int i = 0; i < ENEMIES_COUNT; i++)
            {
                size = rnd.Next(30, 55);
                enemies[i] = new Enemy(rnd.Next(1, width - size) , rnd.Next(1, height - size), size, size);
                enemies[i].Index = i;
            }

            user = new UserPiece(rnd.Next(1, width - 40), rnd.Next(1, height - 40), 40, 40);

            gameState = new GameState(user, enemies,gifts);
        }
        public BoardGame(UserPiece loadedUser, Enemy[]loadedEnemies, Gift[] loadedGifts, int height, int width)
        {

            this.height = height;
            this.width = width;
            keepCheckUserCollision = true; // used for delying 500ms user collision after a user collision

            enemies = new Enemy[ENEMIES_COUNT];
            gifts = new Gift[GIFTS_COUNT];

            int size; // random number that will be used for H & W to create square

            for (int i = 0; i < GIFTS_COUNT; i++)
            {
                gifts[i] = new Gift(loadedGifts[i].X, loadedGifts[i].Y, loadedGifts[i].Height, loadedGifts[i].Width);
                gifts[i].IsUsed = loadedGifts[i].IsUsed;
                gifts[i].Index = loadedGifts[i].Index;
                gifts[i].Life = loadedGifts[i].Life;
            }

            for (int i = 0; i < ENEMIES_COUNT; i++)
            {
                enemies[i] = new Enemy(loadedEnemies[i].X, loadedEnemies[i].Y, loadedEnemies[i].Height, loadedEnemies[i].Width);
                enemies[i].Index = loadedEnemies[i].Index;
                enemies[i].IsAlive = loadedEnemies[i].IsAlive;
                enemies[i].Speed = loadedEnemies[i].Speed;
            }

            user = new UserPiece(loadedUser.X, loadedUser.Y, 40, 40);
            user.Life = loadedUser.Life; 

            gameState = new GameState(loadedUser, loadedEnemies, loadedGifts);
        }

        public bool IsWin()
        {
            int aliveEnemies = 0; //counter for enemies that on canvas
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
            //return true if there is one or less enenies on canvas
            return aliveEnemies < 2;
        }

        public bool EnemiesColiision(Enemy enemy)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemy.Index == i)
                    return false;

                //if((enemies[i].overlapRectangles(enemy) || enemy.overlapRectangles(enemies[i])) && !(enemy.overlapRectangles(user) || enemies[i].overlapRectangles(user)))
                //{
                //    return true;
                //}
                if (enemy.overlapRectangles(enemies[i]) || enemies[i].overlapRectangles(enemy))
                {
                    return true;
                }

            }
            return false;
        }

        public bool randomGift()
        {
            int a = rnd.Next(0,10);
            int b = rnd.Next(0, 200);
            if (a == b)
                return true;
            return false;
        }

       
        public void MakeEnemyMove(Enemy enemy)
        {
            if (enemy.GetMiddleX() > user.GetMiddleX() - enemy.Width / 2)
            {
                enemy.X -= enemy.Speed;
            }
            else if (enemy.GetMiddleX() < user.GetMiddleX() + enemy.Width / 2)
            {
                enemy.X += enemy.Speed;
            }
            if (enemy.GetMiddleY() > user.GetMiddleY() - enemy.Height / 2)
            {
                enemy.Y -= enemy.Speed;
            }
            else if (enemy.GetMiddleY() < user.GetMiddleY() + enemy.Height / 2)
            {
                enemy.Y += enemy.Speed;
            }

        }
       
        public bool userCollision()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (keepCheckUserCollision && enemies[i].IsAlive && enemies[i].overlapRectangles(user))
                {
                    if (user.Life > 0)
                    {
                        keepCheckUserCollision = false;
                        user.Life--;
                        return true;
                    }
                }
            }
            return false;
        }
        public bool userHeartCollision(Gift gift)
        {
            if (gift.IsUsed == false && gift.overlapRectangles(user))
            {
                user.Life += gift.Life;
                return true;
            }
            return false;
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

        public Gift[] Gifts
        {
            get { return gifts; }
            set { gifts = value; }
        }

        public Enemy[] Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        }

        public UserPiece User
        {
            get { return user; }
            set { user = value; }
        }
        public bool KeepCheckUserCollision
        {
            get { return keepCheckUserCollision; }
            set { keepCheckUserCollision = value; }
        }

        public GameState GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }
    }
}
