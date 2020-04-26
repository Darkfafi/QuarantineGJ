using System;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CrystalDataCollection : ScriptableObject
{
	public enum CrystalID
	{
		Gem1,
		Gem2,
		Gem3,
		Gem4
	}

	[SerializeField]
	private CrystalData[] _crystalData = null;

	public CrystalData GetCrystalData(CrystalID id)
	{
		return _crystalData.FirstOrDefault(x => x.ID == id);
	}

	[Serializable]
	public struct CrystalData
	{
		public CrystalID ID;
		public Color CrystalColor;
	}

#if UNITY_EDITOR
	[MenuItem("Assets/Create/DataCollections/CrystralDataCollection")]
	static void CreateDataCollection()
	{
		CrystalDataCollection dataCollection = CreateInstance<CrystalDataCollection>();
		AssetDatabase.CreateAsset(dataCollection, "Assets/_Core/CrystralDataCollection.asset");
		AssetDatabase.SaveAssets();
		Selection.activeObject = dataCollection;
	}
#endif
}
