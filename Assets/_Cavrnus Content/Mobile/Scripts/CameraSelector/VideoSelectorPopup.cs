using System.Collections.Generic;
using CavrnusSdk.API;
using UnityEngine;

namespace Cavrnus.UI
{
    public class VideoSelectorPopup : MonoBehaviour
    {
        [SerializeField] private DeviceSelectorPopupEntry entryPrefab;
        [SerializeField] private Transform entryParent;
        
        private List<CavrnusVideoInputDevice> videoInputs;
        private CavrnusSpaceConnection spaceConnection;
        
        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(OnSpaceConnection);
        }
        
        private void OnSpaceConnection(CavrnusSpaceConnection sc)
        {
            spaceConnection = sc;
            
            CavrnusFunctionLibrary.FetchVideoInputs(opts => {
                videoInputs = opts;
                for (var i = 0; i < opts.Count; i++) {
                    var go = Instantiate(entryPrefab, entryParent, true);
                    go.Setup(i, videoInputs[i].Name, DeviceSelected);
                }
            });
        }
        
        private void DeviceSelected(int id)
        {
            //Have we finished fetching the options?
            if (videoInputs == null) {
                spaceConnection.SetLocalUserStreamingState(false);
                return;
            }

            CavrnusFunctionLibrary.UpdateVideoInput(videoInputs[id]);
			
            spaceConnection.SetLocalUserStreamingState(true);
            
            PopupCanvas.Instance.Close();
        }
    }
}