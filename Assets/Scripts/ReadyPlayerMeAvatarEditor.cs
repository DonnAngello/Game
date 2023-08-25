using UnityEngine;
using UnityEngine.UI;
using ReadyPlayerMe.Core;
using ReadyPlayerMe.Core.Data;

namespace Game.AvatarEditor
{
    public class ReadyPlayerMeAvatar : MonoBehaviour
    {
        private const string TAG = nameof(ReadyPlayerMeAvatar);
        private string avatarUrl = "";

        [SerializeField]
        private GameObject canvas;

        public GameObject avatar;

        private void Start()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
        CoreSettings partner = CoreSettingsHandler.CoreSettings;
        
        WebInterface.SetupRpmFrame(partner.Subdomain);
#endif
        }


        public void OnWebViewAvatarGenerated(string generatedUrl)
        {
            var avatarLoader = new AvatarObjectLoader();
            avatarUrl = generatedUrl;
            avatarLoader.OnCompleted += OnAvatarLoadCompleted;
            avatarLoader.OnFailed += OnAvatarLoadFailed;
            avatarLoader.LoadAvatar(avatarUrl);
        }

        private void OnAvatarLoadFailed(object sender, FailureEventArgs args)
        {
            SDKLogger.Log(TAG, $"Avatar Load failed with error: {args.Message}");
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
            Debug.Log("On Avatar load completed.");
        }

        public void OnCreateAvatar()
        {
            canvas.SetActive(false);
#if !UNITY_EDITOR && UNITY_WEBGL
        WebInterface.SetIFrameVisibility(true);
#endif
        }
    }
}
