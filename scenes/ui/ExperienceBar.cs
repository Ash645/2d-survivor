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
        experienceManager.ExperienceUpdated += OnExperienceUpdated;
    }

    public void OnExperienceUpdated(float current_experience, float target_experience)
    {
        var percent = current_experience/target_experience;
        progress_bar.Value = percent;
    }
}
