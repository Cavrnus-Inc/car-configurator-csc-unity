using System;
using CavrnusDemo.CavrnusDataObjects;
using CavrnusSdk.API;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace CavrnusDemo
{
    /// <summary>
    /// Toggle component that can be poked or indirectly interacted with to toggle its state.
    /// </summary>
    public class CavrnusVisionProToggleComponent : XRBaseInteractable
    {
        [SerializeField] private BoolCavrnusPropertyObject boolProperty;
        
        [Header("Toggle References")]
        [SerializeField]
        Transform m_ToggleBubble;
        
        [SerializeField]
        MeshRenderer m_ToggleBackground;

        [Header("Toggle Colors")]
        [SerializeField]
        Color m_SelectedColor = new Color(.1254f, .5882f, .9529f);
        
        [SerializeField]
        Color m_UnselectedColor = new Color(.1764f, .1764f, .1764f);

        [Header("Toggle Animation")]
        [SerializeField]
        float m_TransitionDuration = 0.15f;

        [Header("Toggle Events")]
        [SerializeField]
        BoolUnityEvent m_OnToggleChanged;

        bool m_IsOn;
        bool m_IsAnimating;
        Vector3 m_BubbleOnTargetPosition;
        Vector3 m_BubbleOffTargetPosition;

        float m_StartLerpTime;

        const float k_BubbleOnPosition = -0.17f;
        const float k_BubbleOffPosition = 0.17f;

        Material m_MaterialInstance;

        private CavrnusSpaceConnection spaceConnection;
        private IDisposable binding;

        private void Start()
        {
            var bubblePosition = m_ToggleBubble.localPosition;
            m_BubbleOnTargetPosition = new Vector3(k_BubbleOnPosition, bubblePosition.y, bubblePosition.z);
            m_BubbleOffTargetPosition = new Vector3(k_BubbleOffPosition, bubblePosition.y, bubblePosition.z);
            
            m_MaterialInstance = m_ToggleBackground.material;
            
            SetBubbleColor(m_IsOn ? m_SelectedColor : m_UnselectedColor);
            
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc =>
            {
                spaceConnection = sc;
                sc.DefineBoolPropertyDefaultValue(boolProperty.ContainerName, boolProperty.PropertyName, boolProperty.DefaultValue);
                binding = sc.BindBoolPropertyValue(boolProperty.ContainerName, boolProperty.PropertyName, b =>
                {
                    Debug.Log($"Hitting binding!: {b}");

                    SetState(b);
                });
            });
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            if (spaceConnection == null) return;

            var serverVal = spaceConnection.GetBoolPropertyValue(boolProperty.ContainerName, boolProperty.PropertyName);
            spaceConnection.PostBoolPropertyUpdate(boolProperty.ContainerName, boolProperty.PropertyName, !serverVal);
            
            Debug.Log($"INTERACTED WITH: {serverVal}");
        }

        private void SetState(bool state)
        {
            m_IsOn = state;
            m_StartLerpTime = Time.time;
            m_IsAnimating = true;
            m_OnToggleChanged?.Invoke(m_IsOn);
        }
        
        private void Update()
        {
            if (!m_IsAnimating)
                return;
            
            var lerpPercentage =  Mathf.Clamp01(Time.time - m_StartLerpTime) / m_TransitionDuration;

            m_ToggleBubble.localPosition = Vector3.Lerp(
                m_IsOn ? m_BubbleOffTargetPosition : m_BubbleOnTargetPosition,
                m_IsOn ? m_BubbleOnTargetPosition : m_BubbleOffTargetPosition,
                lerpPercentage);

            SetBubbleColor(Color.Lerp(m_IsOn ? m_UnselectedColor : m_SelectedColor,
                m_IsOn ? m_SelectedColor : m_UnselectedColor,
                lerpPercentage));

            if (lerpPercentage >= 1f)
                m_IsAnimating = false;
        }

        private void SetBubbleColor(Color color)
        {
            m_MaterialInstance.color = color;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            binding?.Dispose();
        }
    }
}
