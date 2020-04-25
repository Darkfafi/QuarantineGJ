using System;
using UnityEngine;
using static CrystalDataCollection;

public class CrystalDimension : MonoBehaviour
{
	public event Action<CrystalDimension> CrystalDimensionChanged;

	[SerializeField]
	private CrystalID _crystalDimensionID;

	public CrystalID CrystalDimensionID => _crystalDimensionID;

	public void SetCrystalDimension(CrystalID crystalDimensionID)
	{
		_crystalDimensionID = crystalDimensionID;
		CrystalDimensionChanged?.Invoke(this);
	}
}
