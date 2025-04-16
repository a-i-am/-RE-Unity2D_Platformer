using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InputManager : Singleton<InputManager>
{
    public Action KeyAction = null;

    private List<IUpdatable> _updateableObjectList = new List<IUpdatable>();
    
    void Update()
    {
        for (int i = 0; i < Instance._updateableObjectList.Count; ++i)
        {
            Instance._updateableObjectList[i].OnUpdate();
        }

        if (Input.anyKey == false)
            return;

        if (KeyAction != null)
            KeyAction.Invoke();
    }

    public void RegisterUpdateablObject(IUpdatable obj)
    {
        if (!Instance._updateableObjectList.Contains(obj))
        {
            Instance._updateableObjectList.Add(obj);
        }
    }

    public void DeregisterUpdateableObject(IUpdatable obj)
    {
        if (Instance._updateableObjectList.Contains(obj))
        {
            Instance._updateableObjectList.Remove(obj);
        }
    }
}
