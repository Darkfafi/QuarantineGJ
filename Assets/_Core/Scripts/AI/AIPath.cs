using RasofiaGames.SimpleUnityECS;
using UnityEngine;
using System.Linq;

public class AIPath : EntityComponent
{
	[SerializeField]
	private Transform[] _waypoints = new Transform[] { };

	[SerializeField]
	private AnimationCurve _pathCurve = null;

	public int WaypointAmount => _waypoints.Length;

	public Vector3 GetPosition(int waypointIndex, float normalizedTime)
	{
		waypointIndex = Mathf.Clamp(waypointIndex, 0, _waypoints.Length - 1);

		if(waypointIndex == 0)
		{
			return _waypoints[waypointIndex].position;
		}
		else
		{
			Vector3 diff = _waypoints[waypointIndex].position - _waypoints[waypointIndex - 1].position;
			return _waypoints[waypointIndex - 1].position + (normalizedTime * diff) + ((diff.x > diff.y ? Vector3.up * Mathf.Abs(diff.normalized.x) : Vector3.right * Mathf.Abs(diff.normalized.y)) * _pathCurve.Evaluate(normalizedTime));
		}
	}

	private void OnValidate()
	{
		_waypoints = transform.GetComponentsInChildren<Transform>().Where(x => x != transform).ToArray();
	}

	public bool ReachedEnd(int waypointIndex)
	{
		return waypointIndex > _waypoints.Length - 1;
	}

	protected void OnDrawGizmos()
	{
		for(int i = 0; i < _waypoints.Length; i++)
		{
			Gizmos.DrawSphere(_waypoints[i].position, 0.1f);
		}

		if(_waypoints.Length > 1)
		{
			for(int i = 1; i < _waypoints.Length; i++)
			{
				Gizmos.DrawLine(_waypoints[i - 1].position, _waypoints[i].position);
			}
		}
	}
}
