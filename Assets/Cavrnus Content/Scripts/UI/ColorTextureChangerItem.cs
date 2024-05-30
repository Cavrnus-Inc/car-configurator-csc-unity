using System;
using UnityEngine;
using UnityEngine.UI;

namespace CavrnusDemo
{
    public class ColorTextureChangerItem : MonoBehaviour
    {
        public Color Color{ get; private set; }
        public Texture Texture{ get; private set; }
        
        [SerializeField] private Image image;
        [SerializeField] private GameObject selectedBorder;

        public ColorTextureInfo Info{ get; private set; }
        private Action<ColorTextureInfo> onSelected;

        public void Setup(ColorTextureInfo info, Action<ColorTextureInfo> onSelected)
        {
            this.Info = info;
            this.onSelected = onSelected;

            Color = info.Color;
            Texture = info.Texture;
            image.color = info.Color;
            
            selectedBorder.SetActive(false);
        }
        
        public void SetSelectionState(bool state)
        {
            selectedBorder.SetActive(state);
        }

        public void Select()
        {
            onSelected?.Invoke(Info);
        }
    }
}