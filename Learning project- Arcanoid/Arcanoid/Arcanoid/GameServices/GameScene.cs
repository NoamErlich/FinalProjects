using Arcanoid.GameObjects;
using GameEngine.GameServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcanoid.GameServices
{
    public class GameScene : Scene
    {
        public int BrickCount => _gameObjectsSnapshot.Count(x => x is Brick);
    }
}
