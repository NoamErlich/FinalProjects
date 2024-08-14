using GameEngine.GameObjects;
using GameEngine.GameServices;

namespace Billiard.GameObjects
{
    /// <summary>
    /// Represents a striped ball in a billiard game.
    /// </summary>
    public class StripedBall : Ball
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StripedBall"/> class.
        /// </summary>
        /// <param name="scene">The scene where the striped ball exists.</param>
        /// <param name="fileName">The file name of the image representing the striped ball.</param>
        /// <param name="speed">The initial speed of the striped ball.</param>
        /// <param name="width">The width of the striped ball.</param>
        /// <param name="placeX">The initial X coordinate of the striped ball.</param>
        /// <param name="placeY">The initial Y coordinate of the striped ball.</param>
        public StripedBall(Scene scene, string fileName, double speed, double width, double placeX, double placeY)
            : base(scene, fileName, speed, width, placeX, placeY)
        {
        }
    }
}
