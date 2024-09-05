using System;
using CavrnusSdk.API;
using TMPro;
using UnityEngine;

namespace CavrnusSdk.StreamBoards
{
    public class CavrnusStreamUserOption : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI userName;

        private CavrnusUser user;
        private Action<CavrnusUser> onUserSelected;
        private IDisposable disp;

        public void Setup(CavrnusUser user, Action<CavrnusUser> onUserSelected)
        {
            this.user = user;
            this.onUserSelected = onUserSelected;

            if (user == null) {

                userName.text = "Reset Stream";
                return;
            }

            disp = user.BindUserName(n => userName.text = n);
        }

        private void OnDestroy()
        {
            disp?.Dispose();
        }

        public void SelectUser()
        {
            onUserSelected?.Invoke(user);
        }
    }
}