using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using GXPEngine;
using TiledMapParser;
/*
 * The level Gameobject. For generating the game itself as a menu screen or a game level
 * The game should only have one level
 */
public class Level : GameObject
{
    TiledLoader loader;
    List<Player> thePlayers = new List<Player>();

    //Determine the position the player will be displayed in the game camera
    float boundaryValueX; //Should be width / 2 to display the player at the center of the screen
    float boundaryValueY; //Should be height / 2 to display the player at the center of the screen

    Dictionary<string, Door> doorList = new Dictionary<string, Door>();
    Dictionary<string, Fan> fanList = new Dictionary<string, Fan>();
    List<FanArea> fanAreaList = new List<FanArea>();
    List<Button> buttonList = new List<Button>();

    List<Checkpoint> checkpointList = new List<Checkpoint>();
    List<Spike> spikeList = new List<Spike>();

    List<Goal> goalList = new List<Goal>();
    List<Breakable> breakableList = new List<Breakable>();


    float cameraMaxButtom;
    float cameraMaxRight;
    public Level(string theMapfileName, bool isMenu)
    {
        Console.WriteLine("aa:" + (-1 - (game.height * 2) - 100));

        switch(GameData.mapName)
        {
            case "Level1SS.tmx":
                cameraMaxButtom = (-1 * 32 * 20) + game.height;
                cameraMaxRight = (-1 * 32 * 43) + game.width;
                break;
            default:
                cameraMaxButtom = -1 - (game.height * 2) - 100;
                cameraMaxRight = -1 - (game.height * 2) - 100;
                break;
        }


        Map mapData = MapParser.ReadMap(theMapfileName);
        loader = new TiledLoader(theMapfileName);

        if (!isMenu)
        {
            //Manually generates the tile layers.
            CreateTile(mapData, 0); //Create the walls
            CreateTile(mapData, 1); //Create background
        }

        //using autoInstance to Automatically generates the game objects
        loader.autoInstance = true;
        loader.rootObject = this;
        loader.LoadImageLayers();
        loader.LoadObjectGroups(0); //loading game objects
        loader.LoadObjectGroups(1); //loading game objects

        //find the player objects (should only be 2)
        foreach (Player thePlayer in FindObjectsOfType<Player>()) 
        {
            thePlayer.UpdatePos();
            thePlayers.Add(thePlayer);
            thePlayer.spawnPoint = new Vec2(thePlayer.x, thePlayer.y); //the default player spawn point
        }

        GameData.playerList = thePlayers;

        //extracting door objects
        foreach (Door theDoor in FindObjectsOfType<Door>())
        {
            doorList.Add(theDoor.theID, theDoor);
        }

        GameData.doorList = doorList;

        //extracting button objects
        foreach (Button theButton in FindObjectsOfType<Button>())
        {
            buttonList.Add(theButton);
        }

        //extracting Fan objects
        foreach (Fan theFan in FindObjectsOfType<Fan>())
        {
            fanList.Add(theFan.id, theFan);
        }

        GameData.fanList = fanList;

        //extracting Fanarea objects
        foreach (FanArea theFanArea in FindObjectsOfType<FanArea>())
        {
         //   theFanArea.visible = false;
            fanAreaList.Add(theFanArea);
        }

        //extracting checkpint objects
        foreach (Checkpoint theCheckpoint in FindObjectsOfType<Checkpoint>())
        {
            checkpointList.Add(theCheckpoint);
        }

        //extracting spike objects
        foreach (Spike theSpike in FindObjectsOfType<Spike>())
        {
            spikeList.Add(theSpike);
        }

        //extracting goal objects
        foreach (Goal theGoal in FindObjectsOfType<Goal>())
        {
            goalList.Add(theGoal);
        }

        //extracting goal objects
        foreach (Breakable theBreakable in FindObjectsOfType<Breakable>())
        {
            Console.WriteLine("bx: " + theBreakable.x + " | by: " + theBreakable.y);
            breakableList.Add(theBreakable);
        }

        GameData.breakableList = breakableList;

        //Setting up the camera boundary (player at center for these values)
        boundaryValueX = game.width / 2;
        boundaryValueY = game.height / 2;
    }

    void Update()
    {
        UseCamera();

        foreach (Player player in thePlayers) 
        {
            player.ResetFanVelocityList();
        }

        breakableList = GameData.breakableList;

        CheckFanAreas();
        CheckCheckpoint();
        CheckButtons();
        CheckSpike();
        CheckGoal();
        CheckBreakable();

        if (Input.GetKey(Key.R))
        {
            MyGame myGame = (MyGame)game;
            myGame.LoadLevel();
        }
    }


    void CheckFanAreas()
    {
        foreach (FanArea theFanArea in fanAreaList)
        {
            foreach (Player player in thePlayers)
            {
                if (SharedFunctions.CheckIntersectSpriteDetectionRange(player, theFanArea))
                {
                    Fan theFan;
                    if (fanList.TryGetValue(theFanArea.TheFanID, out theFan))
                    {
                        player.AddFanVelocity(theFan.GetVelocity());
                    //    Console.WriteLine(theFan.GetVelocity());
                    }
                }
            }
        }
    }

