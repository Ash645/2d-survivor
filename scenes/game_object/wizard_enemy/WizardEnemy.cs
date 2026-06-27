using Godot;
using System;

public partial class WizardEnemy : CharacterBody2D
{
    public VelocityComponent velocityComponent;

    public override void _Ready()
    {
        velocityComponent = GetNode("VelocityComponent") as VelocityComponent;
    }


    public override void _Process(double delta)
    {
        if (velocityComponent == null)
            return;

        velocityComponent.AccelerateToPlayer();
        velocityComponent.Move(this);
    }

}
