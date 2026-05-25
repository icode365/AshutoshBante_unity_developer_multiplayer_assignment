using TMPro;
using UnityEngine;

namespace Assignment.Scripts.UI
{
	/// <summary>
	/// Component that handle showing nicknames above player
	/// </summary>
	public class UINameplate : MonoBehaviour
	{
		public TextMeshProUGUI NicknameText;
		public Transform _cameraTransform;

		public void SetNickname(string nickname)
		{
			NicknameText.text = nickname;
		}

		private void Awake()
		{
			NicknameText.text = string.Empty;
		}

		private void LateUpdate()
		{
			// Rotate nameplate toward camera
			transform.rotation = _cameraTransform.rotation;
		}
	}
}
