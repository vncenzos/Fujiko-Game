using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    CapsuleCollider playerCollider;
    [SerializeField] public LayerMask interactLayer;
    // Start is called before the first frame update
    void Start()
    {
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInInteractable() != null){
            HandleInteraction(isInInteractable());
            print(isInInteractable());
        }
    }
    
    void HandleInteraction(Interactable interactable) {
        switch(interactable.interactionType){
            case Interactable.InteractionType.Click:
            if( Input.GetKeyDown(KeyCode.E)){
                interactable.Interact();
            }
                break;
            case Interactable.InteractionType.Hold:
                        if( Input.GetKey(KeyCode.E)){
                interactable.Interact();
            }
                break;
            default:
                throw new System.Exception("No supported interactable.");
        }
    }

    private Interactable isInInteractable()
    {
        Interactable interactableP = null;
        Collider[] collisions = Physics.OverlapSphere(transform.position, 0.5f, interactLayer);
        foreach (var collision in collisions)
        {
            if (collision.gameObject.layer == 7)
            {
                //7 is interact layer
                interactableP = collision.GetComponent<Interactable>();
                print("Sta collidendo con :");
                print(interactableP);
            }
            else
            {
                interactableP = null;
            }
        }
        return interactableP;
    }
}

