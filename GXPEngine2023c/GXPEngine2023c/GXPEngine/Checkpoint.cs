using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class Checkpoint : AnimationSpriteCustom
{
    bool isActivePlayer1; //if true, means this is the current spawn point p1 has
    bool isActivePlayer2; 
    Vec2 spawnPosition;
    public Checkpoint(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        spawnPosition.x = obj.GetFloatProperty("float_spawnPositionX");
        spawnPosition.y = obj.GetFloatProperty("float_spawnPositionY");
    }
}