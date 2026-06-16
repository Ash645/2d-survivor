using Godot;
using System;

public partial class HealthComponent : Node
{
    [Signal]
    public delegate void DiedEventHandler();

    [Export]
    public float max_health = 10;
    public float current_health;
    private bool is_dead = false;

    public void Damage(float damage_amount)
    {
        if (is_dead)
            return;

        current_health = Mathf.Max(current_health - damage_amount, 0); // This method reduces the current health by the given damage amount. You can call this method whenever the player takes damage, such as from an enemy attack or environmental hazard.
        CallDeferred(nameof(CheckDeath));
    }
    public override void _Ready()
    {
        current_health = max_health; // This method is called when the node is added to the scene. It initializes the current health to the maximum health value.
    }

    public void CheckDeath()
    {
        if (is_dead || current_health > 0)
            return;

        is_dead = true;
        EmitSignal(SignalName.Died);
        QueueFree();
    }
}
