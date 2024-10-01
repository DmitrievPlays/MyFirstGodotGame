using System;
using Godot;

public abstract partial class Machine : StaticBody3D, IHavePermanentId
{
    public Guid Guid { get; set; } = Guid.NewGuid();

    public abstract void OnInteract();
}
