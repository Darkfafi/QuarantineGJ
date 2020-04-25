using UnityEngine;

public class DataAccessor : MonoBehaviour
{
	public static DataAccessor Instance
	{
		get
		{
			if(_instance == null)
			{
				SetInstance(FindObjectOfType<DataAccessor>());
			}

			return _instance;
		}
	}

	private static DataAccessor _instance = null;

	[SerializeField]
	private CrystalDataCollection _crystalDataCollection = null;

	public CrystalDataCollection CrystalDataCollection => _crystalDataCollection;

	protected void Awake()
	{
		SetInstance(this);
	}

	protected void OnDestroy()
	{
		if(this == _instance)
		{
			_instance = null;
		}
	}

	private static void SetInstance(DataAccessor dataAccessor)
	{
		if(_instance == null)
		{
			_instance = dataAccessor;
			DontDestroyOnLoad(dataAccessor.gameObject);
		}
		else if(_instance != dataAccessor)
		{
			Destroy(dataAccessor.gameObject);
		}
	}
}
