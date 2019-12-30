using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ActionChooseSymbol : Action
{

    public static CustomEvent<char> SymbolChosenEvent = new CustomEvent<char>();

    public char Symbol;

    public override void Act()
    {
        SymbolChosenEvent.Invoke(Symbol);
    }

}
