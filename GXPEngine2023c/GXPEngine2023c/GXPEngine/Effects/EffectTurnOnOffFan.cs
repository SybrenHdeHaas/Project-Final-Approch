using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

//performs a action like (platforms appearing, doors opening, etc.). 
public class EffectTurnOnOffFan : Effect
{
    Fan theFan;
    public EffectTurnOnOffFan(string theFanID)
    {
        theFan = GameData.fanList[theFanID];
    }

    //open or close the door
    public override void TryAction()
    {
        theFan.isOn = !theFan.isOn;
    }

    public override void TryActionOpposite()
    {
        theFan.isOn = !theFan.isOn;
    }
}

