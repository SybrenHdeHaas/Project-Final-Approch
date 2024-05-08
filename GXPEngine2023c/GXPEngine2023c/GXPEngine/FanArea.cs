using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class FanArea : AnimationSpriteCustom
{
    string theFanID;

    public String TheFanID
    {
        get
        {
            return theFanID;
        }
    }

    public FanArea(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        theFanID = obj.GetStringProperty("string_fanID");
    }
}