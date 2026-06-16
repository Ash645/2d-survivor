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
        return (float)(timer.WaitTime - timer.TimeLeft);
    }

}