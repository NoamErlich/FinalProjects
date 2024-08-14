using Billiard.GameObjects;
using GameEngine.GameServices;

/// <summary>
/// Represents a black ball on the table in a billiard game.
/// </summary>
public class BlackBall : Ball
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BlackBall"/> class.
    /// </summary>
    /// <param name="scene">The scene where the black ball exists.</param>
    /// <param name="fileName">The file name of the image representing the black ball.</param>
    /// <param name="speed">The speed of the black ball.</param>
    /// <param name="width">The width of the black ball.</param>
    /// <param name="placeX">The initial X coordinate of the black ball.</param>
    /// <param name="placeY">The initial Y coordinate of the black ball.</param>
    public BlackBall(Scene scene, string fileName, double speed, double width, double placeX, double placeY)
        : base(scene, fileName, speed, width, placeX, placeY)
    {
    }
}
