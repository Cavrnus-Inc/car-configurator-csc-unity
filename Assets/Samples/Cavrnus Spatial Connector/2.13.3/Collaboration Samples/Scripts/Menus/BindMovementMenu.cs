using System;
using CavrnusSdk.API;
using UnityEngine;
using UnityEngine.UI;

namespace CavrnusSdk.CollaborationExamples
{
    public class BindMovementMenu : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float moveAmount = 0.5f;

        [Space] 
        [SerializeField] private string containerName = "MoveObject";
        [SerializeField] private string propertyName = "Transform";

        [Space]
        [SerializeField] private Button buttonUp;
        [SerializeField] private Button buttonDown;
        [SerializeField] private Button buttonLeft;
        [SerializeField] private Button buttonRight;
        [SerializeField] private Button buttonReset;

        private IDisposable bind;
        private CavrnusSpaceConnection spaceConnection;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                spaceConnection = spaceConn;
                bind = spaceConn.BindTransformPropertyValue(containerName, propertyName, data => {
                    target.transform.localPosition = data.Position;
                    target.transform.localRotation = Quaternion.Euler(data.EulerAngles);
                    target.transform.localScale = data.Scale;
                });

                buttonUp.onClick.AddListener(ButtonUp);
                buttonDown.onClick.AddListener(ButtonDown);
                buttonLeft.onClick.AddListener(ButtonLeft);
                buttonRight.onClick.AddListener(ButtonRight);
                buttonReset.onClick.AddListener(ButtonReset);
            });
        }

        private void ButtonReset()
        {
            var data = GetCavTransformData(Vector3.zero);
            PostTransformUpdate(data);
        }

        private void ButtonRight()
        {
            var data = GetCavTransformData(target.localPosition + Vector3.right * moveAmount);
            PostTransformUpdate(data);
        }

        private void ButtonLeft()
        {
            var data = GetCavTransformData(target.localPosition + Vector3.left * moveAmount);
            PostTransformUpdate(data);
        }

        private void ButtonDown()
        {
            var data = GetCavTransformData(target.localPosition + Vector3.down * moveAmount);
            PostTransformUpdate(data);
        }

        private void ButtonUp()
        {
            var data = GetCavTransformData(target.localPosition + Vector3.up * moveAmount);
            PostTransformUpdate(data);
        }

        private CavrnusTransformData GetCavTransformData(Vector3 pos)
        {
            return new CavrnusTransformData(pos, transform.localRotation.eulerAngles, transform.localScale);
        }

        private void PostTransformUpdate(CavrnusTransformData data)
        {
            spaceConnection?.PostTransformPropertyUpdate(containerName, propertyName, data);
        }

        private void OnDestroy()
        {
            bind?.Dispose();

            buttonUp.onClick.RemoveListener(ButtonUp);
            buttonDown.onClick.RemoveListener(ButtonDown);
            buttonLeft.onClick.RemoveListener(ButtonLeft);
            buttonRight.onClick.RemoveListener(ButtonRight);
            buttonReset.onClick.RemoveListener(ButtonReset);
        }
    }
}