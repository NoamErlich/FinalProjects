using Billiard.GameObjects;
using GameEngine.GameServices;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Threading;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Billiard.GameServices
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private GameManager _gameManager; // The game manager that manages the entire game.
        List<string> ballList = new List<string>(); //Ball list for all the balls file names.

        /// <summary>
        /// Initializing the page.
        /// </summary>
        public GamePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The method is called when the page is loaded, sets the Height and Width for the main grid, sets the game state
        /// to start and adds OnTurn event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Manager.GameEvent.OnTurn += OnTurn;
            _gameManager = new GameManager(scene);
            _gameManager.Start();
            mainGrid.Height = ActualHeight;
            mainGrid.Width = ActualWidth;

            Table.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Tables/{GameManager.User.UsingProductName}.png"));
        }

        /// <summary>
        /// This method handles various game scenarios and conditions, including player interactions,
        /// ball movements, turn endings, and win/lose conditions. It manages the flow of gameplay by
        /// evaluating the current state of the game, such as whether the turn has started,
        /// whether the player has completed their turn, and what actions need to be taken based 
        /// on the player's input and the state of the game board.
        /// Additionally, it updates relevant UI elements to reflect changes in the game state, such 
        /// as displaying the current turn number and updating the stick power bar.
        /// This method plays a crucial role in maintaining the game's logic and ensuring a smooth and coherent
        /// player experience throughout the gameplay session.
        /// </summary>
        private void OnTurn()
        {
            // Check if the turn has started
            if (GameManager.doesTurnStarted)
            {
                // Lock the white ball movement during the turn
                WhiteBall.lockMovement = true;

                // Remove lines indicating stick angle
                _gameManager._scene.RemoveLines();

                // Check if the turn has ended
                if (_gameManager.IsTurnEnded())
                {
                    // Update player ball count after the turn
                    foreach (string ball in GameManager.playerBallsList)
                    {
                        if (!ballList.Contains(ball))
                        {
                            if (ball.Contains("Solid"))
                                GameManager.enteredSolidNum++;
                            else
                                GameManager.enteredStripedNum++;

                            ballList.Add(ball);
                        }
                    }

                    UpdateBallsBar(); // Update the balls bar

                    // Check if the current player has won or lost
                    if (GameManager.currentPlayerBallsStyle != "None")
                    {
                        // Handle the case when the player pushes the black ball
                        if (GameManager.enteredBlack)
                        {
                            int currentPlayer = 1;
                            int opponentPlayer = 2;

                            if (GameManager.turnNum % 2 == 0)
                            {
                                currentPlayer = 2;
                                opponentPlayer = 1;
                            }

                            //if player entered all of his balls and in the next turn he enters the black he wins, if he didn't, or if he also entered white, he lost.
                            if (((GameManager.currentPlayerBallsStyle == "SolidBall" && GameManager.enteredSolidNum == 7 && !GameManager.enteredSolid) || (GameManager.currentPlayerBallsStyle == "StripedBall" && GameManager.enteredStripedNum == 7 && !GameManager.enteredStriped)) && !GameManager.enteredWhite)
                                WinMessage.Text = $"Player {currentPlayer} Wins!";
                            else
                                WinMessage.Text = $"Player {opponentPlayer} Wins!";

                            GameOverPop.Visibility = Visibility.Visible;
                            UpdateBallsBar();
                            _gameManager.Pause();

                        }

                        // Handle the case when the player pushes the white ball into a hole
                        else if (GameManager.enteredWhite)
                        {
                            WhiteBall whiteBall = new WhiteBall(_gameManager._scene, "Balls/WhiteBall.png", 0, 37, 190, 220);
                            GameManager.balls[0] = whiteBall;
                            _gameManager._scene.AddObject(whiteBall);
                            GameManager.isMoveWhiteBallEnabled = true;  // Next player allowed to move the white ball on table.

                        }

                        // Handle the case when the current player touches the opponent's ball or black ball first with the white ball
                        else if (GameManager.currentPlayerBallsStyle != GameManager.firstCollideWith && !GameManager.currentTurnSetStyle)
                        {
                            GameManager.isMoveWhiteBallEnabled = true;  // Next player allowed to move the white ball on table.
                        }

                        // Handle the case when the current player enters at least one of their balls and gets another turn
                        else if ((GameManager.currentPlayerBallsStyle == "SolidBall" && GameManager.enteredSolid)
                        || (GameManager.currentPlayerBallsStyle == "StripedBall" && GameManager.enteredStriped))
                        {
                            if (GameManager.firstCollideWith != "BlackBall")
                            {
                                GameManager.turnNum--;
                                ChangeTurnBallStyle();
                            }
                            else
                                GameManager.isMoveWhiteBallEnabled = true; // Next player allowed to move the white ball on table.

                        }

                        // Check if the current turn's ball style is set
                        if (GameManager.currentTurnSetStyle) 
                            GameManager.currentTurnSetStyle = false; //after balls style are set, the game swaps turn and the ball style isnt anymore set on current turn, because we swaped turn;

                        GameManager.turnNum++;
                        ChangeTurnBallStyle();
                    }

                    // Handle the case when the black ball is pushed before all player balls are entered
                    else if (GameManager.enteredBlack)
                    {
                        int opponentPlayer = 2;

                        if (GameManager.turnNum % 2 == 0)
                        {
                            opponentPlayer = 1;
                        }

                        WinMessage.Text = $"Player {opponentPlayer} Wins!";
                        GameOverPop.Visibility = Visibility.Visible;
                        _gameManager.Pause();
                    }

                    // Handle the case when the white ball is pushed into a hole before the player enters any balls
                    else if (GameManager.enteredWhite)
                    {
                        GameManager.turnNum++;
                        WhiteBall whiteBall = new WhiteBall(_gameManager._scene, "Balls/WhiteBall.png", 0, 37, 190, 220);
                        GameManager.balls[0] = whiteBall;
                        _gameManager._scene.AddObject(whiteBall);
                        GameManager.isMoveWhiteBallEnabled = true;  // Next player allowed to move the white ball on table.
                    }

                    // Handle the case when the player touches the black ball or no ball at all, and the turn is swapped
                    else if ((!GameManager.enteredSolid && !GameManager.enteredStriped) || GameManager.firstCollideWith == "BlackBall" || GameManager.firstCollideWith == "None")
                    {
                        if(GameManager.firstCollideWith == "BlackBall" || GameManager.firstCollideWith == "None")
                            GameManager.isMoveWhiteBallEnabled = true;
                        GameManager.turnNum++;
                    }


                    // Set flag indicating the first turn is over
                    GameManager.firstTurnOver = true;

                    // Reset turn variables for the next turn
                    _gameManager.ResetTurnVariables();
                }
            }

            else // If the turn hasn't started yet or after a turn ends, update UI elements
            {
                UpdateTurnNum(); //Changes visually betweens the turns.
                UpdatePowerBar(); //Updates the stick power bar if player is changing it.
            }

        }

        /// <summary>
        /// Updates the visual settings for stick power bar.
        /// </summary>
        private void UpdatePowerBar()
        {
            if (GameManager.powerLevelNum == 1)
            {
                Power2.Opacity = 0.2;
                Power3.Opacity = 0.1;
                Power4.Opacity = 0.1;
                Power5.Opacity = 0.1;
                Power6.Opacity = 0.1;
                Power7.Opacity = 0.1;
                Power8.Opacity = 0.1;
                Power9.Opacity = 0.1;
                Power10.Opacity = 0.1;

                WhiteBall.powerSpeed = 2.5;
            }

            else if (GameManager.powerLevelNum == 2)
            {
                Power2.Opacity = 1;
                Power3.Opacity = 0.1;
                Power4.Opacity = 0.1;
                Power5.Opacity = 0.1;
                Power6.Opacity = 0.1;
                Power7.Opacity = 0.1;
                Power8.Opacity = 0.1;
                Power9.Opacity = 0.1;
                Power10.Opacity = 0.1;

                WhiteBall.powerSpeed = 5;
            }

            else if (GameManager.powerLevelNum == 3)
            {
                Power2.Opacity = 1;
                Power3.Opacity = 1;
                Power4.Opacity = 0.1;
                Power5.Opacity = 0.1;
                Power6.Opacity = 0.1;
                Power7.Opacity = 0.1;
                Power8.Opacity = 0.1;
                Power9.Opacity = 0.1;
                Power10.Opacity = 0.1;

                WhiteBall.powerSpeed = 7.5;
            }

            else if (GameManager.powerLevelNum == 4)
            {
                Power2.Opacity = 1;
                Power3.Opacity = 1;
                Power4.Opacity = 1;
                Power5.Opacity = 0.1;
                Power6.Opacity = 0.1;
                Power7.Opacity = 0.1;
                Power8.Opacity = 0.1;
                Power9.Opacity = 0.1;
                Power10.Opacity = 0.1;

                WhiteBall.powerSpeed = 10;
            }

            else if (GameManager.powerLevelNum == 5)
            {
                Power2.Opacity = 1;
                Power3.Opacity = 1;
                Power4.Opacity = 1;
                Power5.Opacity = 1;
                Power6.Opacity = 0.1;
                Power7.Opacity = 0.1;
                Power8.Opacity = 0.1;
                Power9.Opacity = 0.1;
                Power10.Opacity = 0.1;

                WhiteBall.powerSpeed = 12.5;
            }

            else if (GameManager.powerLevelNum == 6)
            {
                Power2.Opacity = 1;
                Power3.Opacity = 1;
                Power4.Opacity = 1;
                Power5.Opacity = 1;
                Power6.Opacity = 1;
                Power7.Opacity = 0.1;
                Power8.Opacity = 0.1;
                Power9.Opacity = 0.1;
                Power10.Opacity = 0.1;

                WhiteBall.powerSpeed = 15;
            }

            else if (GameManager.powerLevelNum == 7)
            {
                Power2.Opacity = 1;
                Power3.Opacity = 1;
                Power4.Opacity = 1;
                Power5.Opacity = 1;
                Power6.Opacity = 1;
                Power7.Opacity = 1;
                Power8.Opacity = 0.1;
                Power9.Opacity = 0.1;
                Power10.Opacity = 0.1;

                WhiteBall.powerSpeed = 17.5;
            }

            else if (GameManager.powerLevelNum == 8)
            {
                Power2.Opacity = 1;
                Power3.Opacity = 1;
                Power4.Opacity = 1;
                Power5.Opacity = 1;
                Power6.Opacity = 1;
                Power7.Opacity = 1;
                Power8.Opacity = 1;
                Power9.Opacity = 0.1;
                Power10.Opacity = 0.1;

                WhiteBall.powerSpeed = 20;
            }

            else if (GameManager.powerLevelNum == 9)
            {
                Power2.Opacity = 1;
                Power3.Opacity = 1;
                Power4.Opacity = 1;
                Power5.Opacity = 1;
                Power6.Opacity = 1;
                Power7.Opacity = 1;
                Power8.Opacity = 1;
                Power9.Opacity = 1;
                Power10.Opacity = 0.1;

                WhiteBall.powerSpeed = 22.5;
            }

            else if(GameManager.powerLevelNum == 10)
            {
                Power2.Opacity = 1;
                Power3.Opacity = 1;
                Power4.Opacity = 1;
                Power5.Opacity = 1;
                Power6.Opacity = 1;
                Power7.Opacity = 1;
                Power8.Opacity = 1;
                Power9.Opacity = 1;
                Power10.Opacity = 1;

                WhiteBall.powerSpeed = 25;
            }
        }

        /// <summary>
        /// Updates the visual settings for players turn.
        /// </summary>
        private void UpdateTurnNum()
        {
            if (GameManager.turnNum % 2 == 0)
            {
                Player1Grid.Opacity = 0.5;
                Player1Grid.BorderBrush = new SolidColorBrush(Colors.Black);

                Player2Grid.Opacity = 0.8;
                Player2Grid.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else
            {
                Player2Grid.Opacity = 0.5;
                Player2Grid.BorderBrush = new SolidColorBrush(Colors.Black);

                Player1Grid.Opacity = 0.8;
                Player1Grid.BorderBrush = new SolidColorBrush(Colors.White);
            }
        }

        /// <summary>
        /// Updates the visual for balls bar.
        /// </summary>
        private void UpdateBallsBar()
        {
            if (GameManager.currentTurnSetStyle)
                SetImagesForPlayersBars();
            
            foreach (string imgBall in ballList)
            {
                switch (imgBall)
                {
                    case "Balls/SolidBall1.png":
                        if (GameManager.player1BallStyle == "SolidBall")
                            imgBall1.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        else
                            imgBall8.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        break;

                    case "Balls/SolidBall2.png":
                        if (GameManager.player1BallStyle == "SolidBall")
                            imgBall2.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        else
                            imgBall9.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        break;

                    case "Balls/SolidBall3.png":
                        if (GameManager.player1BallStyle == "SolidBall")
                            imgBall3.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        else
                            imgBall10.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        break;

                    case "Balls/SolidBall4.png":
                        if (GameManager.player1BallStyle == "SolidBall")
                            imgBall4.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        else
                            imgBall11.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        break;

                    case "Balls/SolidBall5.png":
                        if (GameManager.player1BallStyle == "SolidBall")
                            imgBall5.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        else
                            imgBall12.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        break;

                    case "Balls/SolidBall6.png":
                        if (GameManager.player1BallStyle == "SolidBall")
                            imgBall6.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        else
                            imgBall13.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        break;

                    case "Balls/SolidBall7.png":
                        if (GameManager.player1BallStyle == "SolidBall")
                            imgBall7.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        else
                            imgBall14.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        break;

                    case "Balls/StripedBall1.png":
                        if (GameManager.player1BallStyle == "SolidBall")
                            imgBall8.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        else
                            imgBall1.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        break;

                    case "Balls/StripedBall2.png":
                        if (GameManager.player1BallStyle == "SolidBall")
                            imgBall9.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        else
                            imgBall2.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        break;

                    case "Balls/StripedBall3.png":
                        if (GameManager.player1BallStyle == "SolidBall")
                            imgBall10.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        else
                            imgBall3.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        break;

                    case "Balls/StripedBall4.png":
                        if (GameManager.player1BallStyle == "SolidBall")
                            imgBall11.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        else
                            imgBall4.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        break;

                    case "Balls/StripedBall5.png":
                        if (GameManager.player1BallStyle == "SolidBall")
                            imgBall12.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        else
                            imgBall5.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        break;

                    case "Balls/StripedBall6.png":
                        if (GameManager.player1BallStyle == "SolidBall")
                            imgBall13.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        else
                            imgBall6.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        break;

                    case "Balls/StripedBall7.png":
                        if (GameManager.player1BallStyle == "SolidBall")
                            imgBall14.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        else
                            imgBall7.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Balls/BallBlackBackground.png"));
                        break;

                }
            }
            
        }

        /// <summary>
        /// Sets the images for both players balls bar to their style,
        /// in the turn which the style is set for them.
        /// </summary>
        private void SetImagesForPlayersBars()
        {
            if ((GameManager.currentPlayerBallsStyle == "SolidBall" && GameManager.turnNum % 2 == 1) || (GameManager.currentPlayerBallsStyle == "StripedBall" && GameManager.turnNum % 2 == 0)) //if player 1 is SolidBall, and player 2 is StripedBall
            {

                GameManager.player1BallStyle = "SolidBall";

                imgBall1.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[2]._fileName}"));
                imgBall2.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[3]._fileName}"));
                imgBall3.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[4]._fileName}"));
                imgBall4.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[5]._fileName}"));
                imgBall5.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[6]._fileName}"));
                imgBall6.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[7]._fileName}"));
                imgBall7.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[8]._fileName}"));
                imgBall8.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[9]._fileName}"));
                imgBall9.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[10]._fileName}"));
                imgBall10.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[11]._fileName}"));
                imgBall11.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[12]._fileName}"));
                imgBall12.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[13]._fileName}"));
                imgBall13.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[14]._fileName}"));
                imgBall14.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[15]._fileName}"));
            }
            else //if player 1 is Striped Ball, and player 2 is Solid ball
            {
                GameManager.player1BallStyle = "StripedBall";

                imgBall1.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[9]._fileName}"));
                imgBall2.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[10]._fileName}"));
                imgBall3.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[11]._fileName}"));
                imgBall4.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[12]._fileName}"));
                imgBall5.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[13]._fileName}"));
                imgBall6.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[14]._fileName}"));
                imgBall7.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[15]._fileName}"));
                imgBall8.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[2]._fileName}"));
                imgBall9.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[3]._fileName}"));
                imgBall10.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[4]._fileName}"));
                imgBall11.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[5]._fileName}"));
                imgBall12.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[6]._fileName}"));
                imgBall13.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[7]._fileName}"));
                imgBall14.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{GameManager.balls[8]._fileName}"));
            }

        }


        /// <summary>
        /// Swaps the current player ball style each turn to the opposite one.
        /// </summary>
        private void ChangeTurnBallStyle()
        {
            if (GameManager.currentPlayerBallsStyle == "SolidBall")
                GameManager.currentPlayerBallsStyle = "StripedBall";
            else
                GameManager.currentPlayerBallsStyle = "SolidBall";
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// If game over grid is visible, changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgReturn_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (GameOverPop.Visibility != Visibility.Visible)
            {
                SoundPlayer.Play("PointerEnteredSFX.wav");
                imgReturn.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/_Return.png"));
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
            }
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// If game over grid is visible, changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgReturn_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (GameOverPop.Visibility != Visibility.Visible)
            {
                imgReturn.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Return.png"));
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
            }
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// If GameOverPop is visible, makes a sound, changing game state to pause, resets events and navigates to a MenuPage.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgReturn_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (GameOverPop.Visibility != Visibility.Visible)
            {
                SoundPlayer.Play("Click.wav");
                _gameManager.Pause();
                _gameManager.ResetEvents();
                Frame.Navigate(typeof(MenuPage));
            }
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// If game over grid is visible, changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgEndgame_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            imgEndgame.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/_Return.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgEndgame_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            imgEndgame.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Return.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes a sound, adds user money, updates data in database, changing game state to pause, resets events and navigates to a MenuPage.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgEndgame_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.wav");
            GameManager.User.Money += 100;
            GameManager.UpdateData();
            _gameManager.Pause();
            _gameManager.ResetEvents();
            Frame.Navigate(typeof(MenuPage));
        }
    }
}
