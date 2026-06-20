using Godot;
using System;

public partial class AxeAbilityController : Node
{
    [Export]
    public PackedScene AxeAbilityScene;

    public int damage = 10;

    public Timer axetimer;

    public override void _Ready()
    {
        axetimer = GetNode("Timer") as Timer;
        axetimer.Timeout += OnTimertimeOut;
    }

    public void OnTimertimeOut()
    {
        var player = GetTree().GetFirstNodeInGroup("player") as Player;

        if (player == null)
            return;

        var foreGround = GetTree().GetFirstNodeInGroup("foreground_layer") as Node2D;

        if (foreGround == null)
            return;

        var axeAbilitySceneInstance = AxeAbilityScene.Instantiate() as AxeAbility;

        foreGround.AddChild(axeAbilitySceneInstance);
        axeAbilitySceneInstance.GlobalPosition = player.GlobalPosition;
        axeAbilitySceneInstance.hitBoxcomponent.damage = damage;
    }

}
