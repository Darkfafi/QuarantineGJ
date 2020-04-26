using RasofiaGames.SimpleUnityECS;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyAI : EntityComponent
{
	[SerializeField]
	private float _minMovementSpeed = 0.8f;

	[SerializeField]
	private float _maxMovementSpeed = 1.2f;

	private Health _health;

	private AIPath _currentPath;
	private int _currentWaypointIndex = 0;
	private float _progress = 1f;
	private float _timeToProgress = 1f;

	private EntityFilter _waypointsFilter;
	private bool _stopMoving = false;
	private float _movementSpeed;

	protected override void Awake()
	{
		base.Awake();

		_health = gameObject.GetComponent<Health>();

		FilterRules waypointsFilterRules = FilterRulesBuilder.SetupNoTagsBuilder()
			.AddHasComponentRule<AIPath>(true).Result();

		_waypointsFilter = EntityFilter.Create(waypointsFilterRules, null, null);
	}

	protected override void Start()
	{
		base.Start();
		_currentPath = _waypointsFilter.GetRandom().GetEntityComponent<AIPath>();
		_movementSpeed = Random.Range(_minMovementSpeed, _maxMovementSpeed);
	}

	protected void Update()
	{
		if(!_stopMoving && _health.IsAlive)
		{
			if(_currentPath.ReachedEnd(_currentWaypointIndex))
			{
				_stopMoving = true;
				return;
			}

			_progress = Mathf.Clamp(_progress + Time.deltaTime * _movementSpeed, 0f, _timeToProgress);
			Vector3 waypointPosStep = _currentPath.GetPosition(_currentWaypointIndex, _progress / _timeToProgress);

			float xDir = -Mathf.Sign(waypointPosStep.x - transform.position.x);

			Vector3 scale = transform.localScale;
			scale.x = Mathf.Abs(scale.x) * xDir;
			transform.localScale = scale;

			transform.position = waypointPosStep;

			if(Mathf.Approximately(_progress, _timeToProgress))
			{
				_progress = 0f;
				_currentWaypointIndex++;
				_timeToProgress = Vector3.Distance(transform.position, _currentPath.GetPosition(_currentWaypointIndex, 1f)) / _movementSpeed;
			}
		}
	}

	protected override void OnDestroy()
	{
		_waypointsFilter.Clean(null, null);
		base.OnDestroy();
	}
}
