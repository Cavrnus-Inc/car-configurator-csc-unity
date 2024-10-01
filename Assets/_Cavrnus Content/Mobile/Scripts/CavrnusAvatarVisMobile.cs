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
                    StartCoroutine(CheckPlatformWithDelay(user, 3f));
                });
            });
        }

        private IEnumerator CheckPlatformWithDelay(CavrnusUser user, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            user.SpaceConnection.BeginTransientBoolPropertyUpdate(user.ContainerId, "AvatarVis", false);
        }
    }
}