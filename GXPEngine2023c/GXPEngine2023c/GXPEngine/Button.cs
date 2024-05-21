using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class Button : AnimationSpriteCustom
{
    public bool isTypeOne;
    //true --> The first type will reverse whatever effect it had, when there is no player colliding with it
    //false --> the other type’s effects will stay indefinitely after a player has collided with it.

    public bool hasPressed;
    public bool isPressedPlayer1;
    public bool isPressedPlayer2;

    public bool breakableIsPressing;

    List<Effect> effects = new List<Effect>();

    TiledObject theObj;
    bool firstTime = true;

    bool typeOneOppositeEffect;

    public Button(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        isTypeOne = obj.GetBoolProperty("isTypeOne");
        hasPressed = false;
        theObj = obj;
    }

    public Button(TiledObject obj = null) : base("player.png", 1, 1, obj)
    {
        alpha = 0;
        isTypeOne = obj.GetBoolProperty("isTypeOne");
        hasPressed = false;
        theObj = obj;
    }

    public void AddNewAction(String pEffectName, String pParameterStringOne = "")
    {
        switch (pEffectName)
        {
            case "action_doorOpenClose":
                AddEffect(new EffectOpenCloseDoor(pParameterStringOne));
                break;
            case "action_fanTurnOnOff":
                AddEffect(new EffectTurnOnOffFan(pParameterStringOne));
                break;
            case "action_level3IncreaseValue":
                AddEffect(new EffectLevel3IncreaseValue(pParameterStringOne));
                break;
            case "action_level3IncreaseValue2":
                AddEffect(new EffectLevel3IncreaseValue2(pParameterStringOne));
                break;
        }
    }

    //do all effects the button has
    public void Action()
    {

        foreach (Effect effect in effects)
        {
            effect.TryAction();
        }
    }

    public void ActionOpposite()
    {
        foreach (Effect effect in effects)
        {
            effect.TryActionOpposite();
        }
    }

    public void AddEffect(Effect theEffect)
    {
        effects.Add(theEffect);
    }
    void Update()
    {
        //We can't continue if the doors are not finished loading
        if (GameData.doorList.Count == 0)
        {
            return;
        }

        //We can only add the effects if the door are loaded (aka when game data door list has values)
        //and when the doors are loaded we just want to add the effects once
        if (firstTime)
        {
            firstTime = false;
            AddNewAction(theObj.GetStringProperty("string_effectName"), theObj.GetStringProperty("string_para"));
            AddNewAction(theObj.GetStringProperty("string_effectName1"), theObj.GetStringProperty("string_para1"));
            AddNewAction(theObj.GetStringProperty("string_effectName2"), theObj.GetStringProperty("string_para2"));
            AddNewAction(theObj.GetStringProperty("string_effectName3"), theObj.GetStringProperty("string_para3"));
        }


        if (isTypeOne)
        {
            if (((isPressedPlayer1 || isPressedPlayer2) && hasPressed == false) || (breakableIsPressing && hasPressed == false))
            {
                hasPressed = true;
                typeOneOppositeEffect = true;
                //      Console.WriteLine("action");

                SoundChannel buttonSound = new Sound("button_pressed.wav", false).Play();
                Action();
            }
            //do opposite action if player no longer pressing the button
            if (!isPressedPlayer1 && !isPressedPlayer2 && typeOneOppositeEffect && !breakableIsPressing)
            {
                typeOneOppositeEffect = false;
                ActionOpposite();
                SoundChannel buttonSound = new Sound("button_unpressed.wav", false).Play();
            }
        }

        else
        {
            if (((isPressedPlayer1 || isPressedPlayer2) && hasPressed == false) || (breakableIsPressing && hasPressed == false))
            {
                hasPressed = true;
                SoundChannel buttonSound = new Sound("button_pressed.wav", false).Play();
                Action();
            }
        }
    }
}