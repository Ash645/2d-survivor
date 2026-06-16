using Godot;
using System;
using System.Linq;

public partial class GameCamera : Camera2D
{
    Vector2 target_position = Vector2.Zero;
    public override void _Ready()
    {
        MakeCurrent();
    }

    public override void _Process(double delta)
    {
        acquireTarget();
        GlobalPosition = GlobalPosition.Lerp(target_position, (float)(1.0f - Math.Exp(-delta * 20.0f)));//This sets the global position of the camera to a linear interpolation between the current global position and the target position, with a smoothing factor based on the delta time. The exponential function is used to create a smooth transition that slows down as it approaches the target position.
    }

    public void acquireTarget()
    {
        var player_nodes = GetTree().GetNodesInGroup("player");//This gets all nodes in the scene that are in the "player" group. You can assign nodes to this group in the Godot editor. This allows the camera to find the player node(s) without needing a direct reference.
        if (player_nodes.Count() >0)
        {
            var player = player_nodes[0] as Node2D;//This gets the first node in the "player" group and casts it to a Node2D. This assumes that there is only one player in the scene. If there are multiple players, you may want to implement a more complex targeting system.
            target_position = player.GlobalPosition;//This sets the target position to the global position of the player. The camera will then move towards this target position in the _Process method.
        }
    }
}
