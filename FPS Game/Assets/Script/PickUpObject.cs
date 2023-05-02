using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public enum PickUpType { Ammo, Health, Pistol, Rifle }
    public string animationTrigger = "Activate";
    public float destroyDelay = 1.5f; // Set a delay before destroying the prefab
    public PickUpType pickUpType;
    public int amount;
    private Animator animator;
    private PickUpManager pickUpManager;
    private InventoryManager inventoryManager;
    private bool hasBeenPickedUp; // Add a boolean to track if the object has been picked up

    void Start()
    {
        if (pickUpType == PickUpType.Pistol || pickUpType == PickUpType.Rifle)
        {
            animator = null; // Set the animator to null if it's a Pistol or Rifle
        }
        else
        {
            animator = GetComponent<Animator>(); // Keep the animator for other types
        }
        GameObject gameManager = GameObject.FindWithTag("GameManager");
        if (gameManager != null)
        {
            pickUpManager = gameManager.GetComponent<PickUpManager>();
            inventoryManager = gameManager.GetComponent<InventoryManager>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenPickedUp) // Check if the object has not been picked up
        {
            pickUpManager.RegisterPickUp(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenPickedUp) // Check if the object has not been picked up
        {
            pickUpManager.UnregisterPickUp(this);
        }
    }

    public void Activate()
    {
        if (hasBeenPickedUp) // Check if the object has already been picked up
        {
            return; // If it has, do not continue with the activation
        }

        if (pickUpType == PickUpType.Ammo)
        {
            pickUpManager.AddAmmo(amount);
            animator.SetTrigger(animationTrigger);
            StartCoroutine(DestroyAfterDelay());

        }
        else if (pickUpType == PickUpType.Health)
        {
            pickUpManager.AddHealth(amount);
            animator.SetTrigger(animationTrigger);
            StartCoroutine(DestroyAfterDelay());
        }
        else if (pickUpType == PickUpType.Pistol)
        {
            inventoryManager.EnablePistolSlot();
            DestroyTaggedObjects("PistolPickUp");
            pickUpManager.DeactivatePickupText();
        }
        else if (pickUpType == PickUpType.Rifle)
        {
            inventoryManager.EnableRifleSlot();
            pickUpManager.DeactivatePickupText();
            DestroyTaggedObjects("AKPickUp");
        }

    
        hasBeenPickedUp = true; // Mark the object as picked up after activation
    }
    private void DestroyTaggedObjects(string tag)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in taggedObjects)
        {
            Destroy(obj);
        }
    }
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
