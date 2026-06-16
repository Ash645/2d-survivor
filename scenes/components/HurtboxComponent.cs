using Godot;
using System;

public partial class HurtboxComponent : Area2D
{
    [Export]
    public HealthComponent healthComponent;

    public override void _Ready()
    {
       AreaEntered += OnAreaEntered;
    }

    public void OnAreaEntered(Area2D _otherArea)
    {
        var hitBox = _otherArea as HitboxComponent;
        if (hitBox == null)
            return;

        if (healthComponent == null || !GodotObject.IsInstanceValid(healthComponent))
            return;

        healthComponent.Damage(hitBox.damage);
    }
}
