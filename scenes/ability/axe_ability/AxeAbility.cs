using System;
using Godot;

public partial class AxeAbility : Node2D
{
    public const int maxRadius = 100;

    public HitboxComponent hitBoxcomponent;

    public Vector2 baseRotation = Vector2.Right;

    public override void _Ready()
    {
        baseRotation = Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau));
        var tween = CreateTween();
        hitBoxcomponent = GetNode("HitboxComponent") as HitboxComponent;
        tween.TweenMethod(Callable.From<float>(AxeTweenMethod), 0.0f, 2.0f, 2);
        tween.TweenCallback(Callable.From(() => QueueFree()));
    }

    public void AxeTweenMethod(float _rotations)
    {
        var percent = _rotations / 2;
        var currentRadius = percent * maxRadius;
        var currentDirection = baseRotation.Rotated(_rotations * Mathf.Tau);

        var player = GetTree().GetFirstNodeInGroup("player") as Player;

        if (player == null)
            return;

        GlobalPosition = player.GlobalPosition + (currentDirection * currentRadius);
    }
}
