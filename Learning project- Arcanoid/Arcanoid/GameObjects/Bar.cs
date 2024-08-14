using GameEngine.GameObjects;
using GameEngine.GameServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.PointOfService;
using Windows.Security.Cryptography.Core;
using Windows.System;
using Windows.UI.Xaml.Input;

namespace Arcanoid.GameObjects
{

    /// <summary>
    /// המחלקה מייצגת עצם משטח מחבט
    /// </summary>
    public class Bar : GameMovingObject
    {
        public double dX => _dX;
        private double _speed;

        /// <summary>
        /// הפעולה בונה עצם מחבט
        /// </summary>
        /// <param name="scene">במת המשחק</param>
        /// <param name="fileName">שם הקובץ שמתאר את המחבט</param>
        /// <param name="speed">מהירות אופקית</param>
        /// <param name="width">רוחב המחבט</param>
        /// <param name="placeX">מיקום היווצרות המחבט ביחס לציר האופקי</param>
        /// <param name="placeY">מיקום היווצרות המחבט ביחס לציר אנכי</param>
        public Bar(Scene scene, string fileName, double speed, double width,double placeX, double placeY) :
            base(scene, fileName, placeX, placeY)
        {
            _speed = speed;
            Image.Height = 20; //כך אנו קובעים את עובי המחבט
            Image.Width = width; //כך אנו קובעים את רוחב המחבט
            Image.Stretch = Windows.UI.Xaml.Media.Stretch.Fill; //כך אנו מותחים מראה המחבט

            //כך המחבט נרשם לאירועים הקשורים למקלדת
            Manager.GameEvent.OnKeyDown += KeyDown;
            Manager.GameEvent.OnKeyUp += KeyUp;
        }

        /// <summary>
        /// באמצעות הפעולה הזאת המחבט מגיב לעזיבת המקש
        /// </summary>
        /// <param name="key"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void KeyUp(VirtualKey key)
        {
            switch(key)
            {
                case VirtualKey.Left:
                case VirtualKey.A:
                case VirtualKey.Right:
                case VirtualKey.D:
                    Stop();
                    break;
            }
        }

        /// <summary>
        /// באמצעות הפעולה הזאת המחבט מגיב ללחיצה על המקלדת
        /// </summary>
        /// <param name="key"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void KeyDown(VirtualKey key)
        {
            switch(key)
            {
                case VirtualKey.Left:
                case VirtualKey.A:
                    MoveTo(int.MinValue, _Y, _speed, 0);
                    break;

                case VirtualKey.Right:
                case VirtualKey.D:
                    MoveTo(int.MaxValue, _Y, _speed, 0);
                    break;
            }
        }

        public override void Render()
        {
            base.Render();
            
            //בדיקת קירות הזירה
            if(_X <= 0) //קצה שמאלי
            {
                _X = 0;
                Stop();
            }    
            else if(_X >= _scene?.ActualWidth - Width) //כאשר אובייקט יגע בצד הימני שלו עם הקיר הימני של הזירה
            {
                _X = _scene.ActualWidth - Width;
                Stop();
            }

            //Messenger.Send(place);
        }
    }
}
