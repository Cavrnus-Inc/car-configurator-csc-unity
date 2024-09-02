using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Cavrnus_Content.Scripts.VisionProComponents
{
    [RequireComponent(typeof(BoxCollider))]
    public class CavrnusVisionProSliderComponent : XRBaseInteractable
    {
        [Header("Slider Configuration")]
        [SerializeField]
        MeshRenderer m_FillRenderer;

        [SerializeField] float m_SliderIndirectChangeScale = 1f;
        [SerializeField] public FloatUnityEvent m_OnSliderValueChanged;
        
        public float SliderValue{ get; private set; }

        float m_BoxColliderSizeX;
        int m_PercentageId;
        Material m_MaterialInstance;
        Vector3 m_StartLocalGrabPos;
        float m_OnGrabStartPercentage;

        void Start()
        {
            m_MaterialInstance = m_FillRenderer.material;
            m_PercentageId = Shader.PropertyToID("_Percentage");
            m_BoxColliderSizeX = GetComponent<BoxCollider>().size.x;
        }

        /// <inheritdoc />
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            m_OnGrabStartPercentage = SliderValue;
            m_StartLocalGrabPos = transform.InverseTransformPoint(args.interactorObject.transform.position);
        }

        void Update()
        {
            if (!isSelected) return;

            var interactorSelecting = interactorsSelecting[0];
            var interactorPosition = interactorsSelecting[0].transform.position;

            if (interactorSelecting is IPokeStateDataProvider) { UpdateSliderAmtDirect(interactorPosition); }
            else { UpdateSliderAmtDelta(interactorPosition); }
        }

        public void SetFillPercentage(float percentage, bool notifyValue = true)
        {
            SliderValue = Mathf.Clamp01(percentage);
            m_MaterialInstance.SetFloat(m_PercentageId, MapRange(percentage, 0f, 1f, -0.01f, 1.01f));
           
            if (notifyValue)
                m_OnSliderValueChanged?.Invoke(1f - SliderValue);
        }

        void UpdateSliderAmtDirect(Vector3 interactorPos)
        {
            var localPosition = transform.InverseTransformPoint(interactorPos);
            var percentage = localPosition.x / m_BoxColliderSizeX + 0.5f;
            SetFillPercentage(percentage);
        }

        void UpdateSliderAmtDelta(Vector3 currentInteractorPos)
        {
            var currentLocalPos = transform.InverseTransformPoint(currentInteractorPos);
            var deltaVector = Vector3.ProjectOnPlane(currentLocalPos - m_StartLocalGrabPos, Vector3.up);
            var dx = deltaVector.x / m_BoxColliderSizeX;
            float newPercent = m_OnGrabStartPercentage + dx * m_SliderIndirectChangeScale;
            SetFillPercentage(newPercent);
        }

        float MapRange(float value, float inputMin, float inputMax, float outputMin, float outputMax)
        {
            return outputMin + ((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin));
        }
    }
}