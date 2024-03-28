using System;
using UnityEngine;
using UnityEngine.UI;

namespace CavrnusDemo
{
    public class ColorTextureChangerItem : MonoBehaviour
    {
        [SerializeField] private Image image;

        private ColorTextureChanger.ColorTextureMapper mapper;
        private Action<ColorTextureChanger.ColorTextureMapper> onSelected;

        public void Setup(ColorTextureChanger.ColorTextureMapper mapper, Action<ColorTextureChanger.ColorTextureMapper> onSelected)
        {
            this.mapper = mapper;
            this.onSelected = onSelected;

            image.color = mapper.Color;
        }

        public void Select()
        {
            onSelected?.Invoke(mapper);
        }
    }
}