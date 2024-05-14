using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class Button : AnimationSpriteCustom
{
    int theType;
    //1 --> The first type will reverse whatever effect it had, when there is no player colliding with it
    //2 --> the other type’s effects will stay indefinitely after a player has collided with it.

    List<Effect> effects = new List<Effect>();

    //tiled to get the action name, action parameter A, action parameter B


    public Button(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
    }

    //do all effects the button has
    public void addEffect()
    {
        foreach (Effect effect in effects)
        {
            effect.tryAction();
        }
    }

    public void AddEffect(Effect theEffect)
    {

    }

}