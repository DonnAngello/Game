using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.AvatarEditor;

public class PlayerSpawner : MonoBehaviourPunCallbacks//, IPunInstantiateMagicCallback
{
    public static PlayerSpawner instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private ReadyPlayerMeAvatar avatarLoader;
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public GameObject reconnectionPrefab;

    [Header("Outputs")]
    public GameObject player;
    public GameObject playerFollowCam;
    public GameObject reconnector;


    public void SpawnPlayer(string avatarUrl)
    {
        Debug.Log("rekonekt pokrenuo ?!");
        Transform spawnPoint = SpawnManager.instance.GetSpawnPoint();

        Debug.Log(spawnPoint.position);

        player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation, 0, new object[] { avatarUrl });

        if (player == null)
        {
            Debug.LogError("PlayerMovement script is null.");
        }

        //playerFollowCam = PhotonNetwork.Instantiate(cameraPrefab.name, new Vector3(0.0f, 9.3f, 4.6f), Quaternion.identity, 0);
        playerFollowCam = Instantiate(cameraPrefab, cameraPrefab.transform.position, Quaternion.identity);
        reconnector = Instantiate(reconnectionPrefab, reconnectionPrefab.transform.position, Quaternion.identity);
    }
    
    public void InstantiateAvatar(PhotonMessageInfo info)
    {
        Debug.Log("pokrenuto");
        object[] instantiationData = info.photonView.InstantiationData;
        var player = info.photonView.gameObject;
        Debug.Log((string)instantiationData[0] + " instaciramo");
        avatarLoader.LoadAvatarInsidePlayer(player, (string) instantiationData[0]);
    }

}
