using GXPEngine;
using GXPEngine.Core;
using System;
using System.Drawing;

internal class Detection : Sprite
{

    public Detection() : base("detector.png")
    {
       collider.isTrigger = true;
       SetOrigin(width / 2, height / 2);

    }
}