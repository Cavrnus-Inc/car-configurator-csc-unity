using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Cavrnus.Chat
{
    [RequireComponent(typeof(TMP_InputField))]
    public class InputFieldHelper : MonoBehaviour
    {
        public UnityEvent<string> OnEndEdit;
        
        private TMP_InputField inputField;
        private void Awake()
        {
            inputField = GetComponent<TMP_InputField>();
            inputField.onSubmit.AddListener(Submit);
        }

        private void Update()
        {
            if (inputField.isFocused) {
                if (Input.GetKeyDown(KeyCode.Return)) {
                    Submit(inputField.text);
                }
            }
        }

        private void Submit(string val)
        {
            OnEndEdit?.Invoke(val);
        }

        private void OnDestroy()
        {
            inputField.onEndEdit.RemoveListener(Submit);
        }
    }
}