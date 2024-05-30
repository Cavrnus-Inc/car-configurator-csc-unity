using System;
using System.Collections.Generic;
using UnityEngine;

namespace CavrnusDemo
{
    public abstract class CavColorChangerUI : MonoBehaviour
    {
        public abstract void Setup(List<ColorTextureInfo> info, Action<ColorTextureInfo> onSelect);

        public abstract void SetSelected(ColorTextureInfo selectedData);
    }

    [Serializable]
    public class ColorTextureInfo
    {
        public string DisplayName;
        public Color Color;
        public Texture Texture;
    }
}