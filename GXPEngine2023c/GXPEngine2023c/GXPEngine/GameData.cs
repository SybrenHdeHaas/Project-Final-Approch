using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//holds variables all classes can use
public static class GameData
{
    public static List<Tile> tileList = new List<Tile>();
    public static List<Player> playerList = new List<Player>();
    public static Dictionary<string, Door> doorList = new Dictionary<string, Door>();
}