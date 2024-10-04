using System;
using System.Collections.Generic;
using System.Linq;
using UnityBase;
using UnityEngine;

namespace CavrnusDemo
{
    [CreateAssetMenu(fileName = "NewColorCollection", menuName = "Cavrnus/Color Collection", order = 1)]
    public class CavrnusColorCollection : ScriptableObject
    {
        [Serializable]
        public class ColorTextureInfo
        {
            public string DisplayName;
            public Color Color;
            public Texture Texture;
        }

        public List<ColorTextureInfo> ColorData;

        public ColorTextureInfo GetDataFromTextureName(string texture)
        {
            return ColorData.FirstOrDefault(data => data.Texture.name.ToLowerInvariant().Equals(texture.ToLowerInvariant()));
        }
        
        public ColorTextureInfo GetDataFromColor(Color color)
        {
            return ColorData.FirstOrDefault(data => ColorsEqual(data.Color, color));
        }
                
        private static bool ColorsEqual(Color c1, Color c2, float tolerance = 0.1f)
        {
            return c1.r.AlmostEquals(c2.r, tolerance) &&
                   c1.g.AlmostEquals(c2.g, tolerance) &&
                   c1.b.AlmostEquals(c2.b, tolerance);
        }
    }
}