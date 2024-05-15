using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

/* a door that can be opened and closed (from button press) */
public class Door : AnimationSpriteCustom
{
    public bool isOpened;
    public string theID;
    public bool isOpenedDefaultValue;

    //if there's image in Tiled for this object
    public Door(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        isOpened = obj.GetBoolProperty("isOpened");
        isOpenedDefaultValue = obj.GetBoolProperty("isOpened");
        theID = obj.GetStringProperty("string_theID");
    }

    //if there's no image in Tiled for this object
    public Door(TiledObject obj = null) : base("player.png", 1, 1, obj)
    {
        isOpened = obj.GetBoolProperty("isOpened");
        theID = obj.GetStringProperty("string_theID");
    }

}