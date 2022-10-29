using DodgeProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodgeProject.Classes
{
    public class GameState
    {
        public const int ENEMIES_COUNT = 10;
        public const int GIFTS_COUNT = 5;

        private Enemy[] enemies;
        private UserPiece user;
        private Gift[] gifts;

        public GameState()
        {
            enemies = new Enemy[ENEMIES_COUNT];
            gifts = new Gift[GIFTS_COUNT];
        }

        public void UpdateCreatures(Enemy[] enemies, Gift[] gifts )
        {
            this.gifts = gifts;
            this.enemies = enemies;
        }

        public void UpdateUser(UserPiece currentUser)
        {
            this.user = currentUser;
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
    }
}
