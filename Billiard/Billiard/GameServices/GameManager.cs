using Billiard.GameObjects;
using DatabaseProject;
using DatabaseProject.Models;
using GameEngine.GameObjects;
using GameEngine.GameServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.WebUI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace Billiard.GameServices
{
    /// <summary>
    /// Manages the game state and operations.
    /// </summary>
    public class GameManager : Manager
    {
        public static GameUser User { get; set; } = new GameUser(); // Gets or sets the current user of the game.

        // Game variables:
        public static string currentPlayerBallsStyle = "None"; // Player Ball style, gets chosen after a
                                                               // player enters balls to one of the holes after turn
                                                               // number 2 included. before that it set to None.

        public static string player1BallStyle = "None"; // Player1 ball style
        public static int turnNum = 1; // Turn number | 1,3,5... -> player1 | 2,4,6... -> player 2 |
        public static bool firstTurnOver = false; // If first turn is over
        public static bool currentTurnSetStyle = false; // What is the ball style at current turn
        public static int enteredSolidNum = 0; // Number of solid balls entered the holes
        public static int enteredStripedNum = 0; // Number of striped balls entered the holes

        // Turn variables:
        public static bool doesTurnStarted = false; // If turn started
        public static bool isMoveWhiteBallEnabled = true; // If white ball can be moved by the current player
        public static string firstCollideWith = "None"; // The name of the ball style the white ball first collided with
        public static bool enteredSolid = false; // If player entered solid balls at his turn.
        public static bool enteredStriped = false; // If player entered striped balls at his turn.
        public static bool enteredWhite = false; // If player entered the white ball at his turn.
        public static bool enteredBlack = false; // If player entered black ball at his turn.
        public static double stickAngle = 180; // Sets the stick angle to 180 at start
        public static int powerLevelNum = 1; // Sets the power level of the power of the blow on the white ball with the stick.
        public static bool isStickAngleSet = false; //If player set an angle for the stick(cue) to hit the white ball.

        // Game objects and related lists:
        public static Ball[] balls; // All balls on table list
        public static SolidBall[] solidBalls; // All solid balls on table list
        public static StripedBall[] stripedBalls; // All striped balls on table list
        public static List<string> playerBallsList; // All balls file names

        public GameScene _scene; // Reference to the game scene

        /// <summary>
        /// Initializes a new instance of the <see cref="GameManager"/> class.
        /// </summary>
        /// <param name="scene">The game scene associated with this manager.</param>
        public GameManager(GameScene scene) : base(scene)
        {
            // Initialize game variables and other components

            // Resets class variables:
            currentPlayerBallsStyle = "None";
            turnNum = 1;
            firstTurnOver = false;
            currentTurnSetStyle = false;
            enteredSolidNum = 0;
            enteredStripedNum = 0;
            isMoveWhiteBallEnabled = true;

            // Resets turn variables:
            doesTurnStarted = false;
            firstCollideWith = "None";
            enteredSolid = false;
            enteredStriped = false;
            enteredWhite = false;
            enteredBlack = false;
            stickAngle = 180;
            powerLevelNum = 1;
            isStickAngleSet = false;

            // Resets balls lists:
            balls = new Ball[16];
            solidBalls = new SolidBall[7];
            stripedBalls = new StripedBall[7];
            playerBallsList = new List<string>();


            _scene = scene; //sets current scene

            _scene.Ground = _scene.ActualHeight - 40; //sets the bottom border height
            InIt();
        }

        /// <summary>
        /// Checks if the current turn has ended.
        /// (Check if any ball is still moving)
        /// </summary>
        /// <returns>True if the turn has ended; otherwise, false.</returns>
        public bool IsTurnEnded()
        {
            foreach (var ball in balls) 
                if (ball.IsBallMoving())
                    return false;
            return true;
        }

        /// <summary>
        /// Resets all turn-related variables.
        /// </summary>
        public void ResetTurnVariables()
        {
            doesTurnStarted = false;
            firstCollideWith = "None";
            enteredSolid = false;
            enteredStriped = false;
            enteredWhite = false;
            enteredBlack = false;
            stickAngle = 180;
            powerLevelNum = 1;
            isStickAngleSet = false;
            WhiteBall.lockMovement = false;
        }

        /// <summary>
        /// Resets all events.
        /// </summary>
        public void ResetEvents()
        {
            GameEvent.RemoveAllActions();
        }

        /// <summary>
        /// Initializes the game scene by creating balls and holes.
        /// </summary>
        private void InIt()
        {
            Scene.RemoveAllObject();
            CreateBallField();
            CreateHoles();
        }

        /// <summary>
        /// Updates game data.
        /// </summary>
        public static void UpdateData()
        {
            Server.UpdateDataInExit(User);
        }

        /// <summary>
        /// Adding all holes to the scene(table).
        /// </summary>
        private void CreateHoles()
        {
            Hole[] holes = new Hole[6];
            holes[0] = new Hole(_scene, "NONE", 20, 20, 0, 0);
            holes[1] = new Hole(_scene, "NONE", 20, 20, 450, 0);
            holes[2] = new Hole(_scene, "NONE", 20, 20, 915, 0);
            holes[3] = new Hole(_scene, "NONE", 20, 20, 915, 455);
            holes[4] = new Hole(_scene, "NONE", 20, 20, 450, 455);
            holes[5] = new Hole(_scene, "NONE", 20, 20, 0, 455);

            for (int i = 0; i < holes.Length; i++)
            {
                _scene.AddObject(holes[i]);
            }

        }

        /// <summary>
        /// Creates the initial layout of the ball field by placing balls.
        /// This method generates positions for the white ball, black ball, solid balls,
        /// and striped balls on the game scene. It ensures that each ball is placed at a 
        /// valid position without overlapping with other balls or going out of bounds. 
        /// The white and black balls are positioned statically, while the solid and striped 
        /// balls are randomly distributed across predefined positions on the game field.
        /// Once the balls are created and positioned, they are added to the scene for 
        /// rendering and interaction.
        /// </summary>
        private void CreateBallField()
        {
            Random rnd = new Random();
            int x = 700; int y = 225;
            GamePoint[] pointArray = {new GamePoint(x - 80, y), new GamePoint(x - 40, y + 20), new GamePoint(x - 40, y - 20), new GamePoint(x, y - 40), new GamePoint(x, y + 40), new GamePoint(x + 40, y + 20), new GamePoint(x + 40, y + 60),
                new GamePoint(x + 40, y - 20), new GamePoint(x + 40, y - 60), new GamePoint(x + 80, y), new GamePoint(x + 80, y + 40), new GamePoint(x + 80, y + 80), new GamePoint(x + 80, y - 40), new GamePoint(x + 80, y - 80)};

            WhiteBall whiteBall = new WhiteBall(_scene, "Balls/WhiteBall.png", 0, 37, x - 510, y);
            BlackBall blackBall = new BlackBall(_scene, "Balls/8BlackBall.png", 0, 37, x, y);

            balls[0] = whiteBall; //adding the whiteball to the balls array
            balls[1] = blackBall; //adding the blackBall to the balls array 

            _scene.AddObject(whiteBall); //adding the whiteball to the scene
            _scene.AddObject(blackBall); //adding the blackBall to the scene

            for (int i = 0; i < solidBalls.Length; i++)
            {
                int num = rnd.Next(0, 14);
                GamePoint p = pointArray[num]; //14 possible points, if null, it means point is already used.
                while (p == null) //while the point is null, keep rolling until you get a different point that isn't null.
                {
                    num = rnd.Next(0, 14);
                    p = pointArray[num]; //roll point
                }
                solidBalls[i] = new SolidBall(_scene, "Balls/SolidBall" + (i + 1) + ".png", 0, 37, p.X, p.Y);
                balls[i + 2] = solidBalls[i]; //adding the current solidBall to the balls array
                _scene.AddObject(solidBalls[i]); //adding the current solidBall to the solidBalls array
                pointArray[num] = null;
            }

            for (int i = 0; i < stripedBalls.Length; i++)
            {
                int num = rnd.Next(0, 14);
                GamePoint p = pointArray[num]; //14 possible points, if null, it means point is already used.
                while (p == null) //while the point is null, keep rolling until you get a different point that isn't null.
                {
                    num = rnd.Next(0, 14);
                    p = pointArray[num]; //roll point
                }
                stripedBalls[i] = new StripedBall(_scene, "Balls/StripedBall" + (i + 1) + ".png", 0, 37, p.X, p.Y);
                balls[i + 9] = stripedBalls[i]; //adding the current stripedBall to the balls array
                _scene.AddObject(stripedBalls[i]); //adding the current stripedBall to the stripedBalls array
                pointArray[num] = null;
            }

            //for (int i = 0; i < balls.Length; i++)
            //{
            //    Scene.AddObject(balls[i]);
            //}

        }
    }
}
