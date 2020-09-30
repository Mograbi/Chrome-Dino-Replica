using Godot;
using System;
using System.Diagnostics;

public enum EnemyType
{
	CACTUS,
	FLYING
};
public class Enemy : Charachter
{
	[Export]
	public EnemyType type;
	static public bool moving = true;
	static public Vector2 cactusInitialPosition = new Vector2(1040, 560);
	static public Vector2 FlyingInitialPosition = new Vector2(1030, 500);
	
	static public new float speed = 0;
	public float Speed
	{
		get { return speed; }
		set { speed = value; }
	}

	public override void _Ready()
	{
		Debug.Print("Enemy Ready");

		var area = GetChild<Area2D>(0);
		if (area != null)
		{
			area.Connect("area_entered", this, "StopAnimation");
			Debug.Print("Connected");
		}
	}
	public override void _PhysicsProcess(float delta)
	{
		if (moving)
		{
			velocity.y = 0;
			velocity.x = -1 * speed * 2.67f;
			velocity = MoveAndSlide(velocity);
		}
		
	}

	static public void Stop()
	{
		moving = false;
	}

	public void StopAnimation(Godot.Object obj)
	{
		var animation = GetNode<AnimatedSprite>("Animation");
		if (animation != null)
		{
			animation.Playing = false;
		}
	}

	static public Enemy Spawn(EnemyType type)
	{
		PackedScene enemyScene;
		Enemy enemyInstance;
		if (type == EnemyType.CACTUS)
		{
			enemyScene = GD.Load<PackedScene>("res://Scene/Enemies/Enemy.tscn");
			

			Random ran = new Random();
			String spriteNum = ((ran.Next() % 4) + 1).ToString();
			Texture enemyTexture = GD.Load<Texture>(($"res://Chrome Dino Game Sprites/CACTUS{spriteNum}.png"));
			Debug.Print($"res://Chrome Dino Game Sprites/CACTUS{spriteNum}.png");

			enemyInstance = (Enemy)enemyScene.Instance();
			var sprite = enemyInstance.GetNode<Sprite>("Sprite");
			sprite.Texture = enemyTexture;
			enemyInstance.Position = Enemy.cactusInitialPosition;
		}
		else
		{
			enemyScene = GD.Load<PackedScene>("res://Scene/Enemies/FlyingEnemy.tscn");
			enemyInstance = (Enemy)enemyScene.Instance();
			Random ran = new Random();
			if ((ran.Next() % 5) > 2)
			{
				enemyInstance.Position = Enemy.FlyingInitialPosition;
			}
			else
			{
				Vector2 tmp = new Vector2(Enemy.cactusInitialPosition.x, Enemy.cactusInitialPosition.y - 10);
				enemyInstance.Position = tmp;
			}
			
		}

		return enemyInstance;
	}
}

