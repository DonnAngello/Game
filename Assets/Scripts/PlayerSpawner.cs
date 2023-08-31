using Game.AvatarEditor;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public static PlayerSpawner instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject playerPrefab;
    public GameObject cameraPrefab;

    [Header("Outputs")]
    public GameObject player;
    public GameObject playerFollowCam;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
            Debug.Log("CaLLED");
        }
    }

    public void SpawnPlayer()
    {
        Transform spawnPoint = SpawnManager.instance.GetSpawnPoint();

        Debug.Log(spawnPoint.position);

        //player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

        if (player == null)
        {
            Debug.LogError("PlayerMovement script is null.");
        }
        
        //playerFollowCam = PhotonNetwork.Instantiate(cameraPrefab.name, new Vector3(0.0f, 9.3f, 4.6f), Quaternion.identity, 0);
        CameraProperties.instance.SetParameters(player.transform);

        //ReadyPlayerMeAvatar.instance.SetPlayerParent(player.transform);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
