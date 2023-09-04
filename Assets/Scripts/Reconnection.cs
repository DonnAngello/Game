using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Photon.Realtime;
using Photon.Pun;

public class Reconnection :MonoBehaviourPunCallbacks, IConnectionCallbacks
{
    private LoadBalancingClient loadBalancingClient;
    private AppSettings appSettings;

    public bool shouldReconnect = true;
    bool reconnecting = false;

    void Start()
    {
        loadBalancingClient = PhotonNetwork.NetworkingClient;
        appSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
        this.loadBalancingClient.AddCallbackTarget(this);
    }

    void OnDestroy()
    {
        if (this.loadBalancingClient != null)
            return;

        this.loadBalancingClient.RemoveCallbackTarget(this);
    }

    void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
    {
        if(!shouldReconnect) return;

        if (this.CanRecoverFromDisconnect(cause))
        {
            Debug.Log("Atempting reconnection!");
            this.Recover();
        }
        else
        {
            Debug.Log("Could not attempt reconnection!");
        }
    }

    private bool CanRecoverFromDisconnect(DisconnectCause cause)
    {
        switch (cause)
        {
            // the list here may be non exhaustive and is subject to review
            case DisconnectCause.Exception:
            case DisconnectCause.ServerTimeout:
            case DisconnectCause.ClientTimeout:
            case DisconnectCause.DisconnectByServerLogic:
            case DisconnectCause.DisconnectByServerReasonUnknown:
                return true;
        }
        return false;
    }

    private void Recover()
    {
        if (!loadBalancingClient.ReconnectAndRejoin())
        {
            Debug.LogError("ReconnectAndRejoin failed, trying Reconnect");
            if (!loadBalancingClient.ReconnectToMaster())
            {
                Debug.LogError("Reconnect failed, trying ConnectUsingSettings");
                if (!loadBalancingClient.ConnectUsingSettings(appSettings))
                {
                    Debug.LogError("ConnectUsingSettings failed");
                }
            }
            else
            {
                Debug.Log("ConnectUsingSetting sucessfull");
            }
        }
        else
        {
            Debug.Log("ReconnectAndRejoin Succes.");
            reconnecting = true;
            shouldReconnect = false;
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (reconnecting)
            Debug.LogError("Reconnection - onJoinRoomFailed" + message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if (reconnecting)
            Debug.LogError("Reconnection - onJoinRandomFailed" + message);
    }

    public override void OnJoinedRoom()
    {
        reconnecting = false;
        Debug.Log("Room joined.");
    }

    #region Unused Methods

    void IConnectionCallbacks.OnConnected()
    {
    }

    void IConnectionCallbacks.OnConnectedToMaster()
    {
    }

    void IConnectionCallbacks.OnRegionListReceived(RegionHandler regionHandler)
    {
    }

    void IConnectionCallbacks.OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }

    void IConnectionCallbacks.OnCustomAuthenticationFailed(string debugMessage)
    {
    }

    #endregion
}
