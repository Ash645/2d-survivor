using Godot;
using System;

public partial class BasicEnemy : CharacterBody2D
{
    private const int MAX_SPEED = 40; //This sets the maximum speed of the enemy.
                                      //private HealthComponent health_component; //This is a reference to the HealthComponent node that is a child of this BasicEnemy node. This allows the enemy to take damage and die when its health reaches zero.

    public Vector2 GetDirectionToPlayer()
    {
        var player_nodes = GetTree().GetFirstNodeInGroup("player") as Node2D;//This gets the first node in the "player" group and casts it to a Node2D. This assumes that there is only one player in the scene. If there are multiple players, you may want to implement a more complex targeting system.
        if (player_nodes != null)
        {
            return (player_nodes.GlobalPosition - GlobalPosition).Normalized(); //This returns the direction vector from the enemy to the player, normalized to a length of 1. This can be used to move the enemy towards the player.
        }
        return Vector2.Zero;
    }

    public override void _Process(double delta)
    {
        var visuals = GetNode("%Visuals") as Node2D;
        var direction = GetDirectionToPlayer(); //This gets the direction to the player.
        Velocity = direction * MAX_SPEED; //This sets the velocity of the enemy to the direction vector multiplied by the maximum speed.
        MoveAndSlide(); //This moves the enemy according to its velocity, and handles collisions with the environment.

        var moveSign = Mathf.Sign(Velocity.X);
        if (moveSign != 0)
            visuals.Scale = new Vector2(moveSign, 1);
    }
}
