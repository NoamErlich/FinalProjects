using GameEngine.GameServices;
using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace GameEngine.GameObjects
{
    /// <summary>
    /// Represents a basic game object with position, appearance, collision, and rendering properties.
    /// </summary>
    public abstract class GameObject
    {
        public double _X; // Current horizontal position
        public double _Y; // Current vertical position

        protected double _placeX; // Initial horizontal position
        protected double _placeY; // Initial vertical position

        public Image Image { get; set; } // Appearance of the object
        public string _fileName; // File name of the image

        public double Width => Image.ActualWidth; // Shortcut property for width
        public double Height => Image.ActualHeight; // Shortcut property for height
        public virtual Rect Rect => new Rect(_X, _Y, Width, Height); // Rectangle surrounding the object
        public virtual Ellipse Ellipse => new Ellipse(); // Ellipse surrounding the object

        public bool Collisional { get; set; } = true; // Indicates if the object is collisional

        protected Scene _scene; // The game scene

        /// <summary>
        /// Constructs a basic game object.
        /// </summary>
        /// <param name="scene">The game scene.</param>
        /// <param name="fileName">The file name of the image.</param>
        /// <param name="width">The width of the object.</param>
        /// <param name="height">The height of the object.</param>
        /// <param name="placeX">The initial horizontal position.</param>
        /// <param name="placeY">The initial vertical position.</param>
        public GameObject(Scene scene, string fileName, double width, double height, double placeX, double placeY)
        {
            _scene = scene;
            _fileName = fileName;
            _X = placeX;
            _Y = placeY;
            _placeX = placeX;
            _placeY = placeY;
            Image = new Image();
            Image.Width = width;
            Image.Height = height;

            SetImage(fileName);
            Render();
        }

        /// <summary>
        /// Constructs a basic game object.
        /// </summary>
        /// <param name="scene">The game scene.</param>
        /// <param name="fileName">The file name of the image.</param>
        /// <param name="placeX">The initial horizontal position.</param>
        /// <param name="placeY">The initial vertical position.</param>
        public GameObject(Scene scene, string fileName, double placeX, double placeY)
        {
            _scene = scene;
            _fileName = fileName;
            _X = placeX;
            _Y = placeY;
            _placeX = placeX;
            _placeY = placeY;
            Image = new Image();

            SetImage(fileName);
            Render();
        }

        /// <summary>
        /// Resets the object to its initial position.
        /// </summary>
        public virtual void InIt()
        {
            _X = _placeX;
            _Y = _placeY;
        }

        /// <summary>
        /// Defines the response to a collision with another game object.
        /// </summary>
        /// <param name="gameObject">The game object with which this object collides.</param>
        public virtual void Collide(GameObject gameObject)
        {

        }

        /// <summary>
        /// Renders the object on the screen.
        /// </summary>
        public virtual void Render()
        {
            Canvas.SetLeft(Image, _X);
            Canvas.SetTop(Image, _Y);
        }

        /// <summary>
        /// Sets the Image source to the image that is in filename.
        /// </summary>
        /// <param name="fileName">File name of the image</param>
        protected void SetImage(string fileName)
        {
            Image.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{fileName}"));
        }
    }
}
