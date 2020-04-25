using UnityEngine;

public class DataAccessor : MonoBehaviour
{
	public static DataAccessor Instance
	{
		get; private set;
	}

	[SerializeField]
	private CrystalDataCollection _crystalDataCollection = null;

	public CrystalDataCollection CrystalDataCollection => _crystalDataCollection;

	protected void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
