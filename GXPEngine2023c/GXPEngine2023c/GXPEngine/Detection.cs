using GXPEngine;
using System;
using System.Collections.Generic;
using System.Threading;

//handles player-to-player detection
public class Detection : Sprite
{
    /*
    private string collisionDirection;
    public string getCollisionDirection() { return collisionDirection; }
    Player player;
    Boolean[] collisionSides = new Boolean[4]; //wich sides are colliding (up, down, left, right)

    private float[] startStats = new float[4];
    private float[] shellsStats = new float[4];
    private Detection dete;
    public Detection GetDete() { return dete; }
    public Player GetPlayer() { return player; }
    */

    int extraWidth;
    int extraHeight;
    public DetectionInfo theDetectionInfo;

    public Detection(int extraWidth = 20, int extraHeight = 20) : base("detector.png")
    {
        this.extraWidth = extraWidth;
        this.extraHeight = extraHeight;
        theDetectionInfo = new DetectionInfo();
    }

    public void ChangeOffSetAndSize(Vec2 playerPos, float[] theStats)
    {
        x = playerPos.x + theStats[0] - extraWidth / 2;
        y = playerPos.y + theStats[1] - extraHeight / 2;
        width = (int)theStats[2] + extraWidth;
        height = (int)theStats[3] + extraHeight;
    }
    public void PlayerInteractCheck()
    {
        foreach (Player thePlayer in GameData.playerList)
        {
            if (thePlayer.detectionRange != this)
            {
                if (SharedFunctions.CheckIntersetSpriteBothTopLeftCorner(this, thePlayer.detectionRange))
                {
                    theDetectionInfo.UpdateInfo(true, thePlayer);   
                }

                else
                {
                    theDetectionInfo.UpdateInfo(false, null);
                }
            }
        }

    }


    void ToggleVisable()
    {

        if (Input.GetKeyDown(Key.TAB))
        {
            if (visible) { visible = false; } else visible = true;
        }

    }
}