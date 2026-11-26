using Unity.Netcode;
using UnityEngine;

public class ColorManagerSpawner : MonoBehaviour
{
    [SerializeField]
    private NetworkObject colorManagerPrefab;

    private void Start()
    {
        if (!NetworkManager.Singleton.IsServer) return;

        NetworkObject colorManager = Instantiate(colorManagerPrefab);

        colorManager.Spawn();
    }
}