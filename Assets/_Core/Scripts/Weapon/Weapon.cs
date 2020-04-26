using RasofiaGames.SimpleUnityECS;
using System;
using System.Collections.Generic;
using UnityEngine;
using static CrystalDataCollection;

public class Weapon : EntityComponent
{
	public event Action<Weapon> AttackEvent;
	public event Action<Weapon> SwitchedWeaponEvent;

	[SerializeField]
	private DamageHitbox _damageHitbox = null;

	private int _crystalIndex = 0;
	private SpriteColorSwap _spriteColorSwap;

	private List<CrystalID> _crystalIDs = new List<CrystalID>()
	{
		// Starting Crystal
		CrystalID.Gem1,
		CrystalID.Gem2,
		CrystalID.Gem3,
	};

	public CrystalID CurrentCrystalID => _crystalIDs[_crystalIndex];

	public CrystalID[] GetCrystalIDs()
	{
		return _crystalIDs.ToArray();
	}

	protected override void Awake()
	{
		base.Awake();
		_spriteColorSwap = gameObject.GetComponent<SpriteColorSwap>();
	}

	protected override void Start()
	{
		base.Start();
		SetCrystalColor();
	}

	public void DoDamage()
	{
		_damageHitbox.Damage();
	}

	public void Attack(Vector2 direction)
	{
		AttackEvent?.Invoke(this);
	}

	public void CycleToNextCrystal()
	{
		_crystalIndex = (_crystalIndex + 1) % _crystalIDs.Count;
		SetCrystalColor();
		SwitchedWeaponEvent?.Invoke(this);
	}

	public void CycleToPreviousCrystal()
	{
		if(_crystalIndex == 0)
		{
			_crystalIndex = _crystalIDs.Count - 1;
		}
		else
		{
			_crystalIndex--;
		}
		SetCrystalColor();
		SwitchedWeaponEvent?.Invoke(this);
	}

	private void SetCrystalColor()
	{
		_damageHitbox.SetCrystalID(CurrentCrystalID);
		
		if(_spriteColorSwap != null)
		{
			_spriteColorSwap.SwapColor(1, DataAccessor.Instance.CrystalDataCollection.GetCrystalData(CurrentCrystalID).CrystalColor, 0);
		}
	}
}
