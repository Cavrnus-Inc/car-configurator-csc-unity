using CavrnusSdk.XR.UI;
using UnityEngine;

namespace CavrnusSdk.XR
{
    public class XrMainInit : MonoBehaviour
    {
        [SerializeField] private XrToolbarMenu xrToolbarMenu;
        
        private void Start()
        {
            xrToolbarMenu.Setup();
        }
    }
}