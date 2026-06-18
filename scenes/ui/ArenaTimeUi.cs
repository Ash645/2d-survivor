using Godot;
using System;

public partial class ArenaTimeUi : CanvasLayer
{
    [Export]
    public ArenaTimeManager arena_time_manager;

    public Label timer_label;

    public override void _Ready()
    {
        timer_label = GetNode("%Label") as Label; // This gets the Label node that is a child of this ArenaTimeUi node. You should have a Label node in your scene tree for this to work.
    }
    public override void _PhysicsProcess(double delta)
    {
        var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        if (arena_time_manager == null ||
                player == null)
            return;
        var time_elapsed = arena_time_manager.GetElapsedTime();
        timer_label.Text = format_seconds_to_string(time_elapsed); // This formats the elapsed time to 2 decimal places and appends "s" for seconds.
    }

    public string format_seconds_to_string(float seconds)
    {
        var minutes = Mathf.Floor(seconds / 60);
        var remaining_seconds = seconds - (minutes * 60);
        return minutes.ToString("0") + ":" + Mathf.Floor(remaining_seconds).ToString("00"); // This formats the time as MM:SS.
    }
}
