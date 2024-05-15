using UnityEngine;

namespace BrokenVector.LowPolyFencePack
{
    [RequireComponent(typeof(Animation))]
    public class DoorController : MonoBehaviour
    {
        public enum DoorState
        {
            Open,
            Closed
        }

        public DoorState CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                currentState = value;
                Animate();
                UpdateCollider();
                UnityEngine.Debug.Log("Door state changed to: " + currentState);
            }
        }

        public bool IsDoorOpen { get { return CurrentState == DoorState.Open; } }

        public bool IsDoorClosed { get { return CurrentState == DoorState.Closed; } }

        public DoorState InitialState = DoorState.Closed;
        public float AnimationSpeed = 1;

        [SerializeField]
        private AnimationClip openAnimation;
        [SerializeField]
        private AnimationClip closeAnimation;

        private Animation animator;
        private DoorState currentState;
        private Collider doorCollider;

        void Awake()
        {
            animator = GetComponent<Animation>();
            doorCollider = GetComponent<Collider>();

            if (animator == null)
            {
                UnityEngine.Debug.LogError("Every DoorController needs an Animator.");
                return;
            }

            animator.playAutomatically = false;

            openAnimation.legacy = true;
            closeAnimation.legacy = true;
            animator.AddClip(openAnimation, DoorState.Open.ToString());
            animator.AddClip(closeAnimation, DoorState.Closed.ToString());
        }

        void Start()
        {
            currentState = InitialState;
            var clip = GetCurrentAnimation();
            animator[clip].speed = 9999;
            animator.Play(clip);
            UpdateCollider();
        }

        // Close the door
        public void CloseDoor()
        {
            if (IsDoorClosed)
                return;

            CurrentState = DoorState.Closed;
            UnityEngine.Debug.Log("CloseDoor called");
        }

        // Open the door
        public void OpenDoor()
        {
            if (IsDoorOpen)
                return;

            CurrentState = DoorState.Open;
            UnityEngine.Debug.Log("OpenDoor called");
        }

        // Toggle the door state
        public void ToggleDoor()
        {
            UnityEngine.Debug.Log("ToggleDoor called. Current state: " + currentState);
            if (IsDoorOpen)
                CloseDoor();
            else
                OpenDoor();
        }

        private void Animate()
        {
            var clip = GetCurrentAnimation();
            animator[clip].speed = AnimationSpeed;
            animator.Play(clip);
            UnityEngine.Debug.Log("Animate called with clip: " + clip);
        }

        private string GetCurrentAnimation()
        {
            return CurrentState.ToString();
        }

        private void UpdateCollider()
        {
            if (doorCollider != null)
            {
                doorCollider.enabled = IsDoorClosed;
                UnityEngine.Debug.Log("UpdateCollider called. Collider enabled: " + doorCollider.enabled);
            }
        }
    }
}
