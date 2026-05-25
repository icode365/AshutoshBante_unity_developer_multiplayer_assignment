using Assignment.Scripts.Multiplayer.Interactables;
using Assignment.Scripts.Multiplayer.Player;
using Fusion;
using UnityEngine;

namespace Assignment.Scripts.Multiplayer.Managers
{
	/// <summary>
	/// Handles player connections (spawning of Player instances).
	/// </summary>
	public class GameManager : NetworkBehaviour
	{
		public NetworkedPlayer PlayerPrefab;
		public float SpawnRadius = 3f;

		
		[SerializeField] private Transform interactableSpawnPoint;
		[SerializeField] private NetworkedInteractable interactablePrefab;
		
		public NetworkedPlayer LocalPlayer { get; private set; }

		public Vector3 GetSpawnPosition()
		{
			var randomPositionOffset = Random.insideUnitCircle * SpawnRadius;
			return transform.position + new Vector3(randomPositionOffset.x, 0, randomPositionOffset.y);
		}

		public override void Spawned()
		{
			LocalPlayer = Runner.Spawn(PlayerPrefab, GetSpawnPosition(), Quaternion.identity, Runner.LocalPlayer);
			Runner.SetPlayerObject(Runner.LocalPlayer, LocalPlayer.Object);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			LocalPlayer = null;
		}
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(transform.position, SpawnRadius);
		}
	}
}