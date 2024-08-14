using GameEngine.GameServices;
using GameEngine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml;
using Billiard.GameServices;
using System.IO;

namespace Billiard.GameObjects
{
    /// <summary>
    /// Represents a ball object in the billiard game.
    /// </summary>
    public class Ball : RoundGameObject
    {
        protected double _speed; //The initial speed of the ball

        /// <summary>
        /// Initializes a new instance of the <see cref="Ball"/> class.
        /// </summary>
        /// <param name="scene">The scene where the ball exists.</param>
        /// <param name="fileName">The file name of the image representing the ball.</param>
        /// <param name="speed">The initial speed of the ball.</param>
        /// <param name="width">The width of the ball.</param>
        /// <param name="placeX">The initial X coordinate of the ball.</param>
        /// <param name="placeY">The initial Y coordinate of the ball.</param>
        public Ball(Scene scene, string fileName, double speed, double width, double placeX, double placeY) :
            base(scene, fileName, 0.5 * width, placeX, placeY)
        {
            _dX = 0;
            _dY = 0;
            Image.Width = width;
            _speed = speed;
        }

        /// <summary>
        /// Checks if the ball is currently moving.
        /// </summary>
        /// <returns>True if the ball is moving, otherwise false.</returns>
        public bool IsBallMoving()
        {
            return _dX != 0 || _dY != 0 || _ddX != 0 || _ddY != 0; 
        }

        /// <summary>
        /// Stops the movement of the ball.
        /// </summary>
        private void StopBall()
        {
            _dX = 0;
            _dY = 0;
            _ddX = 0;
            _ddY = 0;
        }

        /// <summary>
        /// Renders the ball's movement and handles collisions with the scene boundaries.
        /// </summary>
        public override void Render()
        {
            if (_X <= 0) //left wall collision
            {
                _dX = -_dX;
                _ddX = -_ddX;
                _X = 0;
                //SoundPlayer.Play("BallHitTableSFX.wav");
            }
            else if (_X >= _scene?.ActualWidth - Width) //right wall collision
            {
                _dX = -_dX;
                _ddX = -_ddX;
                _X = _scene.ActualWidth - Width;
               //SoundPlayer.Play("BallHitTableSFX.wav");
            }
            else if (_Y <= 0) //ceiling collision
            {
                _dY = -_dY;
                _ddY = -_ddY;
                _Y = 0;
                //SoundPlayer.Play("BallHitTableSFX.wav");
            }
            else if (_Y >= _scene?.ActualHeight - Height) //floor collision
            {
                _dY = -_dY;
                _ddY = -_ddY;
                _Y = _scene.ActualHeight - Height;
                //SoundPlayer.Play("BallHitTableSFX.wav");
            }

            if (Math.Round(_dX) != 0) //Friction X
            {
                if (_dX > 0)
                    _ddX = -0.0675;
                else
                    _ddX = 0.0675;
            }
            else //When ball X speed is almost zero, stop ball X speed and X acceleration and set it to zero
            {
                _ddX = 0;
                _dX = 0;
            }

            if (Math.Round(_dY) != 0) //Friction Y
            {
                if (_dY > 0)
                    _ddY = -0.0675;
                else
                    _ddY = 0.0675;
            }
            else //When ball Y speed is almost zero, stop ball Y speed and Y acceleration and set it to zero
            {
                _ddY = 0;
                _dY = 0;
            }

            base.Render();

        }

        /// <summary>
        /// Handles collisions between the ball and other game objects, such as other balls and holes.
        /// If the ball collides with another ball, it adjusts the speeds of both balls and fixes their positions to prevent overlap.
        /// If the ball collides with a hole, it updates game state variables accordingly, removes the ball from the scene, and stops its movement.
        /// </summary>
        /// <param name="gameObject">The game object the ball collides with.</param>
        public override void Collide(GameObject gameObject)
        {
            if (gameObject is Ball ball)
            {
                // Sets the firstCollideWith Turn Variable to the first ball style that the white ball touched.
                if (ball is SolidBall)
                {
                    if (GameManager.firstCollideWith == "None")
                        GameManager.firstCollideWith = "SolidBall";
                }

                if (ball is StripedBall)
                {
                    if (GameManager.firstCollideWith == "None")
                        GameManager.firstCollideWith = "StripedBall";
                }

                if (ball is BlackBall)
                {
                    if (GameManager.firstCollideWith == "None")
                        GameManager.firstCollideWith = "BlackBall";
                }

                // Swaps speeds when colliding
                double tempSpeedX;
                double tempSpeedY;

                double lenX = Width;
                double lenY = Height;

                tempSpeedX = _dX;
                if (_dX == 0)
                    _dX = ball._dX;
                else
                {
                    _dX = _dX / 1.5;
                    ball._dX = tempSpeedX;
                }

                tempSpeedY = _dY;
                if (_dY == 0)
                    _dY = ball._dY;
                else
                {
                    _dY = _dY / 1.5;
                    ball._dY = tempSpeedY;
                }

                // Fixes collisions
                if (_X > ball._X)
                {
                    _X += 0.1 * lenX;
                    ball._X -= 0.1 * lenX;
                }
                else
                {
                    _X -= 0.1 * lenX;
                    ball._X += 0.1 * lenX;
                }

                if (_Y > ball._Y)
                {
                    _Y += 0.1 * lenY;
                    ball._Y -= 0.1 * lenY;
                }
                else
                {
                    _Y -= 0.1 * lenY;
                    ball._Y += 0.1 * lenY;
                }
            }

            if (gameObject is Hole hole)
            {
                string currentBallStyle = GameManager.currentPlayerBallsStyle;
                string fileName = this._fileName;
                List<string> ballList = GameManager.playerBallsList;

                if (_X <= (hole._X + hole.Width) && _X >= hole._X && _Y <= (hole._Y + hole.Height) && _Y >= hole._Y)
                {
                    // Adds the ball's file name to the list of balls in the hole
                    if (!ballList.Contains(fileName) && fileName != "Balls/WhiteBall.png" && fileName != "Balls/8BlackBall.png")
                    {
                        ballList.Add(fileName);
                    }

                    // Stops the ball and removes it from the scene
                    StopBall();
                    _scene.RemoveObject(this);

                    if (this is WhiteBall)
                    {
                        GameManager.enteredWhite = true;
                    }
                    else if (this is BlackBall)
                    {
                        GameManager.enteredBlack = true;
                    }

                    // Sets the ball style for current player to be SolidBall
                    if (this is SolidBall)
                    {
                        GameManager.enteredSolid = true;
                        if (currentBallStyle == "None" && GameManager.firstTurnOver)
                        {
                            GameManager.currentTurnSetStyle = true;
                            GameManager.currentPlayerBallsStyle = "SolidBall";
                        }
                    }

                    // Sets the ball style for current player to be StripedBall
                    if (this is StripedBall)
                    {
                        GameManager.enteredStriped = true;
                        if (currentBallStyle == "None" && GameManager.firstTurnOver)
                        {
                            GameManager.currentTurnSetStyle = true;
                            GameManager.currentPlayerBallsStyle = "StripedBall";
                        }
                    }
                }
            }
        }
    }
}
