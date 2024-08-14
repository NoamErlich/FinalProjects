using GameEngine.GameObjects;
using GameEngine.GameServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Arcanoid.GameObjects
{
    public class Ball : GameMovingObject
    {
        private int _countLifes;
        private double _speedX;


        public Ball(Scene scene, string fileName, double speed, double width, double placeX, double placeY) :
            base(scene, fileName, placeX, placeY)
        {
            _dX = 0;
            _dY = 0;
            Image.Width = width;
            _countLifes = 3;
            _speedX = speed;

            Manager.GameEvent.OnKeyDown += KeyDown;
            Manager.GameEvent.OnKeyUp += KeyUp;
        }

        private void KeyDown(VirtualKey key)
        {
            if (_dY == 0)
            {
                switch (key)
                {
                    case VirtualKey.Left:
                    case VirtualKey.A:
                        MoveTo(int.MinValue, _Y, _speedX, 0);
                        break;

                    case VirtualKey.Right:
                    case VirtualKey.D:
                        MoveTo(int.MaxValue, _Y, _speedX, 0);
                        break;

                    case VirtualKey.Up:
                    case VirtualKey.W:
                        _dY = -6;
                        break;
                }
            }
                
        }

        private void KeyUp(VirtualKey key)
        {
            if (_dY == 0)
                _dX = 0;
        }

        public override void Render()
        {
            if (_X <= 0) //נגיעה בקיר שמאלי
            {
                _dX = -_dX;
                _X = 0;
            }
            else if (_X >= _scene?.ActualWidth - Width) //נגיעה בקיר ימני
            {
                _dX= -_dX;
                _X = _scene.ActualWidth - Width;
            }
            else if(_Y <= 0) //נגיעה בתיקרה
            {
                _dY = -_dY;
                _Y = 0;
            }
            else if (_Y >= _scene?.ActualHeight - Height) //נגיעה ברצפה- פספוס
            {
                //_dY = -_dY;
                //_Y = _scene.ActualHeight - Height;
                Stop();
                _scene.Init();
                if (Manager.GameEvent.OnRemoveHeart != null)
                {
                    Manager.GameEvent.OnRemoveHeart(--_countLifes);
                }
            }
            base.Render();

        }

        public override void Collide(GameObject gameObject)
        {
            if(gameObject is Bar bar)
            {
                _dY = -_dY;
                if (Math.Abs(bar.dX) != 0)
                {
                    _dX += bar.dX / 2.6; //מעניק לכדור כמחצית מהמהירות של המחבט, כלומר, הכדור נוטה לכיוון תנועת המחבט
                }
                _Y = bar.Rect.Top - Height;
            }

            if (gameObject is Brick brick)
            {
                var intersectRect = RectHelper.Intersect(Rect, brick.Rect);
                if (intersectRect.Height > intersectRect.Width) //touch vertical sides
                {
                    _X -= _dX;
                    _dX = -_dX;
                }
                else
                {
                    _Y -= _dY;
                    _dY = -_dY;
                }
                brick.ChangeBrick();
            }
        }
    }
}
