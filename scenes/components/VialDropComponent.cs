using Godot;
using System;

public partial class VialDropComponent : Node2D
{
    [Export(PropertyHint.Range,"0,1")]    
    public float drop_percent = 0.5f;

    [Export]
    public PackedScene experience_vial_scene; // This is a reference to the PackedScene resource
    [Export]
    HealthComponent health_component; // This is a reference to the HealthComponent node that is a child of this VialDropComponent node. This allows the vial drop component to check the health of the enemy and drop an experience vial when the enemy dies.

    public override void _Ready()
    {
        health_component.Died += OnDied; // This connects the Died signal of the HealthComponent to the OnEnemyDied method in this script. This means that when the enemy dies, the OnEnemyDied method will be called.
    }

    public void OnDied()
    {
        if (GD.Randf() > drop_percent && experience_vial_scene != null) // Only spawn a vial when the roll succeeds and a scene is assigned.
        {
            var spawn_position = GlobalPosition; // This gets the global position of the enemy, which will be used as the spawn position for the experience vial.
            var vial_instance = experience_vial_scene.Instantiate() as Node2D; // This creates an instance of the experience vial scene, which will be the actual vial that is dropped in the game world.

            if (vial_instance != null)
            {
                GetTree().CurrentScene.AddChild(vial_instance); // Add the vial to the active scene so it does not get freed with the enemy.
                vial_instance.GlobalPosition = spawn_position; // This sets the global position of the vial instance to the spawn position, which is the position of the enemy when it died. This will make the
            }
        }

        GetParent().QueueFree();
    }
}
