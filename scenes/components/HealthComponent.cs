using Godot;
using System;

public partial class HealthComponent : Node
{
    [Signal]
    public delegate void DiedEventHandler();

    [Signal]
    public delegate void HealthChangedEventHandler();

    [Export]
    public float maxHealth = 10;
    public float currentHealth;
    private bool isDead = false;

    public void Damage(float damage_amount)
    {
        if (isDead)
            return;

        currentHealth = Mathf.Max(currentHealth - damage_amount, 0); // This method reduces the current health by the given damage amount. You can call this method whenever the player takes damage, such as from an enemy attack or environmental hazard.
        EmitSignal(SignalName.HealthChanged);
        CallDeferred(nameof(CheckDeath));
    }

    public float GetHealthPercent()
    {
        if (maxHealth <= 0)
            return 0;
        return Mathf.Min(currentHealth/maxHealth,1);
    }
    public override void _Ready()
    {
        currentHealth = maxHealth; // This method is called when the node is added to the scene. It initializes the current health to the maximum health value.
    }

    public void CheckDeath()
    {
        if (isDead || currentHealth > 0)
            return;

        isDead = true;
        EmitSignal(SignalName.Died);
        GetParent().QueueFree();
    }
}
