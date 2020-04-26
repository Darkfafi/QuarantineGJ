using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Health))]
public class EnemyBatAnimationsTrigger : MonoBehaviour
{
	// Requirements
	private Animator _enemyAnimator;
	private Health _health;

	protected void Awake()
	{
		_enemyAnimator = gameObject.GetComponent<Animator>();
		_health = gameObject.GetComponent<Health>();

		_health.DeathEvent += OnDeathEvent;
	}

	protected void OnDestroy()
	{
		_health.DeathEvent -= OnDeathEvent;
	}

	private void OnDeathEvent(Health health)
	{
		_enemyAnimator.SetTrigger("Death");
	}
}
