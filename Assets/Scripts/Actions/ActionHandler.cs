using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler
{
    public BaseAction ActionToHandle { get; set; }

    public void ExecuteCommand()
    {
        ActionToHandle.Execute();
    }
}
