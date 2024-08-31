using Godot;
using System;

public abstract partial class Resource
{
	public virtual string Name { get; init; }
	public virtual string Description { get; init; }
	public virtual float MaxPerStack { get; init; }

	public virtual float Amount { get; init; }
}
