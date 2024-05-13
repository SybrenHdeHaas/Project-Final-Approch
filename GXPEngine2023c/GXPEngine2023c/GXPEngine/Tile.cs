using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
public class Tile : AnimationSpriteCustom
{
    /*
     * The 'wall' of the level
     */

    public float widthHalf;
    public float heightHalf;

    public float Mass
    {
        get
        {
            return 4 * widthHalf * heightHalf * 1;
        }
    }

    public Tile(string theImageName, float scaleX, float scaleY, int singleFrameID, int columns, int rows,
        int numberOfFrame, int startFrame, int endFrame, int nextFrameDelay, bool textureKeepInCache, bool hasCollision) :
        base(theImageName, scaleX, scaleY, singleFrameID, columns, rows,
         numberOfFrame, startFrame, endFrame, nextFrameDelay, textureKeepInCache, hasCollision)
    {
        widthHalf = width / 2;
        heightHalf = height / 2;
    }

}