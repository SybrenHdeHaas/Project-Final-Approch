using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

/*
*Same as a AnimationSpriteCustom class, but does the animtion logic.
*This class also allows an animated sprite or an animated sprite that acts like sprite to have
*their properties such as image source name, scale, frame and more to be customized (from Tiled).  
*/
public class AnimationSpriteCustom : AnimationSprite
{
    /*
     *Setting singleFrameID to a value not -1 will make the object act like a sprite with the only frame being it's value. 
     *Useful if you want an object that inherits from this acts like a sprite or setup a sprite that has it's image source being a tileset-like image.
     */
    int singleFrameID;
    int nextFrameDelay; //decides how many time the game frame should pass before the next frame

    public string id;     //a unique id a gameobject can have (for help finding a specific gameobject)
    public string groupID; //a group id a gameobject can have (for help finding a specific gameobject group)


    public AnimationSpriteCustom(string filenName, float scaleX, float scaleY, int singleFrameID, int columns, int rows,
        int numberOfFrames, int startFrame, int endFrame, int nextFrameDelay, bool textureKeepInCache, bool hasCollision) :
        base(filenName, columns, rows, numberOfFrames, textureKeepInCache, hasCollision)
    {
        this.scaleX = scaleX;
        this.scaleY = scaleY;
        this.singleFrameID = singleFrameID;
        this.nextFrameDelay = nextFrameDelay;

        if (singleFrameID != -1)
        {
            SetFrame(singleFrameID);
        }

        else
        {
            SetAnimationCycle(startFrame, endFrame);
        }
    }


    public AnimationSpriteCustom(string filenName, int columns, int rows, TiledObject obj = null) :
    base(filenName, columns, rows, obj.GetIntProperty("i_numberOfFrame", 1), obj.GetBoolProperty("i_textureKeepInCache", false)
        , obj.GetBoolProperty("p_hasCollision", false))
    {
        singleFrameID = obj.GetIntProperty("i_singleFrameID", 1);
        SetNextFrameDelay(obj.GetIntProperty("i_nextFrameDelay", 1));
        id = obj.GetStringProperty("f_theID", "none");
        groupID = obj.GetStringProperty("f_theGroupID", "");

        if (singleFrameID != -1)
        {
            SetFrame(singleFrameID);
        }

        else
        {
            SetAnimationCycle(obj.GetIntProperty("i_startFrame", 1), obj.GetIntProperty("i_startNumberOfFrames", 1));
        }

        if (obj.GetBoolProperty("p_hasCollision", false) == true && obj.GetBoolProperty("p_isTrigger", false))
        {
            collider.isTrigger = true;
        }

    }

    protected virtual void Update()
    {
        DoAnimation();
    }

    void DoAnimation()
    {
        if (singleFrameID != -1)
        {
            return;
        }
        Animate();
    }

    void SetNextFrameDelay(int delay)
    {
        if (delay > 255)
        {
            delay = 255;
        }

        if (delay < 0)
        {
            delay = 0;
        }

        nextFrameDelay = delay;
    }

    public void SetAnimationCycle(int theFrame, int amountOfFrames)
    {
        SetCycle(theFrame, amountOfFrames, (byte)nextFrameDelay);
    }
}