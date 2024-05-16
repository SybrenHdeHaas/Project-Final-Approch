using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Collections.Generic;
using System.Reflection.Emit;
public class MyGame : Game {
	public MyGame() : base(1800, 600, false)     // Create a window that's 800x600 and NOT fullscreen
	{
		LoadLevel();
    }

    public void LoadLevel()
	{
        //Destroy old level
        List<GameObject> children = GetChildren();
        foreach (GameObject child in children)
        {
            child.LateDestroy();
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