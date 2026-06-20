using Godot;
using System;

public partial class ArenaTimeManager : Node
{
    [Export]
    public PackedScene EndScene;
    [Signal]
    public delegate void ArenaDifficultyIncreaseEventHandler(int arenaDifficulty);

    private Timer timer;

    public const double difficultyInterval = 5;
    public double arenaDifficulty = 0;
    public double previousTime = 0;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Timeout += OnTimerTimeout;
        previousTime = timer.WaitTime;
    }

    public override void _Process(double delta)
    {
        var nextTimeTarget = timer.WaitTime - ((arenaDifficulty + 1) * difficultyInterval);
        if (timer.TimeLeft <= nextTimeTarget)
        {
            arenaDifficulty += 1;
            EmitSignal(SignalName.ArenaDifficultyIncrease, arenaDifficulty);
        }
    }

    public float GetElapsedTime()
    {
        var player = GetTree().GetFirstNodeInGroup("player") as Node2D;

        if (player != null)
        {
            return (float)(timer.WaitTime - timer.TimeLeft);
        }
        var timeOfDeath = (float)(timer.WaitTime - timer.TimeLeft);
        timer.Stop();
        return timeOfDeath;
    }

    public void OnTimerTimeout()
    {
        var endSceneInstanciate = EndScene.Instantiate();
        AddChild(endSceneInstanciate);
    }
}