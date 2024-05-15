using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class Checkpoint : AnimationSpriteCustom
{
    public Vec2 spawnPosition;
    public Checkpoint(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        spawnPosition.x = obj.GetFloatProperty("float_spawnPositionX");
        spawnPosition.y = obj.GetFloatProperty("float_spawnPositionY");
    }


    //if there's no image in Tiled for this object
    public Checkpoint(TiledObject obj = null) : base("player.png", 1, 1, obj)
    {
        alpha = 0;
        spawnPosition.x = obj.GetFloatProperty("float_spawnPositionX");
        spawnPosition.y = obj.GetFloatProperty("float_spawnPositionY");
    }
}