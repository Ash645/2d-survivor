using Godot;

public partial class GameEvents : Node
{
    [Signal]
    public delegate float ExperienceVialCollectedEventHandler(float experience); // This signal can be emitted when an experience vial is collected. You can connect this signal to any method that should be called when an experience vial is collected, such as incrementing the player's experience or updating the UI. 

    [Signal]
    public delegate void AbilityUpgradeAddedEventHandler(AbilityUpgrade _upgrade,Godot.Collections.Dictionary _currentUpgrades);
    public void EmitExperienceVialCollected(float experience)
    {
        EmitSignal(SignalName.ExperienceVialCollected, experience); // This method emits the experience_vial_collected signal with the given experience value. You can call this method from the on_area_entered method in the ExperienceVial class when an experience vial is collected.
    }

    public void EmitAbilityUpgradeAdded(AbilityUpgrade _upgrade,Godot.Collections.Dictionary _currentUpgrades)
    {
        EmitSignal(SignalName.AbilityUpgradeAdded,_upgrade,_currentUpgrades);
    }
}
