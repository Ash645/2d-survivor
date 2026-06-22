using System;
using System.Collections.Generic;
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
        Dictionary addChosenUpgrade = new Dictionary();

        if (!currentUpgrades.ContainsKey(_upgrade.id))
        {
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

        CheckAbilityMaxQtyReached(currentUpgrades, _upgrade);
        GD.Print(currentUpgrades);
    }

    public void CheckAbilityMaxQtyReached(Godot.Collections.Dictionary _currentUpgrades, AbilityUpgrade _upgrade)
    {
        var upgradeData = (Dictionary)_currentUpgrades[_upgrade.id];
        var quantity = (uint)upgradeData["quantity"];
        if (quantity == _upgrade.maxQuantity)
        {
            upgradePool = new Godot.Collections.Array<AbilityUpgrade>(upgradePool.Where(poolUpgrade => poolUpgrade.id != _upgrade.id));
        }
    }

    public Godot.Collections.Array<AbilityUpgrade> PickUpgrades()
    {
        var filteredUpgrades = upgradePool.Duplicate();
        var finalChosenUpgrades = new Godot.Collections.Array<AbilityUpgrade>();

        for (int i = 1; i <= 2; i++)
        {
            if (filteredUpgrades.Count == 0)
            {
                break;
            }
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
        if (upgradePool.Count == 0)
        {
            return;
        }
        var upgradeScreenInstance = upgradeScreenScene.Instantiate<UpgradeScreen>();
        AddChild(upgradeScreenInstance);

        var finalChosenUpgrades = PickUpgrades();
        upgradeScreenInstance.SetAbilityUpgrades(finalChosenUpgrades);
        upgradeScreenInstance.UpgradeSelected += OnUpgradeSelected;
    }
}
