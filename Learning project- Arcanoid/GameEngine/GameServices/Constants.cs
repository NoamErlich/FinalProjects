using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameServices
{
    public class Constants
    {
        public const double RunInterval = 5; //the speed the project will run at // pc = 5, laptop = 0.001
        public const int SpeedUnit = 10; //global speed unit for gameMovingObject

        public enum GameState //game state setting
        { 
            Loaded,
            Started,
            Paused,
            GameOver
        }

    }
}
