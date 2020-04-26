using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PlatformerMov2D), typeof(Weapon))]
public class PlayerAnimationsTrigger : MonoBehaviour
{
	// Requirements
	private Animator _playerAnimator;
	private PlatformerMov2D _platformerMov2D;
	private Weapon _weapon;

	protected void Awake()
	{
		_playerAnimator = gameObject.GetComponent<Animator>();
		_platformerMov2D = gameObject.GetComponent<PlatformerMov2D>();
		_weapon = gameObject.GetComponent<Weapon>();

		_platformerMov2D.HitGroundEvent += OnHitGroundEvent;
		_platformerMov2D.LeaveGroundEvent += OnLeaveGroundEvent;

		_weapon.AttackEvent += OnAttackEvent;
	}

	protected void Update()
	{
		if(_platformerMov2D.IsGrounded)
		{
			_playerAnimator.SetFloat("MoveSpeed", _platformerMov2D.CurrentMoveSpeed);
		}
		else
		{
			_playerAnimator.SetFloat("VerticalVelocity", _platformerMov2D.VerticalVelocity);
		}
	}

	protected void OnDestroy()
	{
		_platformerMov2D.HitGroundEvent -= OnHitGroundEvent;
		_platformerMov2D.LeaveGroundEvent -= OnLeaveGroundEvent;

		_weapon.AttackEvent -= OnAttackEvent;
	}

	private void OnAttackEvent(Weapon weapon)
	{
		_playerAnimator.SetTrigger("Attack");
	}

	private void OnHitGroundEvent()
	{
		_playerAnimator.SetFloat("VerticalVelocity", 0);
	}

	private void OnLeaveGroundEvent()
	{
		_playerAnimator.SetFloat("MoveSpeed", 0);
	}
}
