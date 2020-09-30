using Godot;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

public class NewWorld : Node2D
{
	[Export]
	public TileMap bottomMap;
	[Export]
	public int bottomTile;
	public const float GRAVITY = 1200;
	public const float JUMPFORCE = 450;
	public const float GLOBALSPEED = 150;
	private const float ENEMYTIMMER = 1.5f;
	private const float SCORETIMER = 0.13f;
	private const float HARDNESSTIMER = 3f;

	private Player m_player;
	private List<Enemy> m_enemies = new List<Enemy>();
	private Node2D m_backgrounds;
	private Vector2 velocity = new Vector2(0, 0);
	private bool m_playerGrounded = true;
	private bool m_isRunning = false;
	private bool m_firstRun = true;
	private Sprite m_deathSprite;
	private Area2D m_currentArea;
	private Timer m_EnemyTimer;
	private Timer m_scoreTimer;
	private Timer m_hardnessTimer;
	private UInt64 m_score = 0;

	private float m_globalSpeed = 150f;
	private float m_enemyTimer = ENEMYTIMMER;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		m_player = this.GetChild<Player>(1);
		m_player.Init(this);
		m_backgrounds = GetChild(0).GetChild<Node2D>(0);
		m_globalSpeed = 0;
		m_backgrounds.Set("Scrolling Speed", m_globalSpeed);
		ScrollingBackground.moving = false;
		m_EnemyTimer = GetNode<Timer>("EnemyTimer");
		m_scoreTimer = GetNode<Timer>("ScoreTimer");
		m_hardnessTimer = GetNode<Timer>("HardnessTimer");
		m_EnemyTimer.Connect("timeout", this, "SpawnEnemy");
		m_scoreTimer.Connect("timeout", this, "IncreaseScore");
		m_hardnessTimer.Connect("timeout", this, "MakeItHarder");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		do
		{
			if (!m_isRunning)
			{
				if (m_firstRun && Input.IsActionPressed("jump"))
				{
					RunGame();
					break;
				}
				return;
			}
		} while (false);

		if (Input.IsActionPressed("jump"))
		{
			m_player.Jump();
		}
		else if (Input.IsActionPressed("ui_down"))
		{
			m_player.Crouch(); 
		}
		else if (Input.IsActionJustReleased("ui_down"))
		{
			m_player.Run();
		}
	}

	public void RunGame()
	{
		m_globalSpeed = GLOBALSPEED;
		m_isRunning = true;
		ScrollingBackground.moving = true;
		Enemy.moving = true;
		m_backgrounds.Set("Scrolling Speed", m_globalSpeed);
		Enemy.speed = m_globalSpeed;
		m_player.Run();
		m_EnemyTimer.Start(ENEMYTIMMER);
		m_scoreTimer.Start(SCORETIMER);
		m_hardnessTimer.Start(HARDNESSTIMER);
		//SpawnEnemy();
	}

	public void MakeItHarder()
	{
		m_globalSpeed *= 1.1f;
		m_backgrounds.Set("Scrolling Speed", m_globalSpeed);
		Enemy.speed = m_globalSpeed;
		m_hardnessTimer.Start(HARDNESSTIMER);
	}

	public void SpawnEnemy()
	{
		if (!m_isRunning)
		{
			return;
		}
		Random ran = new Random();
		Enemy enemyObj;
		if ((ran.Next() % 2) == 1)
		{
			enemyObj = Enemy.Spawn(EnemyType.CACTUS);
		}
		else
		{
			enemyObj = Enemy.Spawn(EnemyType.FLYING);
		}
		if (enemyObj != null)
		{
			Debug.Print("Adding Enemyu");
			AddChild(enemyObj);
			m_enemies.Add(enemyObj);
		}
		m_enemyTimer *= 0.99f;
		m_EnemyTimer.Start(m_enemyTimer);
	}

	public void IncreaseScore()
	{
		m_score += (UInt64)(GLOBALSPEED * 0.03f);
		GetNode<RichTextLabel>("Score").Text = m_score.ToString();
		m_scoreTimer.Start(SCORETIMER);
	}

	public void EndGame(Godot.Object obj)
	{
		Debug.Print("EndGame");
		Enemy.moving = false;
		ScrollingBackground.moving = false;

		foreach (Enemy enemy in m_enemies)
		{
			enemy.StopAnimation(null);
		}

		m_player.Dead();
		m_isRunning = false;
		m_firstRun = false;
		m_EnemyTimer.Stop();
		m_scoreTimer.Stop();
	}
}
