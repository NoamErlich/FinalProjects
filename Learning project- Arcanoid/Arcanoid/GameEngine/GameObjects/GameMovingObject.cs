using GameEngine.GameServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO.Ports;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace GameEngine.GameObjects
{
    /// <summary>
    /// Represents a base class for all moving objects in the game.
    /// </summary>
    public class GameMovingObject : GameObject
    {
        protected double _dX;  // Horizontal velocity
        protected double _dY;  // Vertical velocity
        protected double _ddX; // Horizontal acceleration
        protected double _ddY; // Vertical acceleration
        protected double _toX; // Target position on the horizontal axis
        protected double _toY; // Target position on the vertical axis


        /// <summary>
        /// Initializes a new instance of the GameMovingObject class with the specified parameters.
        /// </summary>
        /// <param name="scene">The scene where the object will be rendered.</param>
        /// <param name="fileName">The file name of the object's image.</param>
        /// <param name="placeX">The initial horizontal position of the object.</param>
        /// <param name="placeY">The initial vertical position of the object.</param>
        /// <param name="maxSpeed">The maximum speed of the object. Default is 4.</param>
        public GameMovingObject(Scene scene, string fileName, double placeX, double placeY, double maxSpeed = 4) : base(scene, fileName, placeX, placeY)
        {
        }

        /// <summary>
        /// Updates the position of the object and renders it on the scene.
        /// Checks all sides to make sure object is in border and not out of it.
        /// </summary>
        public override void Render()
        {
            _dX += _ddX;
            _dY += _ddY;
            _X += _dX;
            _Y += _dY;
            
            if (_X <=0) //left side
                _X = 0;

            else if (_X >= _scene?.ActualWidth - Width) //right side
                _X = _scene.ActualWidth - Width;

            if (_Y <= 0) //up side
                _Y = 0;

            else if (_Y >= _scene?.ActualHeight - Height) //bottom side
                _Y = _scene.ActualHeight - Height;

            if (Math.Abs(_X - _toX) < 10 && Math.Abs(_Y - _toY) < 10)
            {
                Stop();
                _X = _toX;
                _Y = _toY;
            }

            base.Render();
        }

        /// <summary>
        /// Stops the movement of the object.
        /// </summary>
        public virtual void Stop()
        {
            _dX = _dY = _ddX = 0;
        }

        /// <summary>
        /// Moves the round game object to the specified coordinates with the given speed and acceleration.
        /// </summary>
        /// <param name="toX">The X-coordinate of the destination position.</param>
        /// <param name="toY">The Y-coordinate of the destination position.</param>
        /// <param name="speed">The speed of movement.</param>
        /// <param name="acceleration">The acceleration of movement.</param>
        public void MoveTo(double toX, double toY, double speed = 1, double acceleration = 0)
        {
            _toX = toX;
            _toY = toY;

            // Calculate the distance between the current position and the destination position
            var len = Math.Sqrt(Math.Pow(_toX - _X, 2) + Math.Pow(_toY - _Y, 2));

            // Calculate the cosine and sine values for the direction of movement
            var cos = (_toX - _X) / len;
            var sin = (_toY - _Y) / len;

            // Adjust speed based on a constant factor
            speed *= Constants.SpeedUnit;

            // Calculate the horizontal and vertical components of velocity
            _dX = speed * cos;
            _dY = speed * sin;

            // Calculate the horizontal and vertical components of acceleration
            _ddX = acceleration * cos;
            _ddY = acceleration * sin;
        }


    }
}
