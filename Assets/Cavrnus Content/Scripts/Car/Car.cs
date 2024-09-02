using System;
using System.Collections.Generic;
using CavrnusDemo.SdkExtensions;
using CavrnusSdk.PropertySynchronizers;
using CavrnusSdk.API;
using UnityEngine;
using UnityEngine.UI;

namespace CavrnusDemo
{
    [RequireComponent(typeof(CavrnusPropertiesContainer))]
    public class Car : MonoBehaviour
    {
        [Header("Headlight & TailLights")]
        [SerializeField] private string headLightsPropertyName = "HeadlightsVis";
        
        [Space]
        [SerializeField] private GameObject headLightsGameObject;
        [SerializeField] private Material emissiveMaterial;
        [SerializeField] private string emissionColorMaterialProperty = "_EmissionColor";
        [SerializeField] private Toggle headLightsToggle;
        
        [Header("UnderGlow")]
        [SerializeField] private string underGlowPropertyNameVis = "UnderGlowVis";
        [SerializeField] private string underGlowPropertyNameColor = "UnderGlowColor";
        [SerializeField] private Toggle underGlowToggle;
        
        [Space]
        [SerializeField] private Light underGlowLight;
        [SerializeField] private GameObject underGlowGameObject;
        
        [Space]
        [SerializeField] private string explodeContainerName = "Explode";

        [Header("Animations")]
        [SerializeField] private string driverDoorPropertyNameAnimation = "DriverDoorAnimation";
        [SerializeField] private RotationAnimator driverDoor;
        
        [Space]
        [SerializeField] private string passengerDoorPropertyNameAnimation = "PassengerDoorAnimation";
        [SerializeField] private RotationAnimator passengerDoor;
        
        [Space]
        [SerializeField] private string trunkPropertyNameAnimation = "TrunkAnimation";
        [SerializeField] private RotationAnimator trunk;
        
        [Space]
        [SerializeField] private string carRotationName = "Rotation";
        
        [Space]
        [SerializeField] private string carScaleName = "Scale";
        
        private readonly List<IDisposable> disposables = new List<IDisposable>();
        private CavrnusSpaceConnection spaceConn;
        private CavrnusPropertiesContainer ctx;

