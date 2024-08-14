using Billiard.GameObjects;
using GameEngine.GameObjects;
using GameEngine.GameServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Billiard.GameServices
{
    /// <summary>
    /// Represents the scene of the billiard game, handling user interactions such as pointer movements and clicks.
    /// </summary>
    public class GameScene : Scene
    {
        public static bool WhiteBallPressed; // Indicates whether the white ball has been pressed by the user.

        private GamePoint LeftButtonPressedPos;

        /// <summary>
        /// Gets the total count of balls in the scene.
        /// </summary>
        public int BallCount => _gameObjectsSnapshot.Count(x => x is Ball);

        /// <summary>
        /// Gets the count of solid balls in the scene.
        /// </summary>
        public int SolidBallCount => _gameObjectsSnapshot.Count(x => x is SolidBall);

        /// <summary>
        /// Gets the count of striped balls in the scene.
        /// </summary>
        public int StripedBallCount => _gameObjectsSnapshot.Count(x => x is StripedBall);

        /// <summary>
        /// Initializes a new instance of the GameScene class.
        /// </summary>
        public GameScene() : base()
        {
            LeftButtonPressedPos = new GamePoint(0, 0);
            WhiteBallPressed = false;

            PointerWheelChanged += Scene_PointerWheelChanged;
            PointerPressed += Scene_PointerPressed;
        }

        /// <summary>
        /// Event handler for when the mouse wheel is changed within the scene.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">PointerRoutedEventArgs containing event data.</param>
        private void Scene_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            var wheelDelta = e.GetCurrentPoint((UIElement)sender).Properties.MouseWheelDelta;

            if (wheelDelta > 0) //Scrolled up
            {
                if (GameManager.powerLevelNum < 10)
                    GameManager.powerLevelNum++;
            }
            else //Scrolled down
            {
                if (GameManager.powerLevelNum > 1)
                    GameManager.powerLevelNum--;
            }
        }


        /// <summary>
        /// Event handler for when the pointer is pressed within the scene.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">PointerRoutedEventArgs containing event data.</param>
        public void Scene_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                var p = e.GetCurrentPoint((UIElement)sender);
                if (p.Properties.IsLeftButtonPressed)
                {
                    //Creates a GamePoint instance for mousePosition (X,Y)
                    GamePoint mousePosition = new GamePoint(e.GetCurrentPoint(this).Position.X,
                            e.GetCurrentPoint(this).Position.Y);
                    LeftButtonPressedPos = mousePosition;
                    if (GameManager.isMoveWhiteBallEnabled)
                    {
                        foreach (var ball in _gameObjectsSnapshot)
                        {
                            if (ball is WhiteBall)
                            {
                                if (WhiteBallPressed || (mousePosition.X > ball._X && mousePosition.X < ball._X + ball.Width && mousePosition.Y > ball._Y && mousePosition.Y < ball._Y + ball.Width))
                                {
                                    RemoveLines(); // Remove all lines when the player moves the ball location.

                                    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
                                    if (WhiteBallPressed)
                                    {
                                        if (ValidPos(mousePosition.X, mousePosition.Y))
                                        {
                                            ball.Collisional = false;
                                            ball._X = mousePosition.X - (0.5 * ball.Width);
                                            ball._Y = mousePosition.Y - (0.5 * ball.Width);
                                        }
                                    }
                                    else
                                        WhiteBallPressed = true;
                                }

                                else // If the white ball is allowed to move but the player didn't choose the option to move it, then set the stick angle line.
                                {
                                    GameManager.isStickAngleSet = true;
                                    AddStick(mousePosition.X, mousePosition.Y);
                                }
                            }
                        }
                    }
                    else if (!GameManager.doesTurnStarted) // Draw the stick angle, the angle of the ball movement.
                    {
                        GameManager.isStickAngleSet = true;
                        AddStick(mousePosition.X, mousePosition.Y);
                    }
                }

                else if (p.Properties.IsRightButtonPressed) // If the player presses the right click, they start the ball movement, but the default key to move the ball is the Space bar key.
                {
                    foreach (var ball in _gameObjectsSnapshot)
                    {
                        if (ball is WhiteBall b)
                        {
                            b.KeyDown(Windows.System.VirtualKey.Space);
                        }
                    }
                }

            }
        }


        /// <summary>
        /// Adds a stick line to represent the angle of the ball movement.
        /// </summary>
        /// <param name="mousePosX">The X-coordinate of the mouse position.</param>
        /// <param name="mousePosY">The Y-coordinate of the mouse position.</param>
        public void AddStick(double mousePosX, double mousePosY)
        {
            foreach (var ball in _gameObjectsSnapshot)
            {
                if (ball is WhiteBall)
                {
                    //creates a new instance of a line from the white ball to the mouse location
                    Line line = new Line();
                    line.X1 = mousePosX;
                    line.Y1 = mousePosY;

                    line.X2 = ball._X + (0.5 * ball.Width);
                    line.Y2 = ball._Y + (0.5 * ball.Width);

                    line.Stroke = new SolidColorBrush(Colors.White);
                    line.StrokeThickness = 3;

                    // Remove any previous stick line
                    foreach (var obj in Children)
                    {
                        if (obj is Line line_to_remove)
                        {
                            this.Children.Remove(line_to_remove);
                            break;
                        }
                    }

                    this.Children.Add(line); // Add the new stick line

                    // Calculate the stick angle
                    GameManager.stickAngle = Math.Atan2(line.Y1 - line.Y2, line.X1 - line.X2) * (180 / Math.PI);
                }
            }
        }


        /// <summary>
        /// Removes the stick angle line from the scene.
        /// </summary>
        public void RemoveLines()
        {
            // If the stick angle line is removed, set the flag to false
            GameManager.isStickAngleSet = false;

            foreach (var obj in Children)
            {
                if (obj is Line line_to_remove)
                {
                    this.Children.Remove(line_to_remove); // Remove the previous line
                    break;
                }
            }
        }

        /// <summary>
        /// Checks if the specified position is valid for moving the white ball.
        /// </summary>
        /// <param name="x">The X-coordinate of the position.</param>
        /// <param name="y">The Y-coordinate of the position.</param>
        /// <returns>True if the position is valid; otherwise, false.</returns>
        private bool ValidPos(double x, double y)
        {
            foreach (var ball in _gameObjectsSnapshot)
            {
                if (x > ball._X && x < ball._X + ball.Width && y > ball._Y && y < ball._Y + ball.Width)
                {
                    if (ball is WhiteBall)
                    {
                        WhiteBallPressed = false;
                        // Reset the pointer cursor
                        Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
                    }
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the position of the pointer when left button pressed.
        /// </summary>
        /// <returns>The position of the pointer.</returns>
        public GamePoint GetPointerPos()
        {
            return LeftButtonPressedPos;
        }

    }
}
