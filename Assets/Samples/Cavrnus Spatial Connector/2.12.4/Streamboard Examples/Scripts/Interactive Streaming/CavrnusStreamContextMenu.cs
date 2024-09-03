using CavrnusSdk.API;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CavrnusSdk.StreamBoards
{
    public class CavrnusStreamContextMenu : MonoBehaviour
    {
        [SerializeField] private CavrnusStreamUserOption streamOptionPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private Transform noContentContainer;

        private CavrnusStreamBoard board;

        public void Setup(IEnumerable<CavrnusUser> users, Action<CavrnusUser> userSelected)
        {
            var filteredUsers = users.Where(user => user.GetUserStreaming()).ToList();

            if (filteredUsers.Count == 0)
                noContentContainer.gameObject.SetActive(true);
            else {
                noContentContainer.gameObject.SetActive(false);
                Instantiate(streamOptionPrefab, container).Setup(null, userSelected);
                foreach (var user in filteredUsers) {
                    var go = Instantiate(streamOptionPrefab, container);
                    go.Setup(user, userSelected);
                }
            }
        }

        public void Close() => Destroy(gameObject);
    }
}