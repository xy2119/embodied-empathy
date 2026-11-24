using UnityEngine;

namespace AvatarSDK.MetaPerson.MobileIntegrationSample
{
	/// <summary>
	/// Holds the MetaPerson (AvatarSDK) client credentials.
	/// Values are read at runtime from METAPERSON_CLIENT_ID / METAPERSON_CLIENT_SECRET
	/// (see Assets/Resources/env.txt). Never commit real credentials.
	/// </summary>
	public class AccountCredentials : MonoBehaviour
	{
		public string clientId;

		public string clientSecret;

		public bool IsEmpty()
		{
			return string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret);
		}
	}
}
