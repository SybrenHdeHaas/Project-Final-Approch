using GXPEngine;
using GXPEngine.Core;
using System;
using System.Runtime.InteropServices;
using TiledMapParser;

internal class Breakable : Tile
{
    Vec2 thresholdVelocity; //if the player collides with this over this velocity amount, it's health will be damaged by 1
    public Breakable(string theImageName, float scaleX, float scaleY, int singleFrameID, int columns, int rows,
        int numberOfFrame, int startFrame, int endFrame, int nextFrameDelay, bool textureKeepInCache, bool hasCollision) :
        base(theImageName, scaleX, scaleY, singleFrameID, columns, rows,
         numberOfFrame, startFrame, endFrame, nextFrameDelay, textureKeepInCache, hasCollision)
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

