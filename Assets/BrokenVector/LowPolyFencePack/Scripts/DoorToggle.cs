using UnityEngine;

namespace BrokenVector.LowPolyFencePack
{
    [RequireComponent(typeof(DoorController))]
    public class DoorToggle : MonoBehaviour
    {
        private DoorController doorController;

        void Awake()
        {
            doorController = GetComponent<DoorController>();
            if (doorController == null)
            {
                UnityEngine.Debug.LogError("DoorController component not found on " + gameObject.name);
            }
        }

        void OnMouseDown()
        {
            UnityEngine.Debug.Log("OnMouseDown called on " + gameObject.name);
            doorController.ToggleDoor();
        }
    }
}
