using GameEngine.GameServices;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace GameEngine.GameObjects
{
    /// <summary>
    /// Represents a round game object in the game.
    /// </summary>
    public class RoundGameObject : GameMovingObject
    {
        public double _Radius { get; set; } // Gets or sets the radius of the round game object.
        public double _CenterX => _X + _Radius; // Gets the X-coordinate of the center of the round game object.
        public double _CenterY => _Y + _Radius; // Gets the Y-coordinate of the center of the round game object.

        /// <summary>
        /// Initializes a new instance of the RoundGameObject class with the specified parameters.
        /// </summary>
        /// <param name="scene">The scene where the game object exists.</param>
        /// <param name="fileName">The file name of the image representing the game object.</param>
        /// <param name="radius">The radius of the round game object.</param>
        /// <param name="_X">The X-coordinate of the round game object.</param>
        /// <param name="_Y">The Y-coordinate of the round game object.</param>
        public RoundGameObject(Scene scene, string fileName, double radius, double _X, double _Y)
            : base(scene, fileName, _X, _Y)
        {
            _Radius = radius;
        }

        /// <summary>
        /// Creates and returns an Ellipse object representing the round game object.
        /// </summary>
        /// <returns>An Ellipse object representing the round game object.</returns>
        public Ellipse RoundObject()
        {
            // Create a new Ellipse object
            Ellipse ellipse = new Ellipse();

            // Set the width and height of the ellipse based on the radius
            ellipse.Width = _Radius * 2;
            ellipse.Height = _Radius * 2;

            // Set the fill and stroke properties of the ellipse
            ellipse.Fill = new SolidColorBrush(Colors.Transparent);
            ellipse.Stroke = new SolidColorBrush(Colors.Transparent);
            ellipse.StrokeThickness = 2;

            return ellipse;
        }

        /// <summary>
        /// Renders the round game object on the screen by setting its position.
        /// </summary>
        public override void Render()
        {
            base.Render(); // Call base class's Render method to set position

            // Set the position of the round game object
            Canvas.SetLeft(Image, _CenterX - _Radius);
            Canvas.SetTop(Image, _CenterY - _Radius);
        }
    }
}
