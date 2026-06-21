using Godot;
using System;

public partial class AbilityUpgrade : Resource
{
    [Export]
    public string id;

    [Export]
    public string name;

    [Export(PropertyHint.MultilineText)]
    public string description;
}
