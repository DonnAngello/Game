using UnityEngine;
using UnityEngine.UI;
using ReadyPlayerMe.Loader;
using ReadyPlayerMe.Core;
using ReadyPlayerMe.Core.Data;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;

namespace Game.AvatarEditor
{
    public class ReadyPlayerMeAvatar : MonoBehaviourPunCallbacks
    {
        private const string TAG = nameof(ReadyPlayerMeAvatar);
        private string avatarUrl = "";

        [SerializeField]
        private GameObject canvas;

        [SerializeField]
        private Transform playerParent;

        [SerializeField]
        private RuntimeAnimatorController animatorController;

        //[SerializeField]
        public PlayerMovement playerMovementScript;

        [Header("Outputs")]
        //public GameObject avatar;

        private Dictionary<string, GameObject> playersAvatarUrls = new Dictionary<string, GameObject>();

        public Dictionary<string, GameObject> playerAvatars = new Dictionary<string, GameObject>();

        private string currAvatarUrl;

        private void Start()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
        CoreSettings partner = CoreSettingsHandler.CoreSettings;        
        WebInterface.SetupRpmFrame(partner.Subdomain);
#endif
        }


        public void OnWebViewAvatarGenerated(string generatedUrl)
        {
            /*
            var avatarLoader = new AvatarObjectLoader();
            avatarUrl = generatedUrl;
            avatarLoader.OnCompleted += OnAvatarLoadCompleted;
            avatarLoader.OnFailed += OnAvatarLoadFailed;
            avatarLoader.LoadAvatar(avatarUrl);
            */
            avatarUrl = generatedUrl;
            PlayerSpawner.instance.SpawnPlayer(avatarUrl);
        }

        public void LoadAvatarInsidePlayer(GameObject player, string incomingUrl)
        {
            Debug.Log("Ucitavam");
            currAvatarUrl = incomingUrl;
            playersAvatarUrls[incomingUrl] = player;
            var avatarLoader = new AvatarObjectLoader();
            avatarLoader.OnCompleted += OnAvatarLoadCompleted;
            avatarLoader.OnFailed += OnAvatarLoadFailed;
            avatarLoader.LoadAvatar(incomingUrl);
        }
             

        private void OnAvatarLoadFailed(object sender, FailureEventArgs args)
        {
            Debug.Log("Avatar Load failed with error: " + args.Message);
            //SDKLogger.Log(TAG, $"Avatar Load failed with error: {args.Message}");
        }

        private void OnAvatarLoadCompleted(object sender, CompletionEventArgs args)
        {
            GameObject player = playersAvatarUrls[args.Url];
            if (player is null)
            {
                Debug.Log("Unsucessfull");
                return;
            }
                

            //if (avatar) Destroy(avatar);
            //avatar = args.Avatar;
            playerAvatars[currAvatarUrl] = args.Avatar;
            //avatar.transform.SetParent(playerParent);
            //avatar.transform.SetParent(player.transform);
            //avatar.transform.localPosition = new Vector3(0, -1, 0);
            playerAvatars[currAvatarUrl].transform.SetParent(player.transform);
            playerAvatars[currAvatarUrl].transform.localPosition = new Vector3(0, -1, 0);
            //avatar.transform.rotation = playerParent.rotation;
            //avatar.transform.rotation = player.transform.rotation;
            //avatar.GetComponent<Animator>().runtimeAnimatorController = animatorController;
            playerAvatars[currAvatarUrl].transform.rotation = player.transform.rotation;
            playerAvatars[currAvatarUrl].GetComponent<Animator>().runtimeAnimatorController = animatorController;
            playerMovementScript = player.GetComponent<PlayerMovement>();
            playerMovementScript.animator = playerAvatars[currAvatarUrl].GetComponent<Animator>();
            playerMovementScript.avatar = playerAvatars[currAvatarUrl].transform;
            playerMovementScript.enabled = true;
            //Launcher.instance.SetNickname(avatar.name);
            Debug.Log(playerAvatars[currAvatarUrl].transform.position);
        }

        public void OnCreateAvatar()
        {
            canvas.SetActive(false);
            //Launcher.instance.avatarLoaderScreen.SetActive(false);
#if !UNITY_EDITOR && UNITY_WEBGL
        WebInterface.SetIFrameVisibility(true);
#endif
        }
    }
}
