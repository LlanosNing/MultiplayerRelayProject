using Unity.Netcode;
using UnityEngine;

public class BasicNetworkPlayer : NetworkBehaviour
{
    private void Start()
    {
        string msg = $"Player ID: {NetworkObject.OwnerClientId}";
        if (IsLocalPlayer)
        {
            msg += " and is your Local Player";
        }
        Debug.Log(msg);

        transform.position = PlayerPositioner.Instance.GetPosition(NetworkObject.OwnerClientId);
    }

    private void OnLevelWasLoaded(int level)
    {
        transform.position = PlayerPositioner.Instance.GetPosition(NetworkObject.OwnerClientId);
    }
}
