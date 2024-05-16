using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

//switch to another level upon collision
public class Goal : AnimationSpriteCustom
{
    string mapID;
    bool isMenu;

    //if there's image in Tiled for this object
    public Goal(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        mapID = obj.GetStringProperty("string_theMapID");
        isMenu = obj.GetBoolProperty("bool_mapIsMenu");
    }

    //if there's no image in Tiled for this object
    public Goal(TiledObject obj = null) : base("player.png", 1, 1, obj)
    {
        alpha = 0;
        mapID = obj.GetStringProperty("string_theMapID");
        isMenu = obj.GetBoolProperty("bool_mapIsMenu");
    }

    public void switchLevel()
    {
        GameData.mapName = mapID;
        GameData.mapIsMenu = isMenu;
        MyGame myGame = (MyGame)game;
        myGame.LoadLevel();
    }

}