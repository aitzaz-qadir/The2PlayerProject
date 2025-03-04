using Unity.Netcode;
using UnityEngine;

public class CustomNetwork : MonoBehaviour {
    public GameObject Player1Prefab; // Host Player Prefab
    public GameObject Player2Prefab; // Client Player Prefab

    private void Start() {
        NetworkManager.Singleton.OnServerStarted += SpawnPlayer;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void SpawnPlayer() {
        if (NetworkManager.Singleton.IsHost) {
            GameObject player1 = Instantiate(Player1Prefab);
            player1.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
        }
    }

    private void OnClientConnected(ulong clientId) {
        if (!NetworkManager.Singleton.IsHost) // Clients should spawn Player2Prefab
        {
            GameObject player2 = Instantiate(Player2Prefab);
            player2.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }
    }
}
