using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public Action keyAction = null;
    public void OnUpdate()
    {
        if (Input.anyKey == false) return;

        if(keyAction != null)
        {
            keyAction.Invoke();
        }
    }
}
