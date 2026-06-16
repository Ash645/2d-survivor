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
        EmitSignal(SignalName.ExperienceUpdated,currentExperience,taragetExperience);
        if (currentExperience == taragetExperience)
        {
            currentLevel +=1;
            taragetExperience += targetExperienceGrowth;
            currentExperience = 0;
            EmitSignal(SignalName.ExperienceUpdated,currentExperience,taragetExperience);
            EmitSignal(SignalName.LevelUp, currentLevel);
        }
    }

    public override void _Ready()
    {
        var gameEvents = GetNode<GameEvents>("/root/GameEvents");
        gameEvents.ExperienceVialCollected += OnExperienceVialCollected; // This connects the experience_vial_collected signal from the GameEvents auto-load to the OnExperienceVialCollected method in this ExperienceManager. This allows the ExperienceManager to respond whenever an experience vial is collected.
    }

    public float OnExperienceVialCollected(float experience)
    {
        IncrementExperience(experience); // This method is called when the experience_vial_collected signal is emitted. It takes the experience value from the signal and calls the IncrementExperience method to update the current experience.
        return experience;
    }
}
