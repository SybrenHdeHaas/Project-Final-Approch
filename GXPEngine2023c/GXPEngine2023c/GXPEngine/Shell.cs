using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class Shell : AnimationSpriteCustom
{
    Vec2 Velocity;

    public Shell(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
    }
}