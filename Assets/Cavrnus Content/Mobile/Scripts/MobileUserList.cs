using System.Collections.Generic;
using CavrnusSdk.API;
using UnityEngine;

namespace Cavrnus_Content.Mobile.Scripts
{
    public class MobileUserList : MonoBehaviour
    {
        [SerializeField] private GameObject entryPrefab;
        [SerializeField] private Transform container;

        private readonly Dictionary<string, MobileUserListEntry> menuInstances = new Dictionary<string, MobileUserListEntry>();

        private CavrnusSpaceConnection spaceConn;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConn = sc;
                spaceConn.BindSpaceUsers(UserAdded, UserRemoved);
            });
        }

        private void UserAdded(CavrnusUser user)
        {
            var go = Instantiate(entryPrefab, container);
            menuInstances[user.ContainerId] = go.GetComponent<MobileUserListEntry>();
            menuInstances[user.ContainerId].Setup(user);
        }

        private void UserRemoved(CavrnusUser user)
        {
            if (menuInstances.ContainsKey(user.ContainerId)) {
                Destroy(menuInstances[user.ContainerId].gameObject);
                menuInstances.Remove(user.ContainerId);
            }
        }
    }
}