using GameEngine.GameObjects;
using GameEngine.GameServices;

namespace Billiard.GameObjects
{
    /// <summary>
    /// Represents a hole on the table in a billiard game.
    /// </summary>
    public class Hole : GameObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hole"/> class.
        /// </summary>
        /// <param name="scene">The scene where the hole exists.</param>
        /// <param name="fileName">The file name of the image representing the hole.</param>
        /// <param name="width">The width of the hole.</param>
        /// <param name="height">The height of the hole.</param>
        /// <param name="placeX">The initial X coordinate of the hole.</param>
        /// <param name="placeY">The initial Y coordinate of the hole.</param>
        public Hole(Scene scene, string fileName, double width, double height, double placeX, double placeY)
            : base(scene, fileName, width, height, placeX, placeY)
        {
        }
    }
}
