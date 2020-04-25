using RasofiaGames.SimpleUnityECS;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody2D))]
public class Projectile : EntityComponent
{
	public enum TravelState
	{
		None,
		Traveling,
		Hit
	}

	[SerializeField]
	private int _damageAmount = 1;

	// Values
	private Vector2 _projectileVelocity;

	// Requirements
	private Renderer _renderer;
	private Rigidbody2D _rigidbody2D;

	public TravelState State
	{
		get; private set;
	}

	public CrystalDataCollection.CrystalID CrystalID
	{
		get; private set;
	}

	protected override void Awake()
	{
		base.Awake();
		_renderer = gameObject.GetComponent<Renderer>();
		_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
		SetState(TravelState.None);
	}

	public void Init(CrystalDataCollection.CrystalID crystalID)
	{
		CrystalID = crystalID;
		CrystalDataCollection.CrystalData data = DataAccessor.Instance.CrystalDataCollection.GetCrystalData(crystalID);
		_renderer.material.color = data.CrystalColor;
	}

	public void Fire(Vector2 dir, float speed)
	{
		_projectileVelocity = dir.normalized * speed;
		SetState(TravelState.Traveling);
	}

	private void SetState(TravelState state)
	{
		State = state;
		switch(state)
		{
			case TravelState.Traveling:
				_rigidbody2D.velocity = _projectileVelocity;
				break;
			case TravelState.Hit:
				Destroy(gameObject);
				break;
		}
	}

	protected void OnTriggerEnter2D(Collider2D collision)
	{
		CrystalDimension crystalDimension = collision.gameObject.GetComponent<CrystalDimension>();

		if(crystalDimension == null || crystalDimension.CrystalDimensionID == CrystalID)
		{
			Health health = collision.gameObject.GetComponent<Health>();
			if(health != null)
			{
				health.Damage(_damageAmount);
			}

			Destroy(gameObject);
		}
	}
}
