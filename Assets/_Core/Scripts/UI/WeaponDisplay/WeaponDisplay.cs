using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CrystalDataCollection;

public class WeaponDisplay : MonoBehaviour
{
	[SerializeField]
	private Image _crystalItemPrefab = null;

	[SerializeField]
	private RectTransform _wheel = null;

	[SerializeField]
	private float _wheelRadius = 64f;

	[SerializeField]
	private float _rotationSpeed = 5f;

	private float _currentCrystalAngle = 0f;

	private Dictionary<CrystalID, CrystalItemData> _crystalItems = new Dictionary<CrystalID, CrystalItemData>();

	protected void Update()
	{
		Vector3 rotation = _wheel.localRotation.eulerAngles;
		rotation.z = Mathf.LerpAngle(rotation.z, _currentCrystalAngle, Time.deltaTime * _rotationSpeed);
		_wheel.localRotation = Quaternion.Euler(0f, 0f, rotation.z);
	}

	public void SetCrystalItems(CrystalID[] crystalIDs)
	{
		ClearCrystals();
		float angleDelta = GetAngleDelta(crystalIDs.Length);
		for(int i = 0; i < crystalIDs.Length; i++)
		{
			CrystalData data = DataAccessor.Instance.CrystalDataCollection.GetCrystalData(crystalIDs[i]);
			Image crystalItem = Instantiate(_crystalItemPrefab, _wheel);
			crystalItem.color = data.CrystalColor;
			CrystalItemData crystalItemData = new CrystalItemData()
			{
				ItemIndex = i,
				CrystalID = data.ID,
				CrystalItem = crystalItem
			};
			_crystalItems.Add(data.ID, crystalItemData);
			Vector2 itemPos = new Vector2(Mathf.Cos(angleDelta * Mathf.Deg2Rad * i) * _wheelRadius, Mathf.Sin(angleDelta * Mathf.Deg2Rad * i) * _wheelRadius);
			crystalItem.transform.localPosition = itemPos;
		}
	}

	public void SetCurrentCrystal(CrystalID crystalID)
	{
		if(_crystalItems.TryGetValue(crystalID, out CrystalItemData crystalItemData))
		{
			_currentCrystalAngle = 360f - GetAngleDelta(_crystalItems.Count) * crystalItemData.ItemIndex;
		}
	}

	public void ClearCrystals()
	{
		foreach(var pair in _crystalItems)
		{
			Destroy(pair.Value.CrystalItem.gameObject);
		}
		_crystalItems.Clear();
	}

	public float GetAngleDelta(int crystalAmount)
	{
		return crystalAmount == 0 ? 0f : 360f / crystalAmount;
	}

	private struct CrystalItemData
	{
		public int ItemIndex;
		public CrystalID CrystalID;
		public Image CrystalItem;
	}
}
