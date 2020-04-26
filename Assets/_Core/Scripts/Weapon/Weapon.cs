using System;
using RasofiaGames.SimpleUnityECS;
using System.Collections.Generic;
using UnityEngine;
using static CrystalDataCollection;

public class Weapon : EntityComponent
{
	public event Action<Weapon> AttackEvent;
	public event Action<Weapon> SwitchedWeaponEvent;

	[SerializeField]
	private Transform _shootOrigin = null;

	[SerializeField]
	private float _projectileSpeed = 5f;

	private int _crystalIndex = 0;
	private SpriteColorSwap _spriteColorSwap;

	private List<CrystalID> _crystalIDs = new List<CrystalID>()
	{
		// Starting Crystal
		CrystalID.Red,
		CrystalID.Blue,
		CrystalID.Yellow,
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

	public void Attack(Vector2 direction)
	{
		Projectile projectileInstance = Instantiate(DataAccessor.Instance.CrystalDataCollection.GetProjectilePrefab(CurrentCrystalID));
		projectileInstance.transform.position = _shootOrigin.transform.position;
		projectileInstance.Init(CurrentCrystalID);
		projectileInstance.Fire(direction, _projectileSpeed);
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
		if(_spriteColorSwap != null)
		{
			_spriteColorSwap.SwapColor(1, DataAccessor.Instance.CrystalDataCollection.GetCrystalData(CurrentCrystalID).CrystalColor, 0);
		}
	}
}
