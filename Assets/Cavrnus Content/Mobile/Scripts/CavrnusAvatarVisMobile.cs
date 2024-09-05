using System.Collections;
using CavrnusSdk.API;
using UnityEngine;

namespace Cavrnus_Content.Mobile.Scripts
{
    public class CavrnusAvatarVisMobile : MonoBehaviour
    {
        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                sc.AwaitLocalUser(user => {
                    StartCoroutine(CheckPlatformWithDelay(user, 3f)); // Just to be extra certain...
                });
            });
        }

        private IEnumerator CheckPlatformWithDelay(CavrnusUser user, float delay)
        {
            yield return new WaitForSeconds(delay);

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Debug.Log("Running on a mobile device");
                user.SpaceConnection.BeginTransientBoolPropertyUpdate(user.ContainerId, "AvatarVis", false);
            }
            else
            {
                Debug.Log("Not running on a mobile device");
            }
        }
    }
}