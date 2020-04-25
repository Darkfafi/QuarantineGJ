using RasofiaGames.SimpleUnityECS;
using System;
using UnityEngine;

public class Health : EntityComponent
{
	public delegate void HealthAffectedHandler(Health health, int effectAmount);
	public event HealthAffectedHandler DamagedEvent;
	public event Action<Health> DeathEvent;

	[SerializeField]
	private int _maxHealth = 1;

	public bool IsAlive => CurrentHealth > 0;

	public int CurrentHealth
	{
		get; private set;
	}

	protected override void Awake()
	{
		base.Awake();
		CurrentHealth = _maxHealth;	
	}

	public void Damage(int amount)
	{
		if(!IsAlive)
		{
			return;
		}

		int oldAmount = CurrentHealth;
		CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
		if(oldAmount != CurrentHealth)
		{
			DamagedEvent?.Invoke(this, oldAmount - CurrentHealth);

			if(!IsAlive)
			{
				DeathEvent?.Invoke(this);
			}
		}
	}

	public void Kill()
	{
		Damage(CurrentHealth);
	}
}
