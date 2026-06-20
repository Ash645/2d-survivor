using Godot;
using System;

public partial class ExperienceBar : CanvasLayer
{
    [Export]
    public ExperienceManager experienceManager;

    public ProgressBar progress_bar;

    public override void _Ready()
    {
        progress_bar = GetNode("MarginContainer/ProgressBar") as ProgressBar;
        progress_bar.Value = 0;

        if (experienceManager != null)
        {
            experienceManager.ExperienceUpdated += OnExperienceUpdated;
        }
    }
    /// <summary>
    /// This explicitly removes the signal being listed from Experience manager
    /// so that it can listen to a new object when the main scene is resstarted.
    /// </summary>
    public override void _ExitTree()
    {
        if (experienceManager != null)
        {
            experienceManager.ExperienceUpdated -= OnExperienceUpdated;
        }
    }

    public void OnExperienceUpdated(float current_experience, float target_experience)
    {
        if (progress_bar == null || IsInstanceValid(progress_bar) == false)
        {
            return;
        }

        var percent = current_experience / target_experience;
        progress_bar.Value = percent;
    }
}
