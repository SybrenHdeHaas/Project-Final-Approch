using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMapParser;

//holds variables all classes can use
public static class GameData
{
    public static List<Tile> tileList = new List<Tile>();
    public static List<Player> playerList = new List<Player>();
    public static Dictionary<string, Door> doorList = new Dictionary<string, Door>();

    public static String mapName = "MainMenu.tmx";
    public static bool mapIsMenu = true;
}