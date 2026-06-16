using Godot;
using System;

public partial class EnemyManager : Node
{
    [Export]
    public PackedScene basic_enemy_scene; // This allows you to assign a PackedScene in the Godot editor, which will be used to instantiate basic enemies.

    const int spawn_radius = 375; // This allows you to set the radius around the player where enemies will spawn in the Godot editor.

    public override void _Ready()
    {
        Timer enemy_spawn_timer = GetNode("Timer") as Timer; // This gets the Timer node that is a child of this EnemyManager node. You should have a Timer node in your scene tree for this to work.
        enemy_spawn_timer.Timeout += OnEnemySpawnTimerTimeout; // This connects the Timeout signal of
    }

    public void OnEnemySpawnTimerTimeout()
    {
        var player = GetTree().GetFirstNodeInGroup("player") as Node2D;

        if (player == null)
            return;

        var random_direction = Vector2.Right.Rotated((float)GD.RandRange(0, Math.Tau)); // This generates a random direction vector by rotating the right vector by a random angle between 0 and 2*PI radians.
        var spawn_position = player.GlobalPosition + (random_direction * spawn_radius); // This calculates the spawn position by adding the random direction vector multiplied by the spawn radius to the player's global position.

        var enemy = basic_enemy_scene.Instantiate() as Node2D; // This creates an instance of the basic enemy scene and casts it to a Node2D.
        GetParent().AddChild(enemy); // This adds the enemy instance as a child of the parent node of this EnemyManager. You can change this to add it to a specific node in your scene tree if needed.
        enemy.GlobalPosition = spawn_position; // This sets the global position of the enemy instance to
    }
}
