using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//performs a action like (platforms appearing, doors opening, etc.) from subclasses of this class 
public class Effect
{
    public Effect()
    {
    }

    public virtual void TryAction()
    {
    }

    public virtual void TryActionOpposite()
    {
    }
}

