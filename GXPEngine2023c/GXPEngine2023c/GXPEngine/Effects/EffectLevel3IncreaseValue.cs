﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

//performs a action like (platforms appearing, doors opening, etc.). 
public class EffectLevel3IncreaseValue : Effect
{
    Door theDoor;
    public EffectLevel3IncreaseValue(string theDoorID)
    {
        theDoor = GameData.doorList[theDoorID];
    }

    //open or close the door
    public override void TryAction()
    {
        GameData.level3Value++;
        TryOpenDoor();
    }

    public override void TryActionOpposite()
    {
        GameData.level3Value--;
        TryOpenDoor();
    }


    void TryOpenDoor()
    {

        Console.WriteLine("value: " + GameData.level3Value);

        if (GameData.level3Value == 2)
        {
            theDoor.isOpened = true;
        }

        else
        {
            theDoor.isOpened = false;
        }
    }

}

