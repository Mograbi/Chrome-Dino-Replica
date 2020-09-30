using Godot;
using System;

public class ScrollingBackground : Node2D
{
	static public bool moving = true;
	private Sprite[] m_backgrounds = new Sprite[3];
	private float m_width = 584;
	private float m_minX = -440;

	[Export]
	public float scrollingSpeed = 400f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i = 1; i <= 3; i++)
		{
			m_backgrounds[i - 1] = GetNode<Sprite>("Sprite" + i);
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (moving)
		{
			foreach (Sprite sprite in m_backgrounds)
			{
				Vector2 tmp = sprite.Position;
				tmp.x -= scrollingSpeed * delta;
				if (tmp.x < m_minX)
				{
					tmp.x += m_width * m_backgrounds.Length;
				}
				sprite.Position = tmp;
			}
		}
		
	}
}
