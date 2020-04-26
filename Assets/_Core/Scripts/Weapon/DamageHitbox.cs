using RasofiaGames.SimpleUnityECS;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageHitbox : EntityComponent
{
	[SerializeField]
	private int _damageAmount = 1;

	private Collider2D _hitboxCollider;
	private List<GameObject> _currentColliders = new List<GameObject>();

	public CrystalDataCollection.CrystalID CrystalID
	{
		get; private set;
	}

	public void SetCrystalID(CrystalDataCollection.CrystalID crystalID)
	{
		CrystalID = crystalID;
	}

	public void Damage()
	{
		base.OnEnable();
		CleanDeadEntries();
		for(int i = _currentColliders.Count - 1; i >= 0; i--)
		{
			GameObject entry = _currentColliders[i];
			CrystalDimension crystalDimension = entry.GetComponent<CrystalDimension>();
			if(crystalDimension == null || crystalDimension.CrystalDimensionID == CrystalID)
			{
				Health health = entry.GetComponent<Health>();
				if(health != null)
				{
					health.Damage(_damageAmount);
				}
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();

		_hitboxCollider = gameObject.GetComponent<Collider2D>();
		_hitboxCollider.isTrigger = true;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CleanDeadEntries();
	}

	protected void OnTriggerEnter2D(Collider2D collision)
	{
		_currentColliders.Add(collision.gameObject);
	}

	protected void OnTriggerExit2D(Collider2D collision)
	{
		_currentColliders.Remove(collision.gameObject);
	}

	private void CleanDeadEntries()
	{
		for(int i = _currentColliders.Count - 1; i >= 0; i--)
		{
			if(_currentColliders[i] == null)
			{
				_currentColliders.RemoveAt(i);
			}
		}
	}
}
