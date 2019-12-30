using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class ActionSubmit : Action
{

    public static UnityEvent SubmitEvent = new UnityEvent();

    public override void Act()
    {
        SubmitEvent.Invoke();
    }

}
