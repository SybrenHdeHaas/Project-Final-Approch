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
    public static List<Breakable> breakableList = new List<Breakable>();

    public static Dictionary<string, Door> doorList = new Dictionary<string, Door>();
    public static Dictionary<string, Fan> fanList = new Dictionary<string, Fan>();

    public static String mapName = "Level1SS.tmx";
    public static bool mapIsMenu = false;
}