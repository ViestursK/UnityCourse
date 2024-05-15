using UnityEngine;
using UnityEngine.UI;

public class GateController : MonoBehaviour
{
    public Animation gateAnimation;
    public string openAnimationName = "Open";
    public string closeAnimationName = "Close";
    public UnityEngine.UI.Text interactionPrompt;

    private bool isOpen = false;

    void Start()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.enabled = false;
        }
        else
        {
            UnityEngine.Debug.LogError("Interaction prompt is not assigned in the GateController script.");
        }
    }

    // Show or hide the interaction prompt
    public void ShowPrompt(bool show)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.enabled = show;
        }
    }

    // Handle interaction with the gate
    public void Interact()
    {
        if (gateAnimation != null)
        {
            if (isOpen)
            {
                gateAnimation.Play(closeAnimationName);
            }
            else
            {
                gateAnimation.Play(openAnimationName);
            }
            isOpen = !isOpen;
        }
        else
        {
            UnityEngine.Debug.LogError("Gate animation is not assigned in the GateController script.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowPrompt(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowPrompt(false);
        }
    }
}
