using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraProperties : MonoBehaviour
{
    public static CameraProperties instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private CinemachineFreeLook playerFollowCamera;


public void SetParameters(Transform playerPosition)
    {
        playerFollowCamera.Follow = playerPosition;
        playerFollowCamera.LookAt = playerPosition;
        /*
        if (!playerFollowCam.GetComponent<PhotonView>().IsMine)
        {
            playerFollowCam.SetActive(false);
        }
        */
        Debug.Log(playerFollowCamera.LookAt.ToString() + "tu gleda");

    }
}
