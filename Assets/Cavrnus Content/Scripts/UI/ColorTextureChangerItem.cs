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

        private ColorTextureChanger.ColorTextureMapper mapper;
        private Action<ColorTextureChanger.ColorTextureMapper> onSelected;

        public void Setup(ColorTextureChanger.ColorTextureMapper mapper, Action<ColorTextureChanger.ColorTextureMapper> onSelected)
        {
            this.mapper = mapper;
            this.onSelected = onSelected;

            Color = mapper.Color;
            Texture = mapper.Texture;
            image.color = mapper.Color;
        }
        
        public void SetSelectionState(bool state)
        {
            selectedBorder.SetActive(state);
        }

        public void Select()
        {
            onSelected?.Invoke(mapper);
        }
    }
}