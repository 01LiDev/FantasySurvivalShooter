using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpManager : MonoBehaviour
{
    public GameObject ammoBoxPrefab;
    public GameObject healthBoxPrefab;
    public KeyCode activationKey = KeyCode.E;
    public int maxAmmoBoxes = 5;
    public int maxHealthBoxes = 5;
    public Terrain terrain; // Reference to your terrain object
    public float yOffset = 0.5f; // Offset to place the object above the terrain

    private GameObject player;
    private PickUpObject currentPickUp;
    public ReloadAR reloadAR;
    private PlayerHealthManager playerHealthManager;
    public Text pickupText; // Reference to the Text UI component

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject gameManager = GameObject.FindWithTag("GameManager");
        if (gameManager != null)
        {
            playerHealthManager = gameManager.GetComponent<PlayerHealthManager>(); // Assign the PlayerHealthManager from the GameManager
        }
        // Spawn ammo and health boxes
        SpawnPickUps(ammoBoxPrefab, maxAmmoBoxes);
        SpawnPickUps(healthBoxPrefab, maxHealthBoxes);
        // Set the Text UI to inactive by default
        pickupText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (currentPickUp != null && Input.GetKeyDown(activationKey))
        {
            currentPickUp.Activate();
        }
    }

    public void RegisterPickUp(PickUpObject pickUp)
    {
        currentPickUp = pickUp;
        // Enable the Text UI when a pickup is registered
        pickupText.gameObject.SetActive(true);
    }

    public void UnregisterPickUp(PickUpObject pickUp)
    {
        if (currentPickUp == pickUp)
        {
            currentPickUp = null;
            // Disable the Text UI when a pickup is unregistered
            pickupText.gameObject.SetActive(false);
        }
    }
    public void DeactivatePickupText()
    {
        pickupText.gameObject.SetActive(false);
    }
    private void SpawnPickUps(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPosition = GetRandomPositionOnTerrain();
            GameObject spawnedPickUp = Instantiate(prefab, randomPosition, Quaternion.identity);
            spawnedPickUp.transform.SetParent(transform);
        }
    }
    public void AddAmmo(int amount)
    {
        reloadAR.AddAmmo(amount);
        pickupText.gameObject.SetActive(false);
    }

    public void AddHealth(int amount)
    {
        playerHealthManager.HealPlayer(amount);
        pickupText.gameObject.SetActive(false);
    }
    private Vector3 GetRandomPositionOnTerrain()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;

        float randomX = Random.Range(0, terrainWidth);
        float randomZ = Random.Range(0, terrainLength);

        float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + yOffset;

        return new Vector3(randomX, y, randomZ);
    }
    public void RespawnPickUps()
    {
        SpawnPickUps(ammoBoxPrefab, maxAmmoBoxes);
        SpawnPickUps(healthBoxPrefab, maxHealthBoxes);
    }
}
