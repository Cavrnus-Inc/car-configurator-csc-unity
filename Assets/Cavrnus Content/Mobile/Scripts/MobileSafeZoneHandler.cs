using UnityEngine;

public class MobileSafeZoneHandler : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect safeArea;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }

    void Update()
    {
        // In case of device orientation change
        if (safeArea != Screen.safeArea)
            ApplySafeArea();
    }
}
