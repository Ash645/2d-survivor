using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class SwordAbilityController : Node
{
    [Export]
    public PackedScene swordAbility; // This allows you to assign a PackedScene in the Godot editor, which will be used to instantiate the sword ability when the player uses it.

    public int damage = 5;

    public double baseWaitTime;

    public Timer swordTimer;

    const float maxRange = 150f; // This allows you to set the maximum range of the sword ability in the Godot editor.

    public override void _Ready()
    {
        swordTimer = GetNode("Timer") as Timer; // This gets the Timer node that is a child of this SwordAbilityController node. You should have a Timer node in your scene tree for this to work.
        baseWaitTime = swordTimer.WaitTime;
        swordTimer.Timeout += OnTimerTimeout; // This connects the Timeout signal of the Timer to the OnTimerTimeout method in this script. This means that when the Timer times out, the OnTimerTimeout method will be called.
        var gameEvents = GetNode<GameEvents>("/root/GameEvents");
        gameEvents.AbilityUpgradeAdded += OnAbilityUpgradeAdded;
    }

    public void OnAbilityUpgradeAdded(AbilityUpgrade _upgrade, Godot.Collections.Dictionary _currentUpgrades)
    {
        if (_upgrade.id != "sword_rate")
            return;

        if (!_currentUpgrades.ContainsKey(_upgrade.id))
            return;

        var swordRateDict = (Godot.Collections.Dictionary)_currentUpgrades[_upgrade.id];

        if (!swordRateDict.TryGetValue("quantity", out var quantityObj))
            return;

        float quantity = (float)quantityObj.AsDouble();
        var percentReduction = quantity * 0.1f;
        swordTimer.WaitTime = baseWaitTime * (1 - percentReduction);
        swordTimer.Start();
        GD.Print(swordTimer.WaitTime);
    }
    private void OnTimerTimeout()
    {
        var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        if (player == null)
            return;

        var enemies = GetTree().GetNodesInGroup("enemy")
            .OfType<CharacterBody2D>()
            .Where(enemy => enemy.GlobalPosition.DistanceSquaredTo(player.GlobalPosition) < Math.Pow(maxRange, 2))
            .OrderBy(enemy => enemy.GlobalPosition.DistanceSquaredTo(player.GlobalPosition))
            .ToList();

        if (enemies.Count == 0)
            return;

        var swordInstance = swordAbility.Instantiate() as SwordAbility;
        var foreground_layer = GetTree().GetFirstNodeInGroup("foreground_layer");
        foreground_layer.AddChild(swordInstance);
        
        swordInstance.hitboxComponent.damage = damage;
        swordInstance.GlobalPosition = enemies[0].GlobalPosition;
        swordInstance.GlobalPosition += Vector2.Right.Rotated((float)GD.RandRange(0, Math.Tau)) * 4; // This adds a small random offset to the sword's position to make it look more natural.

        var enemy_direction = enemies[0].GlobalPosition - swordInstance.GlobalPosition;
        swordInstance.Rotation = enemy_direction.Angle();
    }
    
    
}
