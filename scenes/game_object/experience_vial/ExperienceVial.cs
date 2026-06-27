using Godot;
using System;

public partial class ExperienceVial : Node2D
{
    public override void _Ready()
    {
        Area2D areaEntered = GetNode("Area2D") as Area2D;

        areaEntered.Connect("area_entered", new Callable(this, nameof(OnAreaEntered))); // This is where you can initialize any variables or set up the node when it is added to the scene.
    }

    public void OnAreaEntered(Area2D other_area)
    {
        var gameEvents = GetNode<GameEvents>("/root/GameEvents");
        gameEvents.EmitExperienceVialCollected(5);
        QueueFree(); // This will remove the experience vial from the scene after it has been collected.
    }
}
