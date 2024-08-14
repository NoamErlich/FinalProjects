using Billiard.GameServices;
using GameEngine.GameObjects;
using GameEngine.GameServices;
using System;
using Windows.System;
using Windows.UI.Xaml;

namespace Billiard.GameObjects
{
    /// <summary>
    /// Represents the white ball in the billiard game.
    /// </summary>
    public class WhiteBall : Ball
    {
        public static bool lockMovement; // Indicates whether the movement of the white ball is locked.

        public static double powerSpeed; // The power speed of the white ball.

        /// <summary>
        /// Initializes a new instance of the <see cref="WhiteBall"/> class.
        /// </summary>
        /// <param name="scene">The scene where the white ball exists.</param>
        /// <param name="fileName">The file name of the image representing the white ball.</param>
        /// <param name="speed">The initial speed of the white ball.</param>
        /// <param name="width">The width of the white ball.</param>
        /// <param name="placeX">The initial X coordinate of the white ball.</param>
        /// <param name="placeY">The initial Y coordinate of the white ball.</param>
        public WhiteBall(Scene scene, string fileName, double speed, double width, double placeX, double placeY)
            : base(scene, fileName, speed, width, placeX, placeY)
        {
            powerSpeed = 2.5;
            lockMovement = false;

            Manager.GameEvent.OnKeyDown += KeyDown;
        }

        /// <summary>
        /// Resets variables and settings related to the white ball and to game turn.
        /// </summary>
        private void ResetVariablesAndSettings()
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
            GameManager.isMoveWhiteBallEnabled = false;
            GameManager.doesTurnStarted = true;
            GameScene.WhiteBallPressed = false;
            Collisional = true;
        }

        /// <summary>
        /// Handles the key down event for controlling the white ball's movement.
        /// </summary>
        /// <param name="key">The virtual key that is pressed.</param>
        public void KeyDown(VirtualKey key)
        {
            if (!lockMovement && GameManager.isStickAngleSet)
            {
                double speedx = Math.Cos(GameManager.stickAngle * Math.PI / 180) * powerSpeed;
                double speedy = Math.Sin(GameManager.stickAngle * Math.PI / 180) * powerSpeed;

                switch (key)
                {
                    case VirtualKey.Space:
                        _dY = -speedy;
                        _dX = -speedx;
                        ResetVariablesAndSettings();
                        break;
                }
            }
        }
    }
}
