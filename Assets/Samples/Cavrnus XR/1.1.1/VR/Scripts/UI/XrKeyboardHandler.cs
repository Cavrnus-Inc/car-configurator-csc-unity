using System.Collections;
using CavrnusSdk.UI;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
using UnityBase;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CavrnusSdk.XR.UI
{
    public class XrKeyboardHandler : MonoBehaviour
    {
        [SerializeField] private NonNativeKeyboard keyboard;

        private void Awake()
        {
            CavrnusInputField.Selected += Selected;
            CavrnusInputField.DeSelected += DeSelected;
        }

        private void Selected(TMP_InputField newInputField)
        {
            keyboard.InputField = newInputField;
            newInputField.ActivateInputField();

            keyboard.PresentKeyboard();
        }

        private void DeSelected(TMP_InputField deselectedInputField)
        {
            StartCoroutine(DetermineChildRoutine());
        }

        private IEnumerator DetermineChildRoutine()
        {
            yield return null;

            var currSelected = EventSystem.current.currentSelectedGameObject;

            if (currSelected != null) {
                if (currSelected.IsDeepChildOf(keyboard.gameObject)) {
                    print(currSelected);
                    yield break;
                }
            }

            keyboard.Close();
        }

        private void OnDestroy()
        {
            CavrnusInputField.Selected -= Selected;
            CavrnusInputField.DeSelected -= DeSelected;
        }
    }
}