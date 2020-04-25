using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlatformerMov2D : MonoBehaviour
{
	public enum Direction
	{
		None = 0,
		Left = -1,
		Right = 1
	}

	[SerializeField]
	private float _moveSpeed = 5f;

	[SerializeField]
	private float _jumpForce = 5f;

	// Values
	private Transform _currentFloor = null;
	private Transform _leftCol = null;
	private Transform _rightCol = null;

	// Requirements
	private Rigidbody2D _rigidbody2D;
	private Collider2D _collider2D;

	public bool IsGrounded => _currentFloor != null;

	protected void Awake()
	{
		_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
		_collider2D = gameObject.GetComponent<Collider2D>();
	}

	protected void Update()
	{
		bool leftPressed = Input.GetKey(KeyCode.A);
		bool rightPressed = Input.GetKey(KeyCode.D);

		if(leftPressed || rightPressed)
		{
			if(leftPressed)
			{
				Move(Direction.Left);
			}

			if(rightPressed)
			{
				Move(Direction.Right);
			}
		}
		else
		{
			Stop();
		}

		if(Input.GetKeyDown(KeyCode.W))
		{
			Jump();
		}
	}

	public void Move(Direction direction)
	{
		if(!IsSideHit(direction))
		{
			Vector2 vel = _rigidbody2D.velocity;
			vel.x = ((int)direction) * _moveSpeed;
			_rigidbody2D.velocity = vel;
		}
		else
		{
			Stop();
		}
	}

	public void Stop()
	{
		Vector2 vel = _rigidbody2D.velocity;
		vel.x = 0f;
		_rigidbody2D.velocity = vel;
	}

	public void Jump()
	{
		if(IsGrounded)
		{
			_rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
		}
	}

	private void HitGround(Transform groundObject)
	{
		if(!IsGrounded)
		{
			_currentFloor = groundObject;
		}
	}

	private void LeaveGround(Transform groundObject)
	{
		if(IsGrounded && _currentFloor == groundObject)
		{
			_currentFloor = null;
		}
	}

	protected void OnCollisionEnter2D(Collision2D collision)
	{
		if(IsGroundHit(collision))
		{
			HitGround(collision.transform);
		}

		if(IsSideHit(collision, Direction.Right))
		{
			_rightCol = collision.transform;
		}

		if(IsSideHit(collision, Direction.Left))
		{
			_leftCol = collision.transform;
		}
	}

	protected void OnCollisionExit2D(Collision2D collision)
	{
		LeaveGround(collision.transform);

		if(IsSideHit(Direction.Left) && _leftCol == collision.transform)
		{
			_leftCol = null;
		}

		if(IsSideHit(Direction.Right) && _rightCol == collision.transform)
		{
			_rightCol = null;
		}
	}

	private bool IsGroundHit(Collision2D collision)
	{
		for(int i = 0; i < collision.contacts.Length; i++)
		{
			if(_collider2D.bounds.center.y > collision.contacts[i].point.y && Mathf.Approximately(collision.contacts[i].normal.y, 1f))
			{
				return true;
			}
		}

		return false;
	}

	private bool IsSideHit(Direction dir)
	{
		switch(dir)
		{
			case Direction.Left:
				return _leftCol != null;
			case Direction.Right:
				return _rightCol != null;
			default:
				return false;
		}
	}

	private bool IsSideHit(Collision2D collision, Direction dir)
	{
		for(int i = 0; i < collision.contacts.Length; i++)
		{
			if(Mathf.Approximately(collision.contacts[i].normal.x, -(float)dir))
			{
				return true;
			}
		}

		return false;
	}
}
