using GameEngine.GameObjects;
using GameEngine.GameServices;

namespace Billiard.GameObjects
{
    /// <summary>
    /// Represents a solid ball in a billiard game.
    /// </summary>
    public class SolidBall : Ball
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolidBall"/> class.
        /// </summary>
        /// <param name="scene">The scene where the solid ball exists.</param>
        /// <param name="fileName">The file name of the image representing the solid ball.</param>
        /// <param name="speed">The initial speed of the solid ball.</param>
        /// <param name="width">The width of the solid ball.</param>
        /// <param name="placeX">The initial X coordinate of the solid ball.</param>
        /// <param name="placeY">The initial Y coordinate of the solid ball.</param>
        public SolidBall(Scene scene, string fileName, double speed, double width, double placeX, double placeY)
            : base(scene, fileName, speed, width, placeX, placeY)
        {
        }
    }
}
