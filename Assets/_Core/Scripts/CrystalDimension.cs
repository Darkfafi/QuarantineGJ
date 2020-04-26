using System;
using UnityEngine;
using static CrystalDataCollection;

public class CrystalDimension : MonoBehaviour
{
	public event Action<CrystalDimension> CrystalDimensionChanged;

	[SerializeField]
	private CrystalID _crystalDimensionID;

	[SerializeField]
	private SpriteColorSwap _spriteColorSwap = null;

	public CrystalID CrystalDimensionID => _crystalDimensionID;

	protected void Start()
	{
		SetCrystalDimension(_crystalDimensionID);
	}

	public void SetCrystalDimension(CrystalID crystalDimensionID)
	{
		_crystalDimensionID = crystalDimensionID;
		if(_spriteColorSwap != null)
		{
			_spriteColorSwap.SwapColor(1, DataAccessor.Instance.CrystalDataCollection.GetCrystalData(CrystalDimensionID).CrystalColor, 0);
		}
		CrystalDimensionChanged?.Invoke(this);
	}
}
