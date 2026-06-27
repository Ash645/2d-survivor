using Godot;
using System;

public partial class VisualComponent : Node2D
{
    public CharacterBody2D parentGameObject;

    public override void _Ready()
    {
        parentGameObject = GetParent() as CharacterBody2D;
    }

    public override void _Process(double delta)
    {
        if (parentGameObject == null)
            return;

        var moveSign = Mathf.Sign(parentGameObject.Velocity.X);
        if (moveSign != 0)
            this.Scale = new Vector2(moveSign, 1);
    }

}
