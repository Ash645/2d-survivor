using Godot;
using System;

public partial class Main : Node
{
    [Export]
    public PackedScene EndScene;

    public Player player;

    public override void _Ready()
    {
        player = GetNode("%player") as Player;
        player.healthComponent.Died += OnPlayerDied;  
    }

    public void OnPlayerDied()
    {
        var endScreenInstance = EndScene.Instantiate() as EndScene;
        AddChild(endScreenInstance);
        endScreenInstance.SetDefeat();
    }
}
