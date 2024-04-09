using System;
using CavrnusSdk.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CavrnusSdk.CollaborationExamples
{
    public class BindStringMenu : MonoBehaviour
    {
        [SerializeField] private Button addButton;
        [SerializeField] private Button removeButton;
        [SerializeField] private TextMeshProUGUI tmPro;

        private readonly string containerName = "StringExample";
        private readonly string propertyName = "String";

        private CavrnusSpaceConnection spaceConn;

        private IDisposable bind;
        
        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                this.spaceConn = spaceConn;
                
                spaceConn.DefineStringPropertyDefaultValue(containerName, propertyName, tmPro.text);
                bind = spaceConn.BindStringPropertyValue(containerName, propertyName, newString => {
                    tmPro.text = newString;
                });
                
                addButton.onClick.AddListener(AddButtonClicked);
                removeButton.onClick.AddListener(RemoveButtonClicked);
            });
        }

        private void RemoveButtonClicked()
        {
            var current = GetCurrentString();
            if (current.Length != 0)
                PostStringUpdate(current.Remove(current.Length - 1));
        }

        private void AddButtonClicked()
        {
            var current = GetCurrentString();
            PostStringUpdate(current + "!");
        }

        private string GetCurrentString() => spaceConn.GetStringPropertyValue(containerName, propertyName);
        
        private void PostStringUpdate(string newString) => spaceConn.PostStringPropertyUpdate(containerName, propertyName, newString);

        private void OnDestroy()
        {
            bind?.Dispose();
            addButton.onClick.RemoveListener(AddButtonClicked);
            removeButton.onClick.RemoveListener(RemoveButtonClicked);
        }
    }
}