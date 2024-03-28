using UnityEngine;
using CavrnusSdk.PropertySynchronizers;

namespace CavrnusDemo
{
    public class SyncEmissiveColor : CavrnusColorPropertySynchronizer
    {
        [SerializeField] private Material emissiveMaterial;
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            emissiveMaterial.EnableKeyword("_EMISSION");
        }

        public override Color GetValue()
        {
            return emissiveMaterial.GetColor(EmissionColor);
        }

        public override void SetValue(Color value)
        {
            emissiveMaterial.SetColor(EmissionColor, value);
        }
    }
}