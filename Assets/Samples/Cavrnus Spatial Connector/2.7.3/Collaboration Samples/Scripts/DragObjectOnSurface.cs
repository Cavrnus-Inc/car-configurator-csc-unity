using UnityEngine;

namespace CavrnusSdk.CollaborationExamples
{
    public class DragObjectOnSurface : MonoBehaviour
    {
        private Camera cam;
        
        private Vector3 offset;
        private float zCoord;
        
        private void Awake()
        {
            cam = Camera.main;
        }

        private void OnMouseDown()
        {
            zCoord = cam.WorldToScreenPoint(transform.position).z;
            offset = transform.position - GetMouseWorldPosition();
        }

        private Vector3 GetMouseWorldPosition()
        {
            var mousePos = Input.mousePosition;
            mousePos.z = zCoord;
            mousePos.y = transform.position.y;

            return cam.ScreenToWorldPoint(mousePos);
        }

        private void OnMouseDrag()
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }
}