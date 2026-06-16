using Godot;
using System;

public partial class SwordAbility : Node2D
{
    public HitboxComponent     hitboxComponent;
    public override void _Ready()
    {
        hitboxComponent = GetNode("HitboxComponent") as HitboxComponent;
    }

}
