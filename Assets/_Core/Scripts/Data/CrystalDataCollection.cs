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
		Red,
		Blue,
		Yellow,
		Orange
	}

	[SerializeField]
	private Projectile _defaultProjectilePrefab = null;

	[SerializeField]
	private CrystalData[] _crystalData = null;

	public Projectile GetProjectilePrefab(CrystalID id)
	{
		CrystalData data = GetCrystalData(id);
		return data.ProjectilePrefabOverride ?? _defaultProjectilePrefab;
	}

	public CrystalData GetCrystalData(CrystalID id)
	{
		return _crystalData.FirstOrDefault(x => x.ID == id);
	}

	[Serializable]
	public struct CrystalData
	{
		public CrystalID ID;
		public Color CrystalColor;
		public Projectile ProjectilePrefabOverride;
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
