using System;
using System.Collections.Generic;
using CavrnusSdk.UI;
using Collab.Base.Collections;
using UnityEngine;
using UnityEngine.UI;
using CavrnusSdk.API;

namespace CavrnusSdk.XR.UI
{
    public class XrToolbarMenu : MonoBehaviour
    {
        [SerializeField] private Transform menusContainer;
        [SerializeField] private Transform toolBarContainer;
        
        [Space] 
        [SerializeField] private Image spacePickerMenuOpen;
        [SerializeField] private Image settingsMenuOpen;
        [SerializeField] private Image usersMenuOpen;
        [SerializeField] private Image colorPickerMenuOpen;
        [SerializeField] private Image chatMenuOpen;

        [Space]
        [SerializeField] private WidgetUserProfileImage widgetUserProfileImageWidget;
        [SerializeField] private WidgetUserMic widgetUserMicWidget;
        
        private readonly List<IDisposable> disposables = new List<IDisposable>();
        
        public void Setup()
        {
			// Toolbar is hidden by default
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => sc.AwaitLocalUser(OnLocalUser));

            // Set initial vis if in space or not
            var isInSpace = CavrnusFunctionLibrary.IsConnectedToAnySpace();
            toolBarContainer.gameObject.SetActive(isInSpace);
            
            disposables.Add(CavrnusMenuManager.Instance.GetMenuSetting("UsersMenu").Bind(vis => usersMenuOpen.gameObject.SetActive(vis)));
            disposables.Add(CavrnusMenuManager.Instance.GetMenuSetting("SettingsMenu").Bind(vis => settingsMenuOpen.gameObject.SetActive(vis)));
            disposables.Add(CavrnusMenuManager.Instance.GetMenuSetting("SpacePickerMenu").Bind(vis => spacePickerMenuOpen.gameObject.SetActive(vis)));
            disposables.Add(CavrnusMenuManager.Instance.GetMenuSetting("ColorPickerMenu").Bind(vis => colorPickerMenuOpen.gameObject.SetActive(vis)));
            disposables.Add(CavrnusMenuManager.Instance.GetMenuSetting("ChatMenu").Bind(vis => chatMenuOpen.gameObject.SetActive(vis)));
        }

        public void OpenChatMenu() => CavrnusMenuManager.Instance.ToggleMenu("ChatMenu", menusContainer);
        public void OpenColorPicker() => CavrnusMenuManager.Instance.ToggleMenu("ColorPickerMenu", menusContainer);
        public void OpenSettings() => CavrnusMenuManager.Instance.ToggleMenu("SettingsMenu", menusContainer);
        
        public void OpenUsersMenu() => CavrnusMenuManager.Instance.ToggleMenu("UsersMenu", menusContainer);
        
        public void ExitSpace() => spaceConn.ExitSpace();

        private CavrnusSpaceConnection spaceConn;

        private void OnLocalUser(CavrnusUser user)
        {
            spaceConn = user.SpaceConnection;
            toolBarContainer.gameObject.SetActive(true);

            // Setup widget components
			// widgetUserMicWidget.Setup(user);
			widgetUserProfileImageWidget.Setup(user);
		}
        
        private void OnDestroy()
        {
            foreach (var d in disposables)
                d.Dispose();
        }
    }
}