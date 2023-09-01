using Game.AvatarEditor;
using Photon.Pun;
using ReadyPlayerMe.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarCreator : MonoBehaviour, IPunInstantiateMagicCallback
{
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        PlayerSpawner.instance.InstantiateAvatar(info);
    }
}
