using System;
using UnityEngine;
using UnityEngine.UI;

namespace CavrnusSdk.CollaborationExamples
{
    public class MaterialPickerItem : MonoBehaviour
    {
        [SerializeField] private RawImage thumbnail;
        
        private Action<Material> onSelected;

        private Material mat;
        
        public void Setup(Material mat, Action<Material> onSelected)
        {
            this.mat = mat;
            thumbnail.texture = mat.mainTexture;
            this.onSelected = onSelected;
        }

        public void Select() => onSelected?.Invoke(mat);
    }
}