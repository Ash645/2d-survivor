using Godot;
using System;
using System.Linq;
public partial class SwordAbilityController : Node
{
    [Export]
    public PackedScene sword_ability; // This allows you to assign a PackedScene in the Godot editor, which will be used to instantiate the sword ability when the player uses it.

    public int damage = 5;

    const float max_range = 150f; // This allows you to set the maximum range of the sword ability in the Godot editor.

    public override void _Ready()
    {
        var sword_Timer = GetNode("Timer") as Timer; // This gets the Timer node that is a child of this SwordAbilityController node. You should have a Timer node in your scene tree for this to work.
        sword_Timer.Timeout += OnTimerTimeout; // This connects the Timeout signal of the Timer to the OnTimerTimeout method in this script. This means that when the Timer times out, the OnTimerTimeout method will be called.
    }

    private void OnTimerTimeout()
    {
        var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        if (player == null)
            return;

        var enemies = GetTree().GetNodesInGroup("enemy")
            .OfType<CharacterBody2D>()
            .Where(enemy => enemy.GlobalPosition.DistanceSquaredTo(player.GlobalPosition) < Math.Pow(max_range, 2))
            .OrderBy(enemy => enemy.GlobalPosition.DistanceSquaredTo(player.GlobalPosition))
            .ToList();

        if (enemies.Count == 0)
            return;

        var sword_instance = sword_ability.Instantiate() as SwordAbility;
        player.GetParent().AddChild(sword_instance);
        
        sword_instance.hitboxComponent.damage = damage;
        sword_instance.GlobalPosition = enemies[0].GlobalPosition;
        sword_instance.GlobalPosition += Vector2.Right.Rotated((float)GD.RandRange(0, Math.Tau)) * 4; // This adds a small random offset to the sword's position to make it look more natural.

        var enemy_direction = enemies[0].GlobalPosition - sword_instance.GlobalPosition;
        sword_instance.Rotation = enemy_direction.Angle();
    }
    
    
}
