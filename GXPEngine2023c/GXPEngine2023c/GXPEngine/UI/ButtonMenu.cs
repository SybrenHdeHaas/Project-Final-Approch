using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

/*
 * Button for the menu
 */
public class ButtonMenu : AnimationSpriteCustom
{
    string theAction;
    string stringPara1;
    bool paraBool2;
    TiledObject obj;
    public ButtonMenu(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        alpha = 0;
        theAction = obj.GetStringProperty("string_actionName", "");
        stringPara1 = obj.GetStringProperty("string_stringPara1", "");
        paraBool2 = obj.GetBoolProperty("bool_paraBool2");
        this.obj = obj;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            
            if (SharedFunctions.CheckPointWithRect(new Vector2(x, y), width, height, new Vector2(Input.mouseX, Input.mouseY)))
            {
                Console.WriteLine("write");
                switch (theAction)
                {
                    case "loadLevel":
                        GameData.mapName = stringPara1;
                        GameData.mapIsMenu = paraBool2;
                        MyGame myGame = (MyGame)game;
                        myGame.LoadLevel();
                        break;
                }
            }
        }
    }
}