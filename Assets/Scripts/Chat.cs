using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Chat : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject Message;
    public GameObject Content;

    public void SendMessage()
    {
        GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All,(PhotonNetwork.NickName + " : " + inputField.text));

        inputField.text = "";
    }

    [PunRPC]
    public void GetMessage(string RecieveMessage)
    {
        GameObject M = Instantiate(Message, Vector3.zero, Quaternion.identity, Content.transform);
        M.GetComponent<Message>().MyMessage.text = RecieveMessage;
        //Destroy(M, 10.0f);
    }    
}
