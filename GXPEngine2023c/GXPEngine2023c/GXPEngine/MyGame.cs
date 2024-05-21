using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Collections.Generic;
using System.Reflection.Emit;
public class MyGame : Game
{

    SoundChannel mainSoundtrack;
    public MyGame() : base(1800, 600, false)     // Create a window that's 800x600 and NOT fullscreen
    {
        LoadLevel();
    }

    public void LoadLevel()
    {
        GameData.ResetValue();



        //Destroy old level
        List<GameObject> children = GetChildren();
        foreach (GameObject child in children)
        {
            child.LateDestroy();
        }

        if (mainSoundtrack != null)
        {
            mainSoundtrack.Stop();
        }


        switch (GameData.mapName)
        {
            case "redoneLevel3SS.tmx":
                mainSoundtrack = new Sound("soundtrack level 3 loop.mp3", true, false).Play();
                break;
            case "redoneLevel2SS.tmx":
                int randomNum1 = Utils.Random(1, 3);

                if (randomNum1 == 1)
                {
                    mainSoundtrack = new Sound("soundtrack level 2 loop.mp3", true, false).Play();
                }

                else
                {
                    mainSoundtrack = new Sound("bob the snail type shit new.mp3", true, false).Play();
                }
                break;
            default:

                int randomNum = Utils.Random(1, 3);

                if (randomNum == 1)
                {
                    mainSoundtrack = new Sound("The Turtles - HELP! (LOOP).mp3", true, false).Play();
                }

                else
                {
                    mainSoundtrack = new Sound("soundtrack level 1 loop.mp3", true, false).Play();
                }
                break;
        }

        //load level
        Level theLevel = new Level(GameData.mapName, GameData.mapIsMenu);
        AddChild(theLevel);
    }

    static void Main()                          // Main() is the first method that's called when the program is run
    {
        new MyGame().Start();                   // Create a "MyGame" and start it

    }
}