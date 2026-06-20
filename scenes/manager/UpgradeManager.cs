using Godot;
using Godot.Collections;

public partial class UpgradeManager : Node
{
    [Export]
    public Godot.Collections.Array<AbilityUpgrade> upgradePool;

    [Export]
    public ExperienceManager ExperienceManager;

    [Export]
    public PackedScene upgradeScreenScene;

    public Godot.Collections.Dictionary currentUpgrades = new Godot.Collections.Dictionary();

    public override void _Ready()
    { 
        if (ExperienceManager != null)
        {
            ExperienceManager.LevelUp += OnLevelUp;
        }
    }

    public override void _ExitTree()
    {
        if (ExperienceManager != null)
        {
            ExperienceManager.LevelUp -= OnLevelUp;
        }
    }

    public void OnLevelUp(int currentLevel)
    {
        var selectedUpgrade = upgradePool.PickRandom();
        if (selectedUpgrade == null)
            return;

        var packedUpgrades = new Array<AbilityUpgrade>();
        packedUpgrades.Add(selectedUpgrade);

        var upgradeScreenInstance = upgradeScreenScene.Instantiate<UpgradeScreen>();
        AddChild(upgradeScreenInstance);
        upgradeScreenInstance.SetAbilityUpgrades(packedUpgrades);
        upgradeScreenInstance.UpgradeSelected += OnUpgradeSelected;
    }

    public void applyUpgrade(AbilityUpgrade _upgrade)
    {
        Dictionary addChosenUpgrade;
        if (!currentUpgrades.ContainsKey(_upgrade.id))
        {
            addChosenUpgrade = new Dictionary();
            addChosenUpgrade["resource"] = _upgrade;
            addChosenUpgrade["quantity"] = 1;
        }
        else
        {
            addChosenUpgrade = (Dictionary)currentUpgrades[_upgrade.id];
            addChosenUpgrade["quantity"] = (int)addChosenUpgrade["quantity"] + 1;            
        }
        currentUpgrades[_upgrade.id] = addChosenUpgrade;
        var gameEvents = GetNode<GameEvents>("/root/GameEvents");
        gameEvents.EmitAbilityUpgradeAdded(_upgrade,currentUpgrades);
        GD.Print(currentUpgrades);
    }

    public void OnUpgradeSelected(AbilityUpgrade _upgrade)
    {
        applyUpgrade(_upgrade);
    }
}
