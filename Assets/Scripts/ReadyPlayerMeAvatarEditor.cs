using UnityEngine;
using UnityEngine.UI;
using ReadyPlayerMe.Loader;
using ReadyPlayerMe.Core;
using ReadyPlayerMe.Core.Data;
using Photon.Pun;
using Photon.Realtime;

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

        [SerializeField]
        private PlayerMovement playerMovementScript;

        [Header("Outputs")]
        public GameObject avatar;

        private void Start()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
        CoreSettings partner = CoreSettingsHandler.CoreSettings;
        
        WebInterface.SetupRpmFrame(partner.Subdomain);
#endif

            //playerMovementScript = PlayerSpawner.instance.GetPlayer().GetComponent<PlayerMovement>();
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
            Debug.Log(avatarUrl);
            Debug.Log(photonView);
            photonView.RPC("SetPlayer", RpcTarget.AllBuffered, avatarUrl);
            //GetComponent<PhotonView>().RPC("SetPlayer", RpcTarget.All, avatarUrl);
        }

        [PunRPC]
        private void SetPlayer(string incomingUrl)
        {
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
            /*
            if (avatar) Destroy(avatar);
            avatar = args.Avatar;
            if (args.Metadata.BodyType == BodyType.HalfBody)
            {
                avatar.transform.position = new Vector3(0, 1, 0);
            }
            */

            if (avatar) Destroy(avatar);
            avatar = args.Avatar;
            avatar.transform.SetParent(playerParent);
            //avatar.transform.SetParent(PlayerSpawner.instance.player.transform);
            avatar.transform.localPosition = new Vector3(0, -1, 0);
            avatar.transform.rotation = playerParent.rotation;
            //avatar.transform.rotation = PlayerSpawner.instance.player.transform.rotation;
            avatar.GetComponent<Animator>().runtimeAnimatorController = animatorController;
            //playerMovementScript = PlayerSpawner.instance.player.GetComponent<PlayerMovement>();
            playerMovementScript.animator = avatar.GetComponent<Animator>();
            playerMovementScript.avatar = avatar.transform;
            playerMovementScript.enabled = true;
            //Launcher.instance.SetNickname(avatar.name);
            Debug.Log(avatar.transform.position);
            //Launcher.instance.mainMenuScreen.SetActive(true);
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
