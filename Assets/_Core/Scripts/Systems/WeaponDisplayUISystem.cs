using RasofiaGames.SimpleUnityECS;
using System;
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
			Vector3 scale = pair.Value.transform.localScale;
			scale.x = Mathf.Abs(scale.x) * Mathf.Abs(pair.Key.transform.localScale.x) / pair.Key.transform.localScale.x;
			pair.Value.transform.localScale = scale;
		}
	}

	private void OnEntityTracked(Entity entity)
	{
		if(!_entityToWeaponDisplaysMap.ContainsKey(entity))
		{
			Weapon weapon = entity.GetEntityComponent<Weapon>();
			WeaponDisplay weaponWheel = Instantiate(_weaponDisplayPrefab, _container);
			_entityToWeaponDisplaysMap.Add(entity, weaponWheel);
			weapon.SwitchedWeaponEvent += UpdateCurrentWeapon;

			// Initially Set values
			UpdateWheelItems(weapon);
			UpdateCurrentWeapon(weapon);
		}
	}

	private void OnEntityUntracked(Entity entity)
	{
		if(_entityToWeaponDisplaysMap.TryGetValue(entity, out WeaponDisplay weaponDisplay))
		{
			Weapon weapon = entity.GetEntityComponent<Weapon>();
			weapon.SwitchedWeaponEvent -= UpdateCurrentWeapon;
			if(weaponDisplay != null)
			{
				Destroy(weaponDisplay.gameObject);
				_entityToWeaponDisplaysMap.Remove(entity);
			}
		}
	}

	private void UpdateCurrentWeapon(Weapon weapon)
	{
		if(_entityToWeaponDisplaysMap.TryGetValue(weapon.Parent, out WeaponDisplay weaponDisplay))
		{
			weaponDisplay.SetCurrentCrystal(weapon.CurrentCrystalID);
		}
	}

	private void UpdateWheelItems(Weapon weapon)
	{
		if(_entityToWeaponDisplaysMap.TryGetValue(weapon.Parent, out WeaponDisplay weaponDisplay))
		{
			weaponDisplay.SetCrystalItems(weapon.GetCrystalIDs());
		}
	}
}
