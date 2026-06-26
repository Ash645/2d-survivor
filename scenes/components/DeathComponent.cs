using Godot;
using System;

public partial class DeathComponent : Node2D
{
    [Export(PropertyHint.NodePathToEditedNode)]
    public HealthComponent healthComponent;

    public AnimationPlayer animationPlayer;

    [Export]
    public Sprite2D sprite;

    public GpuParticles2D gpuParticles2D;

    public override void _Ready()
    {
        gpuParticles2D = GetNode("%GPUParticles2D") as GpuParticles2D;
        gpuParticles2D.Texture = sprite.Texture;

        if (healthComponent == null || !GodotObject.IsInstanceValid(healthComponent))
            return;

        healthComponent.Died += OnDied;
    }
    public void OnDied()
    {
        var owner = Owner as Node2D;

        if (owner == null || owner is not Node2D)
            return;

        var spawnPosition = owner.GlobalPosition;

        var entities = GetTree().GetFirstNodeInGroup("entities_layer");
        GetParent().RemoveChild(this);
        entities.AddChild(this);

        this.GlobalPosition = spawnPosition;

        animationPlayer = GetNode("%DeathAnimation") as AnimationPlayer;
        animationPlayer.Play("default");
    }
}
