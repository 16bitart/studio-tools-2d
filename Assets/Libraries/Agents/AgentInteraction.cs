using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInteraction : MonoBehaviour
{
    public void Check(GameObject other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            Debug.Log("Found an interactable!");
        }
    }
}

public interface IInteractable
{
    public void Interact(GameObject other);
}
