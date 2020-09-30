using Godot;
using System;
using System.Diagnostics;

public class Player : Charachter
{
	private Sprite m_deathSprite;
	public Area2D m_currentArea = null;
	private AnimatedSprite m_runAnimation;
	private AnimatedSprite m_crouchAnimation;
	private bool m_isOnGround = false;
	private new Vector2 velocity = new Vector2(0, 0);
	private bool m_isRunning = false;
	private bool m_wasJumping = false;
	private bool m_isCrouching = false;

	public NewWorld world;
	public const float GRAVITY = 1200;
	public const float JUMPFORCE = 450;

	public Vector2 Velocity
	{
		get { return velocity; }
		set { velocity = value; }
	}

	public override void _Ready()
	{
		m_deathSprite = GetNode<Sprite>("death");
		m_runAnimation = GetNode<AnimatedSprite>("Run");
		m_crouchAnimation = GetNode<AnimatedSprite>("Crouch");

		GetNode<Area2D>("GroundCheck").Connect("area_exited", this, "OffGround");
		GetNode<Area2D>("GroundCheck").Connect("area_entered", this, "OnGround");
	}
	public override void _PhysicsProcess(float delta)
	{
		if (m_isRunning || (!m_isRunning && !m_isOnGround))
		{
			velocity.x = 0;
			velocity.y += GRAVITY * delta;
			MoveAndSlide(velocity);
		}
	}
	public void Init(NewWorld world)
	{
		this.world = world;
		this.Idle();
		m_runAnimation.GetChild<Area2D>(0).Connect("area_entered", world, "EndGame");
		m_crouchAnimation.GetChild<Area2D>(0).Connect("area_entered", world, "EndGame");
	}

	public void Run()
	{
		m_runAnimation.GetChild<Area2D>(0).Visible = true;
		m_runAnimation.Visible = true;
		m_runAnimation.Playing = true;
		m_deathSprite.Visible = false;
		m_crouchAnimation.Visible = false;
		m_runAnimation.GetChild<Area2D>(0).Monitoring = true;
		m_crouchAnimation.GetChild<Area2D>(0).Monitoring = false;
		m_currentArea = m_runAnimation.GetChild<Area2D>(0);
		m_crouchAnimation.GetChild<Area2D>(0).Visible = false;
		m_isRunning = true;
		m_isCrouching = false;
	}
	public void Jump()
	{
		if (!m_isOnGround || m_isCrouching)
		{
			return;
		}
		m_runAnimation.GetChild<Area2D>(0).Visible = true;
		m_runAnimation.Playing = false;
		m_deathSprite.Visible = false;
		m_crouchAnimation.Visible = false;
		m_crouchAnimation.GetChild<Area2D>(0).Monitoring = false;
		m_currentArea = m_runAnimation.GetChild<Area2D>(0);
		m_crouchAnimation.GetChild<Area2D>(0).Visible = false;

		velocity.y = -JUMPFORCE;
		m_wasJumping = true;
	}

	public void Crouch()
	{
		if (m_isCrouching)
		{
			return;
		}
		m_crouchAnimation.GetChild<Area2D>(0).Visible = true;
		m_runAnimation.Visible = false;
		m_runAnimation.GetChild<Area2D>(0).Visible = false;
		m_runAnimation.Playing = false;
		m_runAnimation.GetChild<Area2D>(0).Monitoring = false;
		m_crouchAnimation.GetChild<Area2D>(0).Monitoring = true;
		m_deathSprite.Visible = false;
		m_crouchAnimation.Visible = true;
		m_crouchAnimation.Playing = true;
		m_currentArea = m_crouchAnimation.GetChild<Area2D>(0);
		m_isCrouching = true;
	}

	private void OnGround(Godot.Object obj)
	{
		if (m_wasJumping)
		{
			m_runAnimation.Playing = true;
			m_wasJumping = false;
		}
		m_isOnGround = true;
	}

	private void OffGround(Godot.Object obj)
	{
		m_isOnGround = false;
	}

	public void Dead()
	{
		m_crouchAnimation.GetChild<Area2D>(0).Visible = false;
		m_runAnimation.Playing = false;
		m_runAnimation.Visible = false;
		m_deathSprite.Visible = true;
		m_crouchAnimation.Visible = false;
		m_crouchAnimation.Playing = false;
		m_runAnimation.GetChild<Area2D>(0).Visible = false;
		m_isRunning = false;
	}

	public void Idle()
	{
		m_crouchAnimation.GetChild<Area2D>(0).Visible = false;
		m_runAnimation.Playing = false;
		m_runAnimation.Visible = true;
		m_deathSprite.Visible = false;
		m_crouchAnimation.Visible = false;
		m_crouchAnimation.Playing = false;
	}
}
