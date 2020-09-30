using Godot;
using System;
using System.Diagnostics;

public class Charachter : KinematicBody2D
{
	public Vector2 velocity = new Vector2(0, 0);
	[Export] public float gravity;
	[Export] public Vector2 speed = new Vector2(0, 0);

	



	/*public override void _PhysicsProcess(float delta)
	{
		velocity.y += gravity * delta;
		velocity.y = Math.Max(velocity.y, speed.y);
		//float horizontalDirection = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
		//velocity.x = horizontalDirection * speed.x;
		velocity = MoveAndSlide(velocity);
	}*/

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
	
