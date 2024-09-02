using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Cavrnus_Content.Scripts.VisionProComponents
{
    public class CavrnusVisionProToggleComponent : XRBaseInteractable
    {
        [Header("Toggle References")]
        [SerializeField]
        Transform m_ToggleBubble;

        [SerializeField] MeshRenderer m_ToggleBackground;

        [Header("Toggle Colors")]
        [SerializeField]
        Color m_SelectedColor = new Color(.1254f, .5882f, .9529f);

        [SerializeField] Color m_UnselectedColor = new Color(.1764f, .1764f, .1764f);

        [Header("Toggle Animation")]
        [SerializeField]
        float m_TransitionDuration = 0.15f;

        [Header("Toggle Events")]
        [SerializeField] public BoolUnityEvent OnToggleChanged;

        public bool IsOn{ get; set; }
        
        bool m_IsAnimating;
        Vector3 m_BubbleOnTargetPosition;
        Vector3 m_BubbleOffTargetPosition;

        float m_StartLerpTime;

        const float k_BubbleOnPosition = -0.17f;
        const float k_BubbleOffPosition = 0.17f;

        Material m_MaterialInstance;

        void Start()
        {
            var bubblePosition = m_ToggleBubble.localPosition;
            m_BubbleOnTargetPosition = new Vector3(k_BubbleOnPosition, bubblePosition.y, bubblePosition.z);
            m_BubbleOffTargetPosition = new Vector3(k_BubbleOffPosition, bubblePosition.y, bubblePosition.z);

            m_MaterialInstance = m_ToggleBackground.material;
            SetBubbleColor(IsOn ? m_SelectedColor : m_UnselectedColor);
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            IsOn = !IsOn;
            m_StartLerpTime = Time.time;
            m_IsAnimating = true;
            OnToggleChanged?.Invoke(IsOn);
        }

        void Update()
        {
            if (!m_IsAnimating) return;

            var lerpPercentage = Mathf.Clamp01(Time.time - m_StartLerpTime) / m_TransitionDuration;

            m_ToggleBubble.localPosition = Vector3.Lerp(IsOn ? m_BubbleOffTargetPosition : m_BubbleOnTargetPosition,
                                                        IsOn ? m_BubbleOnTargetPosition : m_BubbleOffTargetPosition,
                                                        lerpPercentage);

            SetBubbleColor(Color.Lerp(IsOn ? m_UnselectedColor : m_SelectedColor,
                                      IsOn ? m_SelectedColor : m_UnselectedColor,
                                      lerpPercentage));

            if (lerpPercentage >= 1f) m_IsAnimating = false;
        }

        void SetBubbleColor(Color color) { m_MaterialInstance.color = color; }
    }
}