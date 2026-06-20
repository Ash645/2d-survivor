using Godot;
using System;

public partial class EnemyManager : Node
{
    [Export]
    public PackedScene basic_enemy_scene; // This allows you to assign a PackedScene in the Godot editor, which will be used to instantiate basic enemies.

    [Export]
    public Node ArenaTimeManager;
    const int spawn_radius = 375; // This allows you to set the radius around the player where enemies will spawn in the Godot editor.

    public Timer enemySpawntimer;
    public double baseSpawntime = 0;

    public override void _Ready()
    {
        enemySpawntimer = GetNode("Timer") as Timer; // This gets the Timer node that is a child of this EnemyManager node. You should have a Timer node in your scene tree for this to work.
        enemySpawntimer.Timeout += OnEnemySpawnTimerTimeout; // This connects the Timeout signal of
        baseSpawntime = enemySpawntimer.WaitTime;
        var arenaTimeManager = ArenaTimeManager as ArenaTimeManager;
        arenaTimeManager.ArenaDifficultyIncrease += OnArenaDifficultyIncreased;
    }

    public void OnArenaDifficultyIncreased(int _difficulty)
    {
        var timeOff = (.1 / 12) * _difficulty;
        timeOff = Mathf.Min(timeOff, .7);
        GD.Print(timeOff);
        enemySpawntimer.WaitTime = baseSpawntime - timeOff;
    }

    public Vector2 GetSpawnPosition()
    {
        var player = GetTree().GetFirstNodeInGroup("player") as Node2D;

        if (player == null)
            return Vector2.Zero;

        var spawn_position = Vector2.Zero;
        var random_direction = Vector2.Right.Rotated((float)GD.RandRange(0, Math.Tau));

        for (int i = 0; i < 4; i++)
        {
            // This generates a random direction vector by rotating the right vector by a random angle between 0 and 2*PI radians.
            spawn_position = player.GlobalPosition + (random_direction * spawn_radius); // This calculates the spawn position by adding the random direction vector multiplied by the spawn radius to the player's global position.

            var queryParams = PhysicsRayQueryParameters2D.Create(player.GlobalPosition, spawn_position, 1);
            var result = GetTree().Root.World2D.DirectSpaceState.IntersectRay(queryParams);

            if (result.Count == 0)
                break;
            else
                random_direction = random_direction.Rotated(Mathf.DegToRad(90));
        }
        return spawn_position;
    }

    public void OnEnemySpawnTimerTimeout()
    {
        enemySpawntimer.Start();
        var player = GetTree().GetFirstNodeInGroup("player") as Node2D;

        if (player == null)
            return;

        var enemy = basic_enemy_scene.Instantiate() as Node2D; // This creates an instance of the basic enemy scene and casts it to a Node2D.
        var entities_layer = GetTree().GetFirstNodeInGroup("entities_layer");

        entities_layer.AddChild(enemy); // This adds the enemy instance as a child of the parent node of this EnemyManager. You can change this to add it to a specific node in your scene tree if needed.
        enemy.GlobalPosition = GetSpawnPosition(); // This sets the global position of the enemy instance to
    }
}
