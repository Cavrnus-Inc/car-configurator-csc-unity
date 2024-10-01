using System;
using CavrnusSdk.API;
using UnityBase;
using UnityEngine;
using UnityEngine.UI;

namespace CavrnusSdk.StreamBoards
{
    public class CavrnusStreamBoard : MonoBehaviour
    {
        [SerializeField] private RawImage image;
        [SerializeField] private AspectRatioFitter aspectRatioFitter;

        private IDisposable profileDisposable;

        public void UpdateAndBindUserTexture(CavrnusUser user)
        {
            if (user == null)
            {
                ResetStream();
                return;
            }

            profileDisposable = user?.BindUserVideoFrames(SetImageAndAspectRatio);
        }

        public void UpdateTexture(TextureWithUVs tex)
        {
            SetImageAndAspectRatio(tex);
        }

        private void SetImageAndAspectRatio(TextureWithUVs tex)
        {
            if (tex != null) {
                image.texture = tex.Texture;
                image.uvRect = tex.UVRect;

                if (tex.Texture.width > 0 && tex.Texture.height > 0)
                    aspectRatioFitter.aspectRatio = (float)tex.Texture.width / (float)tex.Texture.height;
                else
                    aspectRatioFitter.aspectRatio = 1.5f;
            }
            else
                ResetStream();
        }

        private void ResetStream()
        {
            image.texture = null;
            profileDisposable?.Dispose();
        }
                
        private void OnDestroy() => profileDisposable?.Dispose();
    }
}