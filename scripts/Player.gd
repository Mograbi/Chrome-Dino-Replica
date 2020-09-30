extends KinematicBody2D


# member variables

export var gravity = 1000
var velocity = Vector2(0, gravity)


func _physics_process(delta: float) -> void:
	velocity.y = gravity
	move_and_slide(velocity)
