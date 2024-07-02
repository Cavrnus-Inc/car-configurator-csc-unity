using CavrnusSdk.API;
using System;
using System.Collections;
using System.Collections.Generic;
using Cavrnus.Chat;
using TMPro;
using UnityBase;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CavrnusSdk.UI
{
	public class MiniUserListEntry : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] private List<CanvasGroup> canvasGroups;
		
		[SerializeField] private TMP_Text nameText;
		[SerializeField] private RawImage videoStreamImage;
		[SerializeField] private Image profilePicImage;

		[SerializeField] private MiniUserListSpeakingPulse speakingPulse;
		[SerializeField] private GameObject mutedGameObject;
		
		private List<IDisposable> disposables = new List<IDisposable>();

		public void Setup(CavrnusUser user)
		{
			canvasGroups.ForEach(cg => cg.alpha = 0f);
						
			if (nameText != null) {
				var nameDisposable = user.BindUserName(n => nameText.text = n);
				disposables.Add(nameDisposable);
			}
		
            var picDisp = user.BindProfilePic(this, profilePic =>
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

			var muted = user.BindUserMuted(isMuted => mutedGameObject.SetActive(isMuted));
			disposables.Add(muted);
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

		public void OnPointerClick(PointerEventData eventData) { }

		
		private IEnumerator fadeOutRoutine;
		private IEnumerator fadeInRoutine;

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (fadeOutRoutine != null)
				StopCoroutine(fadeOutRoutine);

			fadeInRoutine = this.DoFade(canvasGroups, 0.1f, true);
			StartCoroutine(fadeInRoutine);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (fadeInRoutine != null)
				StopCoroutine(fadeInRoutine);
            
			fadeOutRoutine = this.DoFade(canvasGroups, 0.2f, false);
			StartCoroutine(fadeOutRoutine);
		}
	}
}