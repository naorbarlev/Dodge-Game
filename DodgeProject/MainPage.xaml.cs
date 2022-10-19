using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using System;
using Windows.UI.Xaml.Input;
using DodgeProject.Model;
using DodgeProject.Classes;
using Windows.System;
using Windows.UI.Xaml;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;

namespace DodgeProject
{
    public sealed partial class MainPage : Page
    {
        private BoardGame boardGame;
        private GameState gameState;
        private Rect windowRect;
        private Rectangle userRect;
        private Rectangle[] enemiesRectangles;
        private DispatcherTimer onGameTimer;
        private const int ON_GAME_TIMER = 10;
        private bool isGameRunning = false;


        

        public MainPage()
        {
            this.InitializeComponent();

            startGame();
            createTimer(ON_GAME_TIMER);

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            onGameTimer.Tick += onGameTimer_Tick;

        }

        public void startGame()
        {

            windowRect = ApplicationView.GetForCurrentView().VisibleBounds;

            boardGame = new BoardGame((int)windowRect.Height, (int)windowRect.Width);

            userRect = CreateUserPiece(boardGame.user);

            enemiesRectangles = new Rectangle[boardGame.enemies.Length];
            for (int i = 0; i < boardGame.enemies.Length; i++)
            {
                enemiesRectangles[i] = CreateEnemy(boardGame.enemies[i]);
            }

            lifesCountTxt.Text = $"Life: {boardGame.user.Life}";
        }

       

        //מייצר משתמש שניתן לשים על הקנבאס עפ הנתונים מהמחלקה
        public Rectangle CreateUserPiece(UserPiece userPiece)
        {
            Rectangle currentRect = new Rectangle();
            currentRect.Width = userPiece.Width;
            currentRect.Height = userPiece.Height;
            currentRect.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(userPiece.ImgUrl)),
               
            };
            Canvas.SetLeft(currentRect, userPiece.X);
            Canvas.SetTop(currentRect, userPiece.Y);
            mainCanvas.Children.Add(currentRect);
            return currentRect;

        }
        public Rectangle CreateEnemy(Enemy enemy)
        {
            Rectangle currentRect = new Rectangle();
            currentRect.Width = enemy.Width;
            currentRect.Height = enemy.Height;
            currentRect.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(enemy.ImgUrl))
            };
            Canvas.SetLeft(currentRect, enemy.X);
            Canvas.SetTop(currentRect, enemy.Y);
            mainCanvas.Children.Add(currentRect);
            return currentRect;

        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Up:
                    boardGame.MakeMove("Up");
                    break;

                case VirtualKey.Down:
                    boardGame.MakeMove("Down");
                    break;

                case VirtualKey.Left:
                    boardGame.MakeMove("Left");
                    break;

                case VirtualKey.Right:
                    boardGame.MakeMove("Right");
                    break;
 
                case VirtualKey.Space:
                    boardGame.MakeMove("Space");
                    break;
            }

            Canvas.SetLeft(userRect, boardGame.user.X);
            Canvas.SetTop(userRect, boardGame.user.Y);
        }

        private void onGameTimer_Tick(object sender, object e)
        {
            if(isGameRunning == false)
            {
                myMessageDilaog
                System.Threading.Thread.Sleep(3000);
                isGameRunning = true;
            }
            EnemisMove();
            boardGame.userCollision();
            for (int i = 0; i < boardGame.enemies.Length; i++)
            {
                if (boardGame.EnemiesColiision(boardGame.enemies[i]))
                {
                    enemiesRectangles[i].Visibility = Visibility.Collapsed;
                    mainCanvas.Children.Remove(enemiesRectangles[i]);
                    boardGame.enemies[i].IsAlive = false;
                }
            }

            lifesCountTxt.Text = $"Life: {boardGame.user.Life}";

            if (boardGame.user.Life <= 0)
            {
                lost();
            }
            if (boardGame.IsWin())
            {
                win();
            }
        }


        private void EnemisMove()
        {
            for (int i = 0; i < boardGame.enemies.Length; i++)
            {
                boardGame.MakeEnemyMove(boardGame.enemies[i]);
                Canvas.SetLeft(enemiesRectangles[i], boardGame.enemies[i].X);
                Canvas.SetTop(enemiesRectangles[i], boardGame.enemies[i].Y);
            }
        }

        private void createTimer(int OnGame)
        {
            onGameTimer = new DispatcherTimer();
            onGameTimer.Interval = new System.TimeSpan(0, 0, 0, 0, OnGame);
            onGameTimer.Start();
        }

        private void win()
        {
            onGameTimer.Stop();
            myMessageDilaog("You won try again!");
        }
        private void lost()
        {
            onGameTimer.Stop();
            myMessageDilaog("You lost try again!");
        }

        public void myMessageDilaog(string msg)
        {
            MessageDialog messageDialog = new MessageDialog(msg, "Lost");
            messageDialog.ShowAsync();
        }

        
    }
}
