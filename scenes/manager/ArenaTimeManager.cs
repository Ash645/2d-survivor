using Godot;
using System;

public partial class ArenaTimeManager : Node
{
    private Timer timer;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
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

}