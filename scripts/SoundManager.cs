using Godot;
using System;

public partial class SoundManager : Node
{
	private AudioStreamPlayer _audioPlayer;

	public static SoundManager Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
		_audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");

		PlaySound("amongas", 1.2f);
	}

	public void PlaySound(string name, float pitch)
	{
		var audioStream = (AudioStream)ResourceLoader.Load(@$"res://sounds/{name}.mp3");
		_audioPlayer.Stream = audioStream;
		_audioPlayer.PitchScale = pitch;
		_audioPlayer.Play();
	}
}