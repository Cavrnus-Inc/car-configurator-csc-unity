using System;
using System.Collections.Generic;
using System.Linq;
using CavrnusSdk.API;
using UnityBase;
using UnityEngine;

namespace CavrnusDemo
{
    public class SyncColors : MonoBehaviour
    {
        [SerializeField] private CavColorChangerUI cavColorChangerUI;
        
        [Header("Cav Properties")]
        [SerializeField] private string containerName;
        [SerializeField] private string propertyName;

        [Space]
        [SerializeField] private List<ColorTextureInfo> colorTextureInfo;
        
        private CavrnusSpaceConnection spaceConn;
        private IDisposable disp;
        
        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                this.spaceConn = spaceConn;

                if (cavColorChangerUI != null)
                    cavColorChangerUI.Setup(colorTextureInfo, Post);
                
                spaceConn.DefineColorPropertyDefaultValue(containerName, propertyName, Color.white);
                disp = spaceConn.BindColorPropertyValue(containerName, propertyName, serverColor => {
                    if (cavColorChangerUI != null) {
                        var foundItem = colorTextureInfo.FirstOrDefault(info => ColorsEqual(info.Color, serverColor));
                        if (foundItem != null)
                            cavColorChangerUI.SetSelected(foundItem);
                    }
                });
            });
        }
        
        private bool ColorsEqual(Color c1, Color c2, float tolerance = 0.1f)
        {
            return c1.r.AlmostEquals(c2.r, tolerance) &&
                   c1.g.AlmostEquals(c2.g, tolerance) &&
                   c1.b.AlmostEquals(c2.b, tolerance);
        }
        
        private void Post(ColorTextureInfo item)
        {
            spaceConn?.PostColorPropertyUpdate(containerName, propertyName, item.Color);
        }
        
        private void OnDestroy()
        {
            disp?.Dispose();
        }
    }
}