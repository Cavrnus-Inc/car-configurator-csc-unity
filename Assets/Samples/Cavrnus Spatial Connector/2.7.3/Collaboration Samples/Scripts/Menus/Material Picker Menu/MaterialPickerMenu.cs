using System.Collections.Generic;
using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusSdk.CollaborationExamples
{
    public class MaterialPickerMenu : MonoBehaviour
    {
        [Header("Sync Information - Matches desired sync component(s)")]
        [SerializeField] private string containerName;
        [SerializeField] private string propertyName;
        
        [Space] [SerializeField] private List<Material> materials;

        [Space] [SerializeField] private GameObject displayItemPrefab;
        [SerializeField] private Transform displayItemsContainer;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                foreach (var material in materials) {
                    var item = Instantiate(displayItemPrefab, displayItemsContainer);
                    item.GetComponent<MaterialPickerItem>().Setup(material, mat => {
                        spaceConn.PostStringPropertyUpdate(containerName, propertyName,mat.name);
                    });
                }
            });
        }
    }
}