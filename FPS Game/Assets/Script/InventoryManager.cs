using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Canvas inventoryCanvas; 
    private bool isInventoryOpen = false; 
    public GameObject SwordSlot; 
    public GameObject PistolSlot;
    public GameObject RifleSlot; 
    public bool canPress2 = false; 
    public bool canPress3 = false;
    void Start()
    {
        // Disable the inventory canvas at the start of the game
        inventoryCanvas.enabled = false;
        PistolSlot.SetActive(false);
        RifleSlot.SetActive(false);
        canPress2 = false;
        canPress3 = false;
    }
    public void EnableRifleSlot()
    {
        RifleSlot.SetActive(true);
        canPress3 = true;
    }

    public void EnablePistolSlot()
    {
        PistolSlot.SetActive(true);
        canPress2 = true;
    }


    void Update()
    {
        // Check if the 'i' key is pressed
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Toggle the inventory canvas and update the inventory state
            isInventoryOpen = !isInventoryOpen;
            inventoryCanvas.enabled = isInventoryOpen;

            // Pause the game if the inventory is opened, and resume it when it's closed
            Time.timeScale = isInventoryOpen ? 0 : 1;
            if (isInventoryOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        
    }
}
