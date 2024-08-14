namespace GameEngine.GameObjects
{
    /// <summary>
    /// Represents a point in a two-dimensional space.
    /// </summary>
    public class GamePoint
    {
        public double X { get; set; } // Gets or sets the X-coordinate of the point.

        public double Y { get; set; } // Gets or sets the Y-coordinate of the point.

        /// <summary>
        /// Initializes a new instance of the GamePoint class with the specified coordinates.
        /// </summary>
        /// <param name="X">The X-coordinate of the point.</param>
        /// <param name="Y">The Y-coordinate of the point.</param>
        public GamePoint(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
