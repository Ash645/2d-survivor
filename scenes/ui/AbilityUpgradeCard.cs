using Godot;
using System;
using System.ComponentModel;

public partial class AbilityUpgradeCard : PanelContainer
{
    [Signal]
    public delegate void SelectedEventHandler();
    public Label nameLabel;
    public Label descLabel;
    public override void _Ready()
    {
        nameLabel = GetNode("%Name") as Label;
        descLabel = GetNode("%Description") as Label;

        GuiInput += OnGuiInput;
    }

    public void SetAbilityUpgrade(AbilityUpgrade _upgrade)
    {
        nameLabel.Text = _upgrade.name;
        descLabel.Text = _upgrade.description;
    }

    public void OnGuiInput(InputEvent _clickEvent)
    {
        if (_clickEvent.IsActionPressed("Left_Click"))
        {
            EmitSignal(SignalName.Selected);
        }
    }
}