        private void Start()
        {
            ctx = GetComponent<CavrnusPropertiesContainer>();

            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConn = sc;

                // Toggle Lights Vis
                emissiveMaterial.EnableKeyword("_EMISSION");
                spaceConn.DefineBoolPropertyDefaultValue(ctx.UniqueContainerName, headLightsPropertyName, headLightsGameObject.activeSelf);
                disposables.Add(spaceConn.BindBoolPropertyValue(ctx.UniqueContainerName, headLightsPropertyName, b => {
                    headLightsGameObject?.SetActive(b);
                    emissiveMaterial.SetColor(emissionColorMaterialProperty, Color.white * Mathf.Pow(2.0F, b ? 8 : 0));

                    headLightsToggle?.SetIsOnWithoutNotify(b);
                }));
                
                // Underglow Vis
                spaceConn.DefineBoolPropertyDefaultValue(ctx.UniqueContainerName, underGlowPropertyNameVis, underGlowGameObject.activeSelf);
                disposables.Add(spaceConn.BindBoolPropertyValue(ctx.UniqueContainerName, underGlowPropertyNameVis, b => {
                    underGlowGameObject?.SetActive(b);
                    underGlowToggle?.SetIsOnWithoutNotify(b);
                }));
                
                // Underglow Color
                spaceConn.DefineColorPropertyDefaultValue(ctx.UniqueContainerName, underGlowPropertyNameColor, underGlowLight.color);
                disposables.Add(spaceConn.BindColorPropertyValue(ctx.UniqueContainerName, underGlowPropertyNameColor, c => {
                    underGlowLight.color = c;
                }));
                
                // Driver Door Animation
                spaceConn.DefineBoolPropertyDefaultValue(ctx.UniqueContainerName, driverDoorPropertyNameAnimation, false);
                disposables.Add(spaceConn.BindBoolPropertyValue(ctx.UniqueContainerName, driverDoorPropertyNameAnimation, b => {
                    driverDoor.SetState(b);
                }));
                
                // Passenger Door Animation
                spaceConn.DefineBoolPropertyDefaultValue(ctx.UniqueContainerName, passengerDoorPropertyNameAnimation, false);
                disposables.Add(spaceConn.BindBoolPropertyValue(ctx.UniqueContainerName, passengerDoorPropertyNameAnimation, b => {
                    passengerDoor.SetState(b);
                }));
                
                // Trunk Animation
                spaceConn.DefineBoolPropertyDefaultValue(ctx.UniqueContainerName, trunkPropertyNameAnimation, false);
                disposables.Add(spaceConn.BindBoolPropertyValue(ctx.UniqueContainerName, trunkPropertyNameAnimation, b => {
                    trunk.SetState(b);
                }));
                
                // Car Explode
                spaceConn.DefineBoolPropertyDefaultValue(ctx.UniqueContainerName, explodeContainerName, false);
                disposables.Add(spaceConn.BindBoolPropertyValue(ctx.UniqueContainerName, explodeContainerName, b => {
                    SetExplode(b);
                }));
                
                spaceConn.DefineFloatPropertyDefaultValue(ctx.UniqueContainerName, carRotationName, 0f);
                disposables.Add(spaceConn.BindFloatPropertyValue(ctx.UniqueContainerName, carRotationName, f => {
                    RotateCar(f);
                }));
                
                spaceConn.DefineFloatPropertyDefaultValue(ctx.UniqueContainerName, carScaleName, 0f);
                disposables.Add(spaceConn.BindFloatPropertyValue(ctx.UniqueContainerName, carScaleName, f => {
                    ScaleCar(f);
                }));
                
                headLightsToggle?.onValueChanged.AddListener(ToggleCarLights);
                underGlowToggle?.onValueChanged.AddListener(ToggleUnderGlow);
            });
        }

        public void SetExplode(bool state)
        {
            spaceConn.PostBoolPropertyUpdate(ctx.UniqueContainerName, driverDoorPropertyNameAnimation,state);
            spaceConn.PostBoolPropertyUpdate(ctx.UniqueContainerName, passengerDoorPropertyNameAnimation, state);
            spaceConn.PostBoolPropertyUpdate(ctx.UniqueContainerName, trunkPropertyNameAnimation, state);
        }
        
        public void ToggleDriverDoorAnim()
        {
            if (spaceConn == null) return;
            
            var current = spaceConn.GetBoolPropertyValue(ctx.UniqueContainerName, driverDoorPropertyNameAnimation);
            spaceConn.PostBoolPropertyUpdate(ctx.UniqueContainerName, driverDoorPropertyNameAnimation, !current);
        }
        
        public void TogglePassengerDoorAnim()
        {
            if (spaceConn == null) return;
            
            var current = spaceConn.GetBoolPropertyValue(ctx.UniqueContainerName, passengerDoorPropertyNameAnimation);
            spaceConn.PostBoolPropertyUpdate(ctx.UniqueContainerName, passengerDoorPropertyNameAnimation, !current);
        }
        
        public void ToggleTrunkAnim()
        {
            if (spaceConn == null) return;
            
            var current = spaceConn.GetBoolPropertyValue(ctx.UniqueContainerName, trunkPropertyNameAnimation);
            spaceConn.PostBoolPropertyUpdate(ctx.UniqueContainerName, trunkPropertyNameAnimation, !current);
        }
        
        public void ToggleUnderGlow()
        {
            if (spaceConn == null) return;
            
            var current = spaceConn.GetBoolPropertyValue(ctx.UniqueContainerName, underGlowPropertyNameVis);
            spaceConn.PostBoolPropertyUpdate(ctx.UniqueContainerName, underGlowPropertyNameVis, !current);
        }
        
        public void ToggleUnderGlow(bool state)
        {
            spaceConn?.PostBoolPropertyUpdate(ctx.UniqueContainerName, underGlowPropertyNameVis, state);
        }

        public void ToggleCarLights(bool state)
        {
            spaceConn?.PostBoolPropertyUpdate(ctx.UniqueContainerName, headLightsPropertyName, state);
        }
        
        public void ToggleCarLights()
        {
            if (spaceConn == null) return;
            
            var current = spaceConn.GetBoolPropertyValue(ctx.UniqueContainerName, headLightsPropertyName);
            spaceConn.PostBoolPropertyUpdate(ctx.UniqueContainerName, headLightsPropertyName, !current);
        }

        public void RotateCar(float val)
        {
            transform.Rotate(Vector3.up, val);
        }
        
        public void ScaleCar(float val)
        {
            transform.localScale = Vector3.one * val;
        }

        private void OnDestroy()
        {
            disposables.ForEach(d => d.Dispose());
            
            headLightsToggle.onValueChanged.RemoveListener(ToggleCarLights);
            underGlowToggle.onValueChanged.RemoveListener(ToggleUnderGlow);
        }
    }
}