using System;
using System.Collections;
using System.Collections.Generic;
using CavrnusSdk.API;
using Collab.LiveRoomSystem.LiveObjectManagement.ObjectTypeManagers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cavrnus.Chat
{
    public class ChatMenu : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button submitButton;
        [SerializeField] private InputFieldHelper inputFieldHelper;
        
        [Header("Chat Entries")]
        [SerializeField] private Transform chatEntryContainer;
        [SerializeField] private GameObject chatEntryPrefab;

        private readonly Dictionary<string, GameObject> createdChats = new Dictionary<string, GameObject>();
        private CavrnusSpaceConnection spaceConn;
        private CavrnusUser localUser;

        private readonly List<IDisposable> disposables = new List<IDisposable>();

        private void Start()
        {
            submitButton.interactable = false;
            
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                spaceConn.AwaitLocalUser(lu => {
                    this.spaceConn = spaceConn;
                    localUser = lu;
                    
                    disposables.Add(spaceConn.BindChatMessages(MessagesOnItemAddedEvent, MessagesOnItemRemovedEvent));
                    
                    submitButton.onClick.AddListener(SubmitChat);
                    
                    inputField.onValueChanged.AddListener(OnInputChanged);
                    inputFieldHelper.OnEndEdit.AddListener(OnInputFieldSubmit);
                });
            });
        }

        private void OnInputFieldSubmit(string arg0)
        {
            SubmitChat();
        }

        private void OnInputChanged(string input)
        {
            submitButton.interactable = !string.IsNullOrWhiteSpace(input);
        }
        
        private void MessagesOnItemAddedEvent(IChatViewModel item)
        {
            var chatObj = Instantiate(chatEntryPrefab, chatEntryContainer);
            chatObj.GetComponent<ChatMenuEntry>().Setup(spaceConn, item, DeleteChat);
            
            createdChats.Add(item.ObjectProperties.Id, chatObj);
        }
        
        private void DeleteChat(IChatViewModel chat)
        {
            // Not needed atm...
        }

        private void MessagesOnItemRemovedEvent(IChatViewModel item)
        {
            var obj = createdChats[item.ObjectProperties.Id];
            
            Destroy(obj);
            createdChats.Remove(item.ObjectProperties.Id);
        }

        private void SubmitChat()
        {
            if (!string.IsNullOrWhiteSpace(inputField.text) && !string.IsNullOrEmpty(inputField.text))
                spaceConn.PostChatMessage(localUser, inputField.text);

            inputField.text = string.Empty;

            StartCoroutine(DoFocus());
        }

        private IEnumerator DoFocus()
        {
            yield return null;

            inputField.ActivateInputField();
        }
        
        private void OnDestroy()
        {
            submitButton.onClick.RemoveListener(SubmitChat);
            inputField.onValueChanged.RemoveListener(OnInputChanged);
        }
    }
}