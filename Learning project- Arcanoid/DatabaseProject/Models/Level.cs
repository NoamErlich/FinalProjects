using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProject.Models
{
    public class Level
    {
        public int Id { get; set; }
        public int LevelNumber { get; set; } = 1;
        public double Speed { get; set; } = 2;
        public int BarWidth { get; set; } = 300;
        //public int BarX { get; set; }
        //public int BallX { get; set; }
        public int CountYellowBrick { get; set; } = 8;
        public int CountPinkBrick { get; set; } = 8;
        public int CountGreenBrick { get; set; } = 8;
    }
}
