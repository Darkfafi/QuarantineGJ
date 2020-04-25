using RasofiaGames.SimpleUnityECS;
using System.Collections.Generic;
using UnityEngine;
using static CrystalDataCollection;

public class Weapon : EntityComponent
{
	[SerializeField]
	private Transform _shootOrigin = null;

	[SerializeField]
	private float _projectileSpeed = 5f;

	private int _crystalIndex = 0;

	private List<CrystalID> _crystalIDs = new List<CrystalID>()
	{
		// Starting Crystal
		CrystalID.Test,
	};

	public CrystalID CurrentCrystalID => _crystalIDs[_crystalIndex];

	public CrystalID[] GetCrystalIDs()
	{
		return _crystalIDs.ToArray();
	}

	public void Shoot(Vector2 direction)
	{
		Projectile projectileInstance = Instantiate(DataAccessor.Instance.CrystalDataCollection.GetProjectilePrefab(CurrentCrystalID));
		projectileInstance.transform.position = _shootOrigin.transform.position;
		projectileInstance.Init(CurrentCrystalID);
		projectileInstance.Fire(direction, _projectileSpeed);
	}
}