    void CheckButtons()
    {
        foreach (Button theButton in buttonList)
        {
            foreach (Player player in thePlayers)
            {
                if (SharedFunctions.CheckIntersectSpriteDetectionRange(player, theButton))
                {
                    if (player.playerIndex == 0 && theButton.hasPressed == false)
                    {
                        theButton.isPressedPlayer1 = true;
                    }

                    if (player.playerIndex == 1 && theButton.hasPressed == false)
                    {
                        theButton.isPressedPlayer2 = true;
                    }
                }

                else
                {
                    if (player.playerIndex == 0)
                    {
                        theButton.isPressedPlayer1 = false;
                    }

                    if (player.playerIndex == 1)
                    {
                        theButton.isPressedPlayer2 = false;
                    }

                    if (theButton.isPressedPlayer1 == false && theButton.isPressedPlayer2 == false)
                    {
                        theButton.hasPressed = false;
                    }
                }
            }
        }
    }

    void CheckCheckpoint()
    {
        foreach (Checkpoint theCheckpoint in checkpointList)
        {
            foreach (Player player in thePlayers)
            {
                if (SharedFunctions.CheckIntersectSpriteDetectionRange(player, theCheckpoint))
                {
                    player.spawnPoint = theCheckpoint.spawnPosition;
                    Console.WriteLine("player spawn set to: " + player.spawnPoint.x + "|" + player.spawnPoint.y);
                }
            }
        }
    }

    void CheckSpike()
    {
        foreach (Spike theSpike in spikeList)
        {
            foreach (Player player in thePlayers)
            {
                if (SharedFunctions.CheckIntersectSpriteDetectionRange(player, theSpike))
                {
                    Console.WriteLine("player touche spike move to: " + player.spawnPoint.x + "|" + player.spawnPoint.y);
                    player.Position = player.spawnPoint;
                    player.Velocity = new Vec2(0, 0);
                }
            }
        }
    }


    void CheckGoal()
    {
        foreach (Goal theGoal in goalList)
        {
            foreach (Player player in thePlayers)
            {
                if (SharedFunctions.CheckIntersectSpriteDetectionRange(player, theGoal))
                {
                    theGoal.switchLevel();
                    return;
                }
            }
        }
    }

    void CheckBreakable()
    {
        foreach (Breakable theBreakable in breakableList)
        {
            foreach (Player player in thePlayers)
            {
                if (SharedFunctions.CheckIntersectSpriteDetectionRange(player, theBreakable))
                {
                  //  Console.WriteLine("Player velocity length: " + player.Velocity.Length());

                }
            }
        }
    }

    //Sets the game area player can look. AKA the game camera
    //Can set how far right and down player can see. (left stops at x < 0, top stops at y < 0)
    void UseCamera()
    {

        //first determine if the camera moves, then determine the max distance the camera can move
        //handling player moving right
        foreach (Player player in thePlayers)
        {
            //for now, camera only follow player1
            if (player.GetPlyaerIndex() != 0)
            {
                return;
            }

            if (player.x + x > boundaryValueX && x > cameraMaxRight)
            {
                x = boundaryValueX - player.x;
            }

            //handling player moving left
            if (player.x + x < game.width - boundaryValueX && x < 0)
            {
                x = game.width - boundaryValueX - player.x;
            }

            //handling player moving up
            if (player.y + y < game.height - boundaryValueY && y < 0)
            {
                y = game.height - boundaryValueY - player.y;
            }

            //handling player moving down
            if (player.y + y > boundaryValueY && y > cameraMaxButtom)
            {
                y = boundaryValueY - player.y;
            }
        }
    }


    //Spawns all the Tiles of the level
    void CreateTile(Map mapData, int theLayer)
    {
        //Check if map data is not empty
        if (mapData.Layers == null || mapData.Layers.Length == 0)
        {
            return;
        }
        Layer layer = mapData.Layers[theLayer];


        //Helps to render all the background (layer 1) sprites in one call.
        SpriteBatch background = new SpriteBatch();
        AddChild(background);

        short[,] tileNumbers = layer.GetTileArray(); //holding the tile data

        //Generating the tiles depends on the map data.
        //Extracting the 2d array extracted from the map data. Each number represent a specific tile
        for (int i = 0; i < layer.Height; i++)
        {
            for (int j = 0; j < layer.Width; j++)
            {
                int theTileNumber = tileNumbers[j, i]; //extracting the tile number
                TileSet theTilesSet = mapData.GetTileSet(theTileNumber); //what tile set the number comes from

                //A number with value 0 means no tile, so ignore number 0
                if (theTileNumber != 0)
                {
                    Tile theTile; //the tile object to be added to the game level

                    /*
                     * Determining what type of tile the tile object will be based on the tile layer.
                     * Layer 0 represents wall tiles, layer 1 represents background tiles
                     */

                // A wall tile. collision on
                if (theLayer == 0)
                    {
                        theTile = new Tile(theTilesSet.Image.FileName, 1, 1, theTileNumber - theTilesSet.FirstGId,
                            theTilesSet.Columns, theTilesSet.Rows, -1, 1, 1, 10, false, true);
                        theTile.x = j * theTile.width;
                        theTile.y = (i) * theTile.height;
                        AddChild(theTile);
                        
                        GameData.tileList.Add(theTile);

                    }

                    //Background. collision off
                 else if (theLayer == 1)
                    {
                        theTile = new Tile(theTilesSet.Image.FileName, 1, 1, theTileNumber - theTilesSet.FirstGId,
                         theTilesSet.Columns, theTilesSet.Rows, -1, 1, 1, 10, false, false);
                        theTile.x = j * theTile.width;
                        theTile.y = i * theTile.height;
                        background.AddChild(theTile);
                    }
                }
            }
        }

        background.Freeze(); //Freeze all the background tiles. Creating better performance
    }
}