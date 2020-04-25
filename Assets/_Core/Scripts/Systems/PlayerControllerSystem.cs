using RasofiaGames.SimpleUnityECS;
using UnityEngine;

public class PlayerControllerSystem : MonoBehaviour
{
	private EntityFilter _playerControllerFilter;

	protected void Awake()
	{
		FilterRules playerFilterRules = FilterRulesBuilder.SetupHasTagBuilder("Player")
			.AddHasComponentRule<PlatformerMov2D>(true)
			.AddHasComponentRule<Weapon>(true)
			.Result();

		_playerControllerFilter = EntityFilter.Create(playerFilterRules, null, null);
	}

	protected void Update()
	{
		_playerControllerFilter.ForEach(playerEntity => 
		{
			PlatformerMov2D platformer2D = playerEntity.GetEntityComponent<PlatformerMov2D>();

			bool leftPressed = Input.GetKey(KeyCode.A);
			bool rightPressed = Input.GetKey(KeyCode.D);

			if(leftPressed || rightPressed)
			{
				if(leftPressed)
				{
					platformer2D.Move(PlatformerMov2D.Direction.Left);
				}

				if(rightPressed)
				{
					platformer2D.Move(PlatformerMov2D.Direction.Right);
				}
			}
			else
			{
				platformer2D.Stop();
			}

			if(Input.GetKeyDown(KeyCode.W))
			{
				platformer2D.Jump();
			}

			if(Input.GetKeyDown(KeyCode.K))
			{
				playerEntity.GetEntityComponent<Weapon>().Shoot(Vector2.right * playerEntity.transform.localScale.x);
			}

			if(Input.GetKeyDown(KeyCode.Q))
			{
				playerEntity.GetEntityComponent<Weapon>().CycleToPreviousCrystal();
			}

			if(Input.GetKeyDown(KeyCode.E))
			{
				playerEntity.GetEntityComponent<Weapon>().CycleToNextCrystal();
			}
		});
	}

	protected void OnDestroy()
	{
		_playerControllerFilter.Clean(null, null);
	}
}
