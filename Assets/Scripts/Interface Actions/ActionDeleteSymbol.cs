using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class ActionDeleteSymbol : Action
{

    public static UnityEvent SymbolDeletedEvent = new UnityEvent();

    public override void Act()
    {
        SymbolDeletedEvent.Invoke();
    }

}
