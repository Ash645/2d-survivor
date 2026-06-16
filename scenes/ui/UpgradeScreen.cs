using Godot;
using System;

public partial class UpgradeScreen : CanvasLayer
{
    [Export]
    public PackedScene UpgradeCardScene;

    [Signal]
    public delegate void UpgradeSelectedEventHandler(AbilityUpgrade upgrdade);

    public HBoxContainer cardContainer;

    public override void _Ready()
    {
        cardContainer = GetNode("%CardContainer") as HBoxContainer;
        GetTree().Paused = true;
    }

    public void SetAbilityUpgrades(Godot.Collections.Array<AbilityUpgrade> _abilityUpgrades)
    {
        foreach (var upgrade in _abilityUpgrades)
        {
            var cardInstance = UpgradeCardScene.Instantiate<AbilityUpgradeCard>();
            cardContainer.AddChild(cardInstance);
            cardInstance.SetAbilityUpgrade(upgrade);
            cardInstance.Selected += () => OnUpgradeSelected(upgrade);
        }

    }

    public void OnUpgradeSelected(AbilityUpgrade _upgrade)
    {
        EmitSignal(SignalName.UpgradeSelected,_upgrade);
        GetTree().Paused = false;
        QueueFree();
    }
}
