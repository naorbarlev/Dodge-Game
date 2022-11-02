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
using Windows.Storage.Pickers;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls.Primitives;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using Windows.Media.Core;
using Windows.UI.Xaml.Navigation;

namespace DodgeProject
{
    public sealed partial class MainPage : Page
    {
        private BoardGame boardGame;
        private Rect windowRect;
        private Rectangle userRect;
        private Rectangle[] enemiesRectangles;
        private Rectangle[] giftsRectangles;
        private DispatcherTimer runningGameTimer;
        private const int RUNNING_GAME_TIMER = 10;
        private CommandBar cmdBar;
        private AppBarButton restart, pause, play, saveAs, stop;
        private const double CMD_BAR_HEIGHT = 70;
        private string loadedGame;

        private MediaElement collisionMediaElment, congratsMediaElment, giftMediaElment, countDownMediaElement, gameOverMediaElement;

        public MainPage()
        {
            this.InitializeComponent();

            Loaded += MainPage_Loaded;

            //StartGame();
            //createCmdBar();
            //createTimer(RUNNING_GAME_TIMER);
            //EventHandlers();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            MessageDilaogAsync("loaded", "loaded");
            StartGame();
            createCmdBar();
            createTimer(RUNNING_GAME_TIMER);
            EventHandlers();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!String.IsNullOrEmpty((string)e.Parameter))
            {
                loadedGame = (string)e.Parameter;
                MessageDilaogAsync(loadedGame, loadedGame);
            }
            base.OnNavigatedTo(e);

        }
        
       public void EventHandlers()
        {
            //event handlers
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            DelayAction(2000, new Action(() => { 

                runningGameTimer.Tick += RunnungGameTimer_Tick;
                boardGame.IsGameRunning = true;
            }));

            restart.Click += Restart_Click;
            play.Click += Play_Click;
            pause.Click += Pause_Click;
            saveAs.Click += SaveAs_Click;
            stop.Click += Stop_Click;
        }

