using System.Collections.Generic;
using UnityEngine;
using CavrnusSdk.API;

namespace CavrnusSdk.UI
{
    public class MiniUserList : MonoBehaviour
    {
        [SerializeField] private GameObject entryPrefab;
        [SerializeField] private Transform container;

        private readonly Dictionary<string, MiniUserListEntry> menuInstances = new Dictionary<string, MiniUserListEntry>();

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
            menuInstances[user.ContainerId] = go.GetComponent<MiniUserListEntry>();
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