using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Input;

namespace GameEngine.GameServices
{
    public class GameEvents
    {
        public Action OnRun;                 //The event where everyone who registers will move
        public Action OnClock;               //The event that will trigger the clock
        public Action<VirtualKey> OnKeyUp;   //The event that those who sign up for will be able to respond to the release of a key
        public Action<VirtualKey> OnKeyDown; //The event that whoever registers for will be able to respond to the press of a key
        public Action<int> OnRemoveHeart;    //The event that whenever the ball reaching the floor will cause the game page to lose heart
        public Action OnRemoveBrick;         //The event that whenever the ball hits a brick it will cause the brick to change level\color or disapper.
        public Action OnMovingMouse;         //whenever a player moves the mouse, the event accuers.
        public Action OnTurn;                //whenever a turn is ended the event accuers.


        /// <summary>
        /// resets all the events to null.
        /// </summary>
        public void RemoveAllActions()
        {
            OnRun = null;
            OnClock = null;
            OnRemoveHeart = null;
            OnRemoveBrick = null;
            OnKeyUp = null;
            OnKeyDown = null;
            OnTurn = null;
        }
    }
}
