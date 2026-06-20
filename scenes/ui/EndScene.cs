using Godot;
using System;

public partial class EndScene : CanvasLayer
{
    public Button restartButton;
    public Button quitButton;
    public override void _Ready()
    {
        GetTree().Paused = true;
        restartButton = GetNode("%RestartButton") as Button;
        quitButton = GetNode("%QuitButton") as Button;
        restartButton.Pressed += OnRestartButtonPressed;
        quitButton.Pressed += OnQuitButtonPressed;
    }

    public void SetDefeat()
    {
        var titleLabel = GetNode("%TitleLabel") as Label;
        var descriptionLabel = GetNode("%DescriptionLabel") as Label;

        titleLabel.Text ="Defeated!";
        descriptionLabel.Text = "You lost!";
    }

    public void OnRestartButtonPressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://scenes/main/main.tscn");
    }

    public void OnQuitButtonPressed()
    {
        GetTree().Quit();
    } 
}
