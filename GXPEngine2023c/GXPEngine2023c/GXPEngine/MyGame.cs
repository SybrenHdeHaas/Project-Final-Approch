using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions

public class MyGame : Game {

	private Player player1;

	public MyGame() : base(800, 600, false)     // Create a window that's 800x600 and NOT fullscreen
	{
        Level theLevel = new Level("map1.tmx");
        AddChild(theLevel);

		player1 = new Player(0);
		AddChild(player1);
		player1.SetXY(64, 64);

    }

	// For every game object, Update is called every frame, by the engine:
	void Update() {
		// Empty
	}

	static void Main()                          // Main() is the first method that's called when the program is run
	{
		new MyGame().Start();                   // Create a "MyGame" and start it
		
	}
}