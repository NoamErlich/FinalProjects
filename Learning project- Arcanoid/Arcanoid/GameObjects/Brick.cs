using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GameEngine.GameObjects;
using GameEngine.GameServices;
using Windows.Foundation;

namespace Arcanoid.GameObjects
{
    public class Brick : GameObject
    {
        public override Rect Rect => new Rect(_X, _Y, Width, Height - 37); //המלבן שמקיף את העצם

        public enum BrickType
        { 
            Green = 0,
            Pink = 1,
            Yellow = 2,
        }

        private BrickType _brickType;

        public Brick(Scene scene, BrickType brickType, double width, double height ,double placeX, double placeY) : base(scene, string.Empty, placeX, placeY)
        {
            Image.Width = width;
            Image.Height = height;
            _brickType = brickType;
            SetImage();
        }
        
        private void SetImage()
        {
            switch (_brickType)
            {
                case BrickType.Green:
                    base.SetImage("Brick/greenBrick.png");
                    break;
                case BrickType.Pink:
                    base.SetImage("Brick/pinkBrick.png");
                    break;
                case BrickType.Yellow:
                    base.SetImage("Brick/yellowBrick.png");
                    break;
            }

        }

        internal void ChangeBrick()
        {
            if (_brickType == BrickType.Yellow)
                _brickType = BrickType.Pink;
            else if (_brickType == BrickType.Pink)
                _brickType = BrickType.Green;
            else if (_brickType == BrickType.Green)
            {
                _scene.RemoveObject(this);
                if (Manager.GameEvent.OnRemoveBrick != null)
                {
                    Manager.GameEvent.OnRemoveBrick();
                }
            }
            SetImage();
        }
    }
}