        public void StartGame()
        {
            createMediaElements();

            windowRect = ApplicationView.GetForCurrentView().VisibleBounds;

            if(String.IsNullOrEmpty(loadedGame))
            {
                boardGame = new BoardGame((int)windowRect.Height, (int)windowRect.Width);
                MessageDilaogAsync("test", "test");

            }
            else
            {
                //read json and call for the new ctor
                boardGame = new BoardGame((int)windowRect.Height, (int)windowRect.Width);
                MessageDilaogAsync("test1", "test1");

            }

            userRect = CreateUserPiece(boardGame.User);

            enemiesRectangles = new Rectangle[boardGame.Enemies.Length];
            for (int i = 0; i < boardGame.Enemies.Length; i++)
            {
                enemiesRectangles[i] = CreateEnemy(boardGame.Enemies[i]);
            }

            giftsRectangles = new Rectangle[boardGame.Gifts.Length];
            updateLifes();
            
        }


        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if(boardGame.IsGameRunning)
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
            }
            boardGame.GameState.UpdateUser(boardGame.User);
            Canvas.SetLeft(userRect, boardGame.User.X);
            Canvas.SetTop(userRect, boardGame.User.Y);
        }

        private void RunnungGameTimer_Tick(object sender, object e)
        {
            if(boardGame.IsGameRunning)
            {
                //enemies move according to user
                EnemisMove();

                //calling for user collision eith bad guys
                if (boardGame.userCollision())
                {
                    //changing value to true after 500 ms in order to continue checking for collision
                    DelayAction(500, new Action(() => { boardGame.KeepCheckUserCollision = true; }));
                }

                //lottery in order to add gifts to board game              
                if (boardGame.randomGift())
                {
                    //Random rnd = new Random();

                    for (int i = 0; i < boardGame.Gifts.Length; i++)
                    {
                        //checking if one of the rect array does not has instance 
                        //checking if gift object does not used
                        if (giftsRectangles[i] == null && !boardGame.Gifts[i].IsUsed)
                        {
                            //breaking in order to put only one gift every lottery
                            giftsRectangles[i] = CreateGift(boardGame.Gifts[i]);
                            break;
                        }
                    }
                }

                for (int i = 0; i < boardGame.Enemies.Length; i++)
                {
                    if (boardGame.EnemiesColiision(boardGame.Enemies[i]))
                    {
                        enemiesRectangles[i].Visibility = Visibility.Collapsed;
                        mainCanvas.Children.Remove(enemiesRectangles[i]);
                        boardGame.Enemies[i].IsAlive = false;
                    }
                }

                //התנגשות בלב
                for (int i = 0; i < boardGame.Gifts.Length; i++)
                {
                    if (giftsRectangles[i] != null && boardGame.userHeartCollision(boardGame.Gifts[i]))
                    {
                        boardGame.Gifts[i].IsUsed = true;
                        giftsRectangles[i].Visibility = Visibility.Collapsed;
                        mainCanvas.Children.Remove(giftsRectangles[i]);
                    }
                }
                updateLifes();
                boardGame.GameState.UpdateCreatures(boardGame.Enemies, boardGame.Gifts);

                if (boardGame.User.Life <= 0)
                    lost();

                if (boardGame.IsWin())
                    win();
            }
        }

        private void EnemisMove()
        {
            for (int i = 0; i < boardGame.Enemies.Length; i++)
            {
                boardGame.MakeEnemyMove(boardGame.Enemies[i]);
                Canvas.SetLeft(enemiesRectangles[i], boardGame.Enemies[i].X);
                Canvas.SetTop(enemiesRectangles[i], boardGame.Enemies[i].Y);
            }
        }

        private void createTimer(int interval)
        {
            runningGameTimer = new DispatcherTimer();
            runningGameTimer.Interval = new System.TimeSpan(0, 0, 0, 0, interval);
            runningGameTimer.Start();
        }

        private void win()
        {
            loadedGame = "";
            boardGame.IsGameRunning = false;
            runningGameTimer.Stop();
            EndGameMessageDilaogAsync("Do you want to play again?", "You Won!");
        }
        private void lost()
        {
            loadedGame = "";
            boardGame.IsGameRunning = false;
            runningGameTimer.Stop();
            EndGameMessageDilaogAsync("Do you want to play again?", "You Lost :(");
        }

        public void updateLifes()
        {
            lifesCountTxt.Text = $"Life: {(boardGame.User.Life)}";
        }

        public async Task EndGameMessageDilaogAsync(string msg, string res)
        {
            MessageDialog dialog = new MessageDialog(msg, res);
            dialog.Commands.Add(new UICommand("Yes", null));
            dialog.Commands.Add(new UICommand("No", null));
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            var cmd = await dialog.ShowAsync();

            if (cmd.Label == "Yes")
            {
                this.Frame.Navigate(typeof(MainPage));
            }
            if (cmd.Label == "No")
            {
                this.Frame.Navigate(typeof(SplashScreen));
            }
        
        }

        public async Task MessageDilaogAsync(string msg, string res)
        {
            MessageDialog dialog = new MessageDialog(msg, res);
            var cmd = await dialog.ShowAsync();
        }

        private void createCmdBar()
        {
            ToolTip toolTipPause = new ToolTip();
            ToolTip toolTipReplay = new ToolTip();
            ToolTip toolTipPlay = new ToolTip();
            ToolTip toolTipSaveAs = new ToolTip();
            ToolTip toolTipStop = new ToolTip();

            BitmapIcon biPlay = new BitmapIcon();
            biPlay.UriSource = new Uri("ms-appx:///Assets/play.png");

            BitmapIcon biReplay = new BitmapIcon();
            biReplay.UriSource = new Uri("ms-appx:///Assets/refresh.png");

            BitmapIcon biPause = new BitmapIcon();
            biPause.UriSource = new Uri("ms-appx:///Assets/pause.png");

            BitmapIcon biSaveAs= new BitmapIcon();
            biSaveAs.UriSource = new Uri("ms-appx:///Assets/download.png");

            BitmapIcon biStop = new BitmapIcon();
            biStop.UriSource = new Uri("ms-appx:///Assets/stop.png");


            cmdBar = new CommandBar();
            cmdBar.Background = new SolidColorBrush(Color.FromArgb(100, 71, 130, 218));

            play = new AppBarButton();
            play.Label = "Play";
            play.Icon = biPlay;
            toolTipPlay.Content = "Press to play";
            ToolTipService.SetToolTip(play, toolTipPlay);

            restart = new AppBarButton();
            restart.Label = "Replay";
            restart.Icon = biReplay;
            toolTipReplay.Content = "Press to restart the game";
            ToolTipService.SetToolTip(restart, toolTipReplay);

            pause = new AppBarButton();
            pause.Label = "Pause";
            pause.Icon = biPause;
            toolTipPause.Content = "Press to pause the game";
            ToolTipService.SetToolTip(pause, toolTipPause);

            saveAs = new AppBarButton();
            saveAs.Label = "Save as";
            saveAs.Icon = biSaveAs;
            toolTipSaveAs.Content = "Press to save game";
            ToolTipService.SetToolTip(saveAs, toolTipSaveAs);

            stop = new AppBarButton();
            stop.Label = "Stop";
            stop.Icon = biStop;
            toolTipStop.Content = "Press to Stop";
            ToolTipService.SetToolTip(stop, toolTipStop);

            /*Cancels tab affect*/
            saveAs.IsTabStop = false;
            pause.IsTabStop = false;
            restart.IsTabStop = false;
            play.IsTabStop = false;
            stop.IsTabStop = false;
            cmdBar.IsTabStop = false;

            /*Cancel right left*/
            saveAs.CanBeScrollAnchor = false;
            pause.CanBeScrollAnchor = false;
            restart.CanBeScrollAnchor = false;
            play.CanBeScrollAnchor = false;
            stop.CanBeScrollAnchor = false;
            cmdBar.CanBeScrollAnchor = false;

            cmdBar.PrimaryCommands.Add(play);
            cmdBar.PrimaryCommands.Add(restart);
            cmdBar.PrimaryCommands.Add(pause);
            cmdBar.PrimaryCommands.Add(saveAs);
            cmdBar.PrimaryCommands.Add(stop);
            cmdBar.Height = CMD_BAR_HEIGHT;


            Canvas.SetLeft(cmdBar, 0);
            Canvas.SetTop(cmdBar, boardGame.Height-40);
            mainCanvas.Children.Add(cmdBar);
            
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            runningGameTimer.Stop();
            mainCanvas.Children.Remove(userRect);
            
            for (int i = 0; i < boardGame.Enemies.Length; i++)
            {
                if (boardGame.Enemies[i].IsAlive)
                {
                    mainCanvas.Children.Remove(enemiesRectangles[i]);
                }
                enemiesRectangles[i].Visibility = Visibility.Visible;
            }

            this.Frame.Navigate(typeof(MainPage));
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            runningGameTimer.Stop();
            this.Frame.Navigate(typeof(SplashScreen));
        }
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            boardGame.IsGameRunning = false;
        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            boardGame.IsGameRunning = true;
        }

        private async void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            boardGame.IsGameRunning = false;
            var jsonGS = JsonConvert.SerializeObject(boardGame.GameState);
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("JSON file", new List<string>() { ".json" });
            savePicker.SuggestedFileName = "MyGame";
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                // write to file
                await Windows.Storage.FileIO.WriteTextAsync(file, jsonGS);
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    EndGameMessageDilaogAsync("save", "save");
                }
                else
                {
                    EndGameMessageDilaogAsync("not save", "not save");
                }
            }
            else
            {
                EndGameMessageDilaogAsync("cancle", "cancle");
            }
        }
        public static void writeGameStateToFile(GameState gameState, string fileName)
        {
            var jsonString = JsonConvert.SerializeObject(gameState, Formatting.Indented);
            File.WriteAllText(fileName, jsonString);
        }
        public static void DelayAction(int millisecond, Action action)
        {
            var timer = new DispatcherTimer();
            timer.Tick += delegate
            {
                action.Invoke();
                timer.Stop();
            };

            timer.Interval = TimeSpan.FromMilliseconds(millisecond);
            timer.Start();
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

            //currentRect.Stroke = new SolidColorBrush(Colors.Red);
            //currentRect.StrokeThickness = 1;

            currentRect.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(enemy.ImgUrl))
            };
            Canvas.SetLeft(currentRect, enemy.X);
            Canvas.SetTop(currentRect, enemy.Y);
            mainCanvas.Children.Add(currentRect);
            return currentRect;

        }
        public Rectangle CreateGift(Gift gift)
        {
            Rectangle currentRect = new Rectangle();
            currentRect.Width = gift.Width;
            currentRect.Height = gift.Height;
            currentRect.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(gift.ImgUrl))
            };
            Canvas.SetLeft(currentRect, gift.X);
            Canvas.SetTop(currentRect, gift.Y);
            mainCanvas.Children.Add(currentRect);
            return currentRect;

        }

        private async void createMediaElements()
        {
            collisionMediaElment = new MediaElement();
            collisionMediaElment.IsLooping = false;
            collisionMediaElment.AutoPlay = false;
            collisionMediaElment.Volume = 0.4;
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets\\SoundEffects");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("explosion1.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            collisionMediaElment.SetSource(stream, file.ContentType);


            congratsMediaElment = new MediaElement();
            congratsMediaElment.IsLooping = false;
            congratsMediaElment.AutoPlay = false;
            congratsMediaElment.Volume = 0.4;
            folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets\\SoundEffects");
            file = await folder.GetFileAsync("congratulations1.mp3");
            stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            congratsMediaElment.SetSource(stream, file.ContentType);
            
            giftMediaElment = new MediaElement();
            giftMediaElment.IsLooping = false;
            giftMediaElment.AutoPlay = false;
            giftMediaElment.Volume = 0.4;
            folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets\\SoundEffects");
            file = await folder.GetFileAsync("gift.wav");
            stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            giftMediaElment.SetSource(stream, file.ContentType);
            

            countDownMediaElement = new MediaElement();
            countDownMediaElement.IsLooping = false;
            countDownMediaElement.AutoPlay = false;
            countDownMediaElement.Volume = 0.4;
            folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets\\SoundEffects");
            file = await folder.GetFileAsync("countdown.mp3");
            stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            countDownMediaElement.SetSource(stream, file.ContentType);
            

            gameOverMediaElement = new MediaElement();
            gameOverMediaElement.IsLooping = false;
            gameOverMediaElement.AutoPlay = false;
            gameOverMediaElement.Volume = 0.4;
            folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets\\SoundEffects");
            file = await folder.GetFileAsync("gameover.wav");
            stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            gameOverMediaElement.SetSource(stream, file.ContentType);
            

        }


    }
}
