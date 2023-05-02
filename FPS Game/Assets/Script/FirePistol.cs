using System.Collections;
using UnityEngine;

public class FirePistol : MonoBehaviour
{
    public float force = 100f;
    public GameObject prefabToSpawn;
    public float delayBetweenShots = 0.5f;
    public float zRotation = 90f;
    public LayerMask layerMask;

    private Vector3 direction;
    private bool isFiring = false;
    private ParticleSystemController particleSystemController;
    private Camera mainCamera;
    public GameObject particleSpawnerObject;
    public CasingRelease casingRelease;
    private bool canFire = true;

    private void Start()
    {
        particleSystemController = particleSpawnerObject.GetComponent<ParticleSystemController>();
        mainCamera = Camera.main;
    }
    private void OnEnable()
    {
        canFire = true;
    }
    private void Update()
    {
        isFiring = Input.GetMouseButton(0) && gameObject.activeInHierarchy;

        // Check if we can fire again
        if (!canFire)
        {
            return;
        }

        // Fire the pistol if the player is pressing the fire button
        if (isFiring)
        {
            StartCoroutine(FireCoroutine());
        }
    }

    private IEnumerator FireCoroutine()
    {
        canFire = false;
        // Create a ray from the camera going through the middle of your screen
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        // Check whether you are pointing to something so as to adjust the direction
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            direction = (hit.point - transform.position).normalized;
        }
        else
        {
            direction = ray.direction;
        }

        GameObject spawnedPrefab = Instantiate(prefabToSpawn, transform.position, Quaternion.Euler(zRotation, 0f, 0f));
        if (!spawnedPrefab.GetComponent<Rigidbody>())
        {
            spawnedPrefab.AddComponent<Rigidbody>();
        }
        spawnedPrefab.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);

        // Spawn particles when a new projectile is spawned
        particleSystemController.SpawnParticles();
        // Instantiate casing when a new projectile is spawned
        casingRelease.ReleaseCasing();

        yield return new WaitForSeconds(delayBetweenShots);

        canFire = true;
    }
}
