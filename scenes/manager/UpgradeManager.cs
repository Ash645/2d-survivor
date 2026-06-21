using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
        gameEvents.EmitAbilityUpgradeAdded(_upgrade, currentUpgrades);
        GD.Print(currentUpgrades);
    }

    public Godot.Collections.Array<AbilityUpgrade> PickUpgrades()
    {
        var filteredUpgrades = upgradePool.Duplicate();
        var finalChosenUpgrades = new Godot.Collections.Array<AbilityUpgrade>();

        for (int i = 1; i <= 2; i++)
        {
            var selectedUpgrade = filteredUpgrades.PickRandom() as AbilityUpgrade;
            finalChosenUpgrades.Add(selectedUpgrade);
            filteredUpgrades = new Godot.Collections.Array<AbilityUpgrade>(filteredUpgrades.Where(upgrade => upgrade.id != selectedUpgrade.id));
        }
        return finalChosenUpgrades;
    }

    public void OnUpgradeSelected(AbilityUpgrade _upgrade)
    {
        applyUpgrade(_upgrade);
    }

    public void OnLevelUp(int currentLevel)
    {
        var upgradeScreenInstance = upgradeScreenScene.Instantiate<UpgradeScreen>();
        AddChild(upgradeScreenInstance);

        var finalChosenUpgrades = PickUpgrades();
        upgradeScreenInstance.SetAbilityUpgrades(finalChosenUpgrades);
        upgradeScreenInstance.UpgradeSelected += OnUpgradeSelected;
    }
}
