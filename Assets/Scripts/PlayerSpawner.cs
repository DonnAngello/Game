using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.AvatarEditor;

public class PlayerSpawner : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public static PlayerSpawner instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private ReadyPlayerMeAvatar avatarLoader;
    public GameObject playerPrefab;
    public GameObject cameraPrefab;

    [Header("Outputs")]
    public GameObject player;
    public GameObject playerFollowCam;


    public void SpawnPlayer(string avatarUrl)
    {
        Transform spawnPoint = SpawnManager.instance.GetSpawnPoint();

        Debug.Log(spawnPoint.position);

        player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation, 0, new object[] { avatarUrl });

        if (player == null)
        {
            Debug.LogError("PlayerMovement script is null.");
        }

        playerFollowCam = PhotonNetwork.Instantiate(cameraPrefab.name, new Vector3(0.0f, 9.3f, 4.6f), Quaternion.identity, 0);
        CameraProperties.instance.SetParameters(player.transform);
        
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        var player = info.photonView.gameObject;
        avatarLoader.LoadAvatarInsidePlayer(player, (string) instantiationData[0]);
    }
}
