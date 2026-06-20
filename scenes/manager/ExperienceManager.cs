using Godot;
using Godot.Collections;
using System;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;

public partial class ExperienceManager : Node
{
    [Signal]
    public delegate void ExperienceUpdatedEventHandler(float currentExperience, float taragetExperience);

    [Signal]
    public delegate void LevelUpEventHandler(int newLevel);

    public float currentExperience = 0;
    public float currentLevel = 1;

    public const float targetExperienceGrowth = 5;
    public float taragetExperience = 1;

    public void IncrementExperience(float experience)
    {
        currentExperience = Mathf.Min(currentExperience + experience, taragetExperience); //
        //GD.Print(currentExperience);// This method increments the current experience by the given amount. You can call this method whenever the player gains experience, such as after defeating an enemy or completing a task.
        EmitSignal(SignalName.ExperienceUpdated, currentExperience, taragetExperience);
        if (currentExperience == taragetExperience)
        {
            currentLevel += 1;
            taragetExperience += targetExperienceGrowth;
            currentExperience = 0;
            EmitSignal(SignalName.ExperienceUpdated, currentExperience, taragetExperience);
            EmitSignal(SignalName.LevelUp, currentLevel);
        }
    }

    public override void _Ready()
    {
        var gameEvents = GetNode<GameEvents>("/root/GameEvents");
        gameEvents.ExperienceVialCollected += OnExperienceVialCollected;
    }

    /// <summary>
    /// This removes the active or outdated signal listener from the autoload GameEvents
    /// So that when game is restarteed, the GameEvent can listen to the new instance of ExperienceManager
    /// 
    /// This is because, ExperienceManager is the only object listening to GameEvent in it's _Ready() method
    /// </summary>
    public override void _ExitTree()
    {
        if (IsInstanceValid(GetNode<GameEvents>("/root/GameEvents")))
        {
            var gameEvents = GetNode<GameEvents>("/root/GameEvents");
            gameEvents.ExperienceVialCollected -= OnExperienceVialCollected;
        }
    }

    public float OnExperienceVialCollected(float experience)
    {
        if (IsQueuedForDeletion())
        {
            return experience;
        }

        IncrementExperience(experience);
        return experience;
    }
}
