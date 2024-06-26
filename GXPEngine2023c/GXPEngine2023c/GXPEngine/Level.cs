﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.Remoting.Messaging;
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

    Dictionary<string, Fan> fanList = new Dictionary<string, Fan>();
    List<FanArea> fanAreaList = new List<FanArea>();
    public Level(string theMapfileName)
    {
        Map mapData = MapParser.ReadMap(theMapfileName);
        loader = new TiledLoader(theMapfileName);

        //Manually generates the tile layers.
        CreateTile(mapData, 0); //Create the walls
     //   CreateTile(mapData, 1); //Create background

        //using autoInstance to Automatically generates the game objects
        loader.autoInstance = true;
        loader.rootObject = this;
        loader.LoadImageLayers();
        loader.LoadObjectGroups(0); //loading game objects

        //find the player objects (should only be 2)
        foreach (Player thePlayer in FindObjectsOfType<Player>()) 
        {
            Console.WriteLine("player spawn position: " + thePlayer.x + "|" + thePlayer.y);
            thePlayer.UpdatePos();
            thePlayers.Add(thePlayer);
        }

        GameData.playerList = thePlayers;

        //Extracting all Fan objects
        foreach (Fan theFan in FindObjectsOfType<Fan>())
        {
            fanList.Add(theFan.id, theFan);
        }

        foreach (FanArea theFanArea in FindObjectsOfType<FanArea>())
        {
         //   theFanArea.visible = false;
            fanAreaList.Add(theFanArea);
        }

        //Setting up the camera boundary (player at center for these values)
        boundaryValueX = game.width / 2;
        boundaryValueY = game.height / 2;
    }

    void Update()
    {
        GameData.playerList = thePlayers;

        UseCamera();

        foreach (Player player in thePlayers) { player.ResetFanVelocityList(); }
            
        CheckFanAreas();
    }


    void CheckFanAreas()
    {
        foreach (FanArea theFanArea in fanAreaList)
        {
            foreach (Player player in thePlayers)
            {
                /* intersection btw player model (not red box) and fan area [working] --> SharedFunctions.CheckIntersectSprites(player, theFanArea) */

                //check intersection btw player red box and fan area
                if (SharedFunctions.CheckIntersectSpriteDetectionRange(player, theFanArea))
                {
                    Fan theFan;
                    if (fanList.TryGetValue(theFanArea.TheFanID, out theFan))
                    {
                        player.AddFanVelocity(theFan.GetVelocity());
                        Console.WriteLine(theFan.GetVelocity());
                    }
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

            if (player.x + x > boundaryValueX && x > -1 * ((game.width * 6) - 800))
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
            if (player.y + y > boundaryValueY && y > -1 - (game.height * 2) - 100)
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