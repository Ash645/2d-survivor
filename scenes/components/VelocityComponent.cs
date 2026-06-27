using Godot;
using System;

public partial class VelocityComponent : Node
{
    [Export]
    public int maxSpeed = 40;

    [Export]
    public float acceleration = 5;

    Vector2 velocity = Vector2.Zero;

    public void Move(CharacterBody2D _characterBody)
    {
        _characterBody.Velocity = velocity;
        _characterBody.MoveAndSlide();
        velocity = _characterBody.Velocity;
    }

    public void AccelerateInDirection(Vector2 _direction)
    {
        var targetVelocity = _direction * maxSpeed;
        velocity = velocity.Lerp(targetVelocity, (float)(1.0f - System.Math.Exp(-acceleration * GetProcessDeltaTime())));
    }

    public void AccelerateToPlayer()
    {
        var ownerNode2d = Owner as Node2D;

        if (ownerNode2d == null)
            return;

        var player = GetTree().GetFirstNodeInGroup("player") as Player;
        if (player == null)
            return;

        var direction = (player.GlobalPosition - ownerNode2d.GlobalPosition).Normalized();
        AccelerateInDirection(direction);
    }
}
