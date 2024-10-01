using System;
using System.Collections;
using System.Collections.Generic;
using Cavrnus_Content.Mobile.Scripts.UI;
using Cavrnus.UI;
using CavrnusSdk.API;
using TMPro;
using UnityBase;
using UnityEngine;
using UnityEngine.UI;

namespace Cavrnus_Content.Mobile.Scripts
{
    public class MobileUserListEntry : MonoBehaviour
    {
	    [SerializeField] private VideoSelectorPopup videoDeviceSelectorPopupPrefab;
	    [SerializeField] private CavrnusBoundOnlyPropertyToggle videoToggle;
	    
		[SerializeField] private TMP_Text nameText;
		[SerializeField] private RawImage videoStreamImage;
		[SerializeField] private Image profilePicImage;

		[SerializeField] private Transform localUserActionContainer;

		[SerializeField] private MiniUserListSpeakingPulse speakingPulse;
		[SerializeField] private GameObject mutedGameObject;
		
		private List<IDisposable> disposables = new List<IDisposable>();

		private CavrnusUser user;

		public void Setup(CavrnusUser user)
		{
			this.user = user;
			
			localUserActionContainer.gameObject.SetActive(user.IsLocalUser);
			if (user.IsLocalUser) {
				SetupLocalUser();
			}
			
			if (nameText != null) {
				var nameDisposable = user.BindUserName(n => nameText.text = n);
				disposables.Add(nameDisposable);
			}
		
            var picDisp = user.BindProfilePic(profilePic =>
            {
                profilePicImage.sprite = profilePic;
                if (profilePic != null)
                    profilePicImage.GetComponent<AspectRatioFitter>().aspectRatio =
                        (float)profilePic.texture.width / (float)profilePic.texture.height;
            });
            disposables.Add(picDisp);

            var isStreaming = user.BindUserStreaming(isStreaming => videoStreamImage.gameObject.SetActive(isStreaming));
			disposables.Add(isStreaming);
			
			var isSpeaking = user.BindUserSpeaking(isSpeaking => speakingPulse.IsSpeaking = isSpeaking);
			disposables.Add(isSpeaking);

            var videoDisp = user.BindUserVideoFrames(tex => {
                StartCoroutine(AssignVidTexture(tex));
			});
			disposables.Add(videoDisp);

			if (mutedGameObject) {
				var muted = user.BindUserMuted(isMuted => mutedGameObject.SetActive(isMuted));
				disposables.Add(muted);
			}
		}

		private void SetupLocalUser()
		{
			videoToggle.OnClicked.AddListener(() => {
				//Only open if we aren't currently streaming
				if (!user.GetUserStreaming()) {
					PopupCanvas.Instance.OpenPopup(Instantiate(videoDeviceSelectorPopupPrefab).gameObject);
				}
				else {
					user.SpaceConnection.SetLocalUserStreamingState(false);
				}
			});
		}

		private IEnumerator AssignVidTexture(TextureWithUVs tex)
		{
			if (tex.Texture.width > 0 && tex.Texture.height > 0)
				videoStreamImage.GetComponent<AspectRatioFitter>().aspectRatio =
					(float) tex.Texture.width / (float) tex.Texture.height;
			else
				videoStreamImage.GetComponent<AspectRatioFitter>().aspectRatio = 1.5f;
			
			yield return new WaitForSeconds(1f); // Need delay to handle if user is already streaming when loading space

			videoStreamImage.texture = tex.Texture;
			videoStreamImage.uvRect = tex.UVRect;
		}
		
		private void OnDestroy()
		{
			foreach (var disp in disposables) 
				disp.Dispose();
		}
    }
}