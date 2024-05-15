using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

/* a door that can be opened and closed (from button press) */
public class Spike : AnimationSpriteCustom
{
    public bool isOpened;
    public string theID;

    //if there's image in Tiled for this object
    public Spike(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {

    }

    //if there's no image in Tiled for this object
    public Spike(TiledObject obj = null) : base("player.png", 1, 1, obj)
    {
        alpha = 0;
    }
}