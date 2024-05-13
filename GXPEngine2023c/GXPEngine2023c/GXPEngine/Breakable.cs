using GXPEngine;
using GXPEngine.Core;
using System;
using System.Runtime.InteropServices;
using TiledMapParser;

internal class Breakable : Sprite
{
    
    
    public Breakable(TiledObject obj = null) : base("Breakable_Wall.png")
    {
        SetOrigin(width / 2, height / 2);
        

    }



    void Update() 
    {
        /*if (Input.GetKey(Key.G)) {
            Console.WriteLine(x);
        }*/

    }










}

