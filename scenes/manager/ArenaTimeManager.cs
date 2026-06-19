using Godot;
using System;

public partial class ArenaTimeManager : Node
{
    [Export]
    public PackedScene VictoryScene;
    private Timer timer;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Timeout += OnTimerTimeout;
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
        var victorySceneInstance = VictoryScene.Instantiate();
        AddChild(victorySceneInstance);
    }
}