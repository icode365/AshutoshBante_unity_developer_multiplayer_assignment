using Fusion;
using UnityEngine;
using Assignment.Scripts.UI;
using Assignment.Scripts.Player;
using Assignment.Scripts.Player.Camera;

namespace Assignment.Scripts.Multiplayer.Player
{
    public class NetworkedPlayer : NetworkBehaviour
    {
        [Header("References")] public UINameplate Nameplate;

        [Networked, HideInInspector, Capacity(24), OnChangedRender(nameof(ApplyNickname))]
        public string Nickname { get; set; }

        public override void Spawned()
        {
            Debug.Log("Player has spawned.");
            if (HasStateAuthority)
            {
                Nickname = $"{Object.StateAuthority.PlayerId}_" + PlayerPrefs.GetString("PlayerName", Object.StateAuthority.PlayerId.ToString());

#if UNITY_EDITOR
                gameObject.name = $"{Nickname}_local";
#endif
                EnableCameraController();
            }
            else
            {
                DisablePlayerInput();
            }

            ApplyNickname();
        }

        private void DisablePlayerInput()
        {
            GetComponent<PlayerInputHandler>().enabled = false;
        }

        private void EnableCameraController()
        {
            var cameraTransform = Camera.main.transform;
            var cameraController = GetComponentInChildren<ThirdPersonCameraController>().gameObject;
            
            cameraTransform.parent = cameraController.transform;
            cameraTransform.localPosition = Vector3.zero;
            cameraTransform.localRotation = Quaternion.identity;
        }

        private void ApplyNickname()
        {
            if (HasStateAuthority)
            {
                Nameplate.gameObject.SetActive(false);
                return;
            }

            Nameplate.SetNickname(Nickname);
        }
    }
}