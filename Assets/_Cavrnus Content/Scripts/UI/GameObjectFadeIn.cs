using System.Collections.Generic;
using CavrnusCore;
using CavrnusSdk.UI;
using UnityEngine;

namespace CavrnusDemo
{
    public class GameObjectFadeIn : MonoBehaviour
    {
        [SerializeField] private float duration = 0.2f;
        
        private CanvasGroup cg;
        private void Awake()
        {
            cg = gameObject.AddComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            gameObject.DoFade(new List<CanvasGroup> {cg}, duration, true);
        }
    }
}