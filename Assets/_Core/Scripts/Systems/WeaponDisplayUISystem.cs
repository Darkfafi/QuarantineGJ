using RasofiaGames.SimpleUnityECS;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDisplayUISystem : MonoBehaviour
{
	[SerializeField]
	private WeaponDisplay _weaponDisplayPrefab = null;

	[SerializeField]
	private Camera _camera = null;

	[SerializeField]
	private RectTransform _container = null;

	private EntityFilter _weaponHolderFilter;
	private Dictionary<Entity, WeaponDisplay> _entityToWeaponDisplaysMap = new Dictionary<Entity, WeaponDisplay>();

	protected void Awake()
	{
		FilterRules weaponHolderFilterRules = FilterRulesBuilder.SetupHasTagBuilder("WeaponDisplay")
																.AddHasComponentRule<Weapon>(true).Result();

		_weaponHolderFilter = EntityFilter.Create(weaponHolderFilterRules, OnEntityTracked, OnEntityUntracked);
	}

	protected void Update()
	{
		foreach(var pair in _entityToWeaponDisplaysMap)
		{
			Vector3 worldToScreenSpace = _camera.WorldToScreenPoint(pair.Key.transform.position);
			pair.Value.transform.position = worldToScreenSpace;
		}
	}

	private void OnEntityTracked(Entity entity)
	{
		if(!_entityToWeaponDisplaysMap.ContainsKey(entity))
		{
			WeaponDisplay weaponWheel = Instantiate(_weaponDisplayPrefab, _container);
			_entityToWeaponDisplaysMap.Add(entity, weaponWheel);
		}
	}

	private void OnEntityUntracked(Entity entity)
	{
		if(_entityToWeaponDisplaysMap.TryGetValue(entity, out WeaponDisplay weaponDisplay))
		{
			if(weaponDisplay != null)
			{
				Destroy(weaponDisplay.gameObject);
				_entityToWeaponDisplaysMap.Remove(entity);
			}
		}
	}
}
