using System;
using System.Collections;
using System.Collections.Generic;
using CavrnusSdk.API;
using Collab.Base.Collections;
using Collab.LiveRoomSystem.LiveObjectManagement.ObjectTypeManagers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace Cavrnus.Chat
{
    public class ChatMenuEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("Chat Metadata References")]
        [SerializeField] private TextMeshProUGUI creatorName;
        [SerializeField] private TextMeshProUGUI creationTime;
        [SerializeField] private TextMeshProUGUI message;
        [SerializeField] private Image profilePicImage;
        
        [Header("Chat Visuals")]
        [SerializeField] private Image chatBubbleBackground;
        [SerializeField] private Color localUserColor;
        
        [Header("Chat Hover")]
        [SerializeField] private List<CanvasGroup> rootCanvasGroup;
        [SerializeField] private List<CanvasGroup> extraButtonsCanvasGroup;
        
        [Header("Layout Components")]
        [SerializeField] private HorizontalOrVerticalLayoutGroup rootLayoutElement;
        [SerializeField] private HorizontalOrVerticalLayoutGroup innerLayoutElement;
        [SerializeField] private HorizontalOrVerticalLayoutGroup chatContentLayoutElement;
        [SerializeField] private HorizontalOrVerticalLayoutGroup extrasContentLayoutElement;
        [SerializeField] private HorizontalOrVerticalLayoutGroup metadataContainer;
        
        private readonly List<IDisposable> disposables = new List<IDisposable>();
        
        private IChatViewModel chat;
        private Action<IChatViewModel> onDelete;
        
        public void Setup(CavrnusSpaceConnection spaceConn, IChatViewModel chat, Action<IChatViewModel> onDelete)
        {
            this.chat = chat;
            this.onDelete = onDelete;

            disposables.Add(spaceConn.BindSpaceUsers(UserAdded, UserRemoved));
            disposables.Add(chat.TextSource.Bind(msg => message.text = msg));
            disposables.Add(chat.CreateTime.Bind(msg => creationTime.text = UnityBase.HelperFunctions.ToPrettyDay(msg.ToLocalTime())));

            StartCoroutine(AnimationHelper.DoFade(rootCanvasGroup, 1f, true));
            extraButtonsCanvasGroup.ForEach(cg => cg.alpha = 0f);
        }
        
        private void UserAdded(CavrnusUser u)
        {
            if (u.UserId == chat.CreatorId.Value) {
                disposables.Add(u.BindUserName(n => creatorName.text = n));
                disposables.Add(u.BindProfilePic(this, profilePic =>
                {
                    profilePicImage.sprite = profilePic;
                    if (profilePic != null)
                        profilePicImage.GetComponent<AspectRatioFitter>().aspectRatio =
                            (float)profilePic.texture.width / (float)profilePic.texture.height;
                }));
                
                if (u.IsLocalUser) {
                    rootLayoutElement.reverseArrangement = true;
                    rootLayoutElement.childAlignment = TextAnchor.UpperRight;
                    innerLayoutElement.childAlignment = TextAnchor.UpperRight;
                    chatContentLayoutElement.childAlignment = TextAnchor.UpperRight;
                    extrasContentLayoutElement.childAlignment = TextAnchor.MiddleLeft;
                    metadataContainer.reverseArrangement = true;
                    
                    chatBubbleBackground.color = localUserColor;
                }
            }
        }
        
        public void RemoveChatButtonClick()
        {
            onDelete?.Invoke(chat);
        }

        private void UserRemoved(CavrnusUser u)
        {
            
        }
        
        private IEnumerator fadeInRoutine;
        private IEnumerator fadeOutRoutine;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (fadeOutRoutine != null)
             StopCoroutine(fadeOutRoutine);

            fadeInRoutine = AnimationHelper.DoFade(extraButtonsCanvasGroup, 0.1f, true);
            StartCoroutine(fadeInRoutine);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (fadeInRoutine != null)
                StopCoroutine(fadeInRoutine);
            
            fadeOutRoutine = AnimationHelper.DoFade(extraButtonsCanvasGroup, 0.2f, false);
            StartCoroutine(fadeOutRoutine);
        }

        public void OnPointerClick(PointerEventData eventData) { }

        private void OnDestroy()
        {
            disposables.ForEach(d => d.Dispose());
        }
    }
}