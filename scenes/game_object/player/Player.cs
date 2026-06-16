using Godot;
using System;

public partial class Player : CharacterBody2D
{
    const int MAX_SPEED = 125; //This sets the maximum speed of the player.
    const int ACCELERATION_SMOOTHING = 20; //This sets the smoothing factor for acceleration, which controls how quickly the player reaches maximum speed. A higher value will make the player accelerate faster, while a lower value will make it accelerate more slowly.
    
    public override void _Process(double delta)
    {
        var movement_vector = GetMovementVector().Normalized(); //This gets the movement vector from the input.
        var target_velocity = movement_vector * MAX_SPEED; //This sets the velocity of the player to the movement vector multiplied by the maximum speed.
        Velocity = Velocity.Lerp(target_velocity, (float)(1.0f - Math.Exp(-delta * ACCELERATION_SMOOTHING))); //This smoothly interpolates the player's velocity towards the target velocity using an exponential function to create a smooth acceleration effect.
        MoveAndSlide();
    }

    private Vector2 GetMovementVector()
    {
        //var movement_vector = Vector2.Zero; //This sets the movement vector to zero, so that the player doesn't move if no input is detected.
        var x_movement = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"); //This gets the strength of the "move_right" and "move_left" actions, and subtracts them to get the horizontal movement.
        var y_movement = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up"); //This gets the strength of the "move_down" and "move_up" actions, and subtracts them to get the vertical movement.
        return new Vector2(x_movement, y_movement).Normalized(); //This normalizes the movement vector, so that the player moves at the same speed in all directions.
    }
}
