using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

//performs a action like (platforms appearing, doors opening, etc.). 
public class EffectOpenCloseDoor : Effect
{
    Door theDoor;
    public EffectOpenCloseDoor(string theDoorID)
    {
        theDoor = GameData.doorList[theDoorID];
    }

    //open or close the door
    public override void TryAction()
    {

        /* //debug message to view the conent of doorlist
        foreach (KeyValuePair<string, Door> theDoor in GameData.doorList)
        {
            Console.WriteLine("door idddd: " + theDoor.Value.theID);
        }
        */

        theDoor.isOpened = !theDoor.isOpened;
        Console.WriteLine(theDoor.isOpened);
    }
}

