﻿using UnityEngine;

public class InputAction
{
    private KeyCode primaryKey;
    private KeyCode secondaryKey;
    
    public InputAction(KeyCode primaryKey, KeyCode secondaryKey = KeyCode.None)
    {
        this.primaryKey   = primaryKey;
        this.secondaryKey = secondaryKey;
    }

    public bool Start   { get => Input.GetKeyDown(primaryKey) || Input.GetKeyDown(secondaryKey); }
    public bool Perform { get => Input.GetKey(primaryKey)     || Input.GetKey(secondaryKey); }
    public bool Cancel  { get => Input.GetKeyUp(primaryKey)   || Input.GetKeyUp(secondaryKey); }

    public void ChangePrimaryBinding(KeyCode newKey) => primaryKey = newKey;
    public void ChangeSecondaryBinding(KeyCode newKey) => secondaryKey = newKey;
}