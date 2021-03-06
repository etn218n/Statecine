﻿using NodeCanvas;

public class CrouchState : State
{
    private readonly Character2D character;

    public CrouchState(Character2D character)
    {
        this.character = character;
    }
        
    public override void OnEnter()
    {
        character.Stop();
        character.PlayCrouchAnimation();
        character.UpperBodyCollider.isTrigger = true;
    }

    public override void OnExit()
    {
        character.UpperBodyCollider.isTrigger = false;
    }
}