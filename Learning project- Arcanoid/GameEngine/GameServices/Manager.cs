using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using static GameEngine.GameServices.Constants;

namespace GameEngine.GameServices
{
    /// <summary>
    /// Abstract class representing a manager responsible for controlling game events and state.
    /// </summary>
    public abstract class Manager
    {
        public Scene Scene { get; private set; } // Gets the scene associated with the manager.

        private static DispatcherTimer _runTimer; // Timer for running game events continuously

        public static GameEvents GameEvent { get; } = new GameEvents(); // Gets the static game events instance.

        public static GameState GameState { get; set; } = GameState.Loaded; // Gets or sets the current game state.

        /// <summary>
        /// Constructs a new instance of the Manager class with the specified scene.
        /// </summary>
        /// <param name="scene">The scene associated with the manager.</param>
        public Manager(Scene scene)
        {
            Scene = scene;
            _runTimer = new DispatcherTimer();
            _runTimer.Interval = TimeSpan.FromMilliseconds(Constants.RunInterval);
            _runTimer.Tick += _runTimer_Tick;
            _runTimer.Start();
            // Registering for keyboard events
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
        }

        /// <summary>
        /// Event handler for the key release event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="args">The event arguments.</param>
        private void CoreWindow_KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            if (GameEvent.OnKeyUp != null)
                GameEvent.OnKeyUp(args.VirtualKey);
        }

        /// <summary>
        /// Event handler for the key press event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="args">The event arguments.</param>
        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (GameEvent.OnKeyDown != null)
                GameEvent.OnKeyDown(args.VirtualKey);
        }

        /// <summary>
        /// Event handler for the run timer tick event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void _runTimer_Tick(object sender, object e)
        {
            if (GameEvent.OnRun != null)
            {
                GameEvent.OnRun();
                GameEvent.OnTurn();
            }
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void Start()
        {
            Scene.Init();
            _runTimer.Start();
            GameState = GameState.Started;
        }

        /// <summary>
        /// Pauses the game.
        /// </summary>
        public void Pause()
        {
            _runTimer.Stop();
            GameState = GameState.Paused;
        }

        /// <summary>
        /// Resumes the game.
        /// </summary>
        public void Resume()
        {
            _runTimer.Start();
            GameState = GameState.Started;
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        /// <returns>True if the game is ended, otherwise false.</returns>
        public bool GameOver()
        {
            if (GameState != GameState.GameOver)
            {
                GameState = GameState.GameOver;
                _runTimer.Stop();
                return true;
            }
            return false;
        }
    }
}
