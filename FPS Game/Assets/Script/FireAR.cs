using System.Collections;
using UnityEngine;

public class FireAR : MonoBehaviour
{
    public float force = 100f;
    public GameObject prefabToSpawn;
    public float delayBetweenShots = 0.5f;
    public float zRotation = 90f;
    public LayerMask layerMask;
    public float offsetAngle = 5f;
    public Animator characterAnimator;
    public Animator weaponAnimator;
    private Vector3 direction;
    private bool isFiring = false;
    private bool isFiringCoroutineRunning = false;

    private ParticleSystemController particleSystemController;
    private Camera mainCamera;
    public GameObject particleSpawnerObject;

    public ReloadAR ReloadAr;

    public CasingRelease casingRelease;

    private void Start()
    {
        particleSystemController = particleSpawnerObject.GetComponent<ParticleSystemController>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && ReloadAr.ARMagazine > 0 && !ReloadAr.IsReloading)
        {
            isFiring = true;
        }
        else
        {
            isFiring = false;
            
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        isFiringCoroutineRunning = false;
        weaponAnimator.SetBool("Firing", false);
        ReloadAr.IsReloading = false;
    }
    private IEnumerator FireCoroutine()
    {

        isFiringCoroutineRunning = true;

        // Turn on weapon animation
        weaponAnimator.SetBool("Firing", true);


        Vector3 offsetDirection = Quaternion.Euler(0, offsetAngle, 0) * mainCamera.transform.forward;
        direction = offsetDirection;


        // Check if the player is currently reloading, and if so, exit early and wait for the reload animation to complete
        if (ReloadAr.IsReloading)
        {
            isFiringCoroutineRunning = false;
            yield break;
        }

        if (ReloadAr.IsReloading)
        {
            // Start the reload animation if the magazine is empty
            isFiring = false;
            ReloadAr.IsReloading = true;

        }
        else
        {
            GameObject spawnedPrefab = Instantiate(prefabToSpawn, transform.position, Quaternion.Euler(zRotation, 0f, 0f));
            if (!spawnedPrefab.GetComponent<Rigidbody>())
            {
                spawnedPrefab.AddComponent<Rigidbody>();
            }
            spawnedPrefab.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);

            // Spawn particles when a new projectile is spawned
            particleSystemController.SpawnParticles();

            // Deduct one bullet from the magazine count
            ReloadAr.ARMagazine--;
            // Instantiate casing when a new projectile is spawned
            casingRelease.ReleaseCasing();

            yield return new WaitForSeconds(delayBetweenShots);

            if (isFiring && ReloadAr.ARMagazine > 0)
            {
                StartCoroutine(FireCoroutine());
            }
            else
            {
                // Turn off weapon animation
                weaponAnimator.SetBool("Firing", false);
                isFiringCoroutineRunning = false;
            }
        }
    }


    private void FixedUpdate()
    {
        if (isFiring && !isFiringCoroutineRunning)
        {
            StartCoroutine(FireCoroutine());
        }
    }
}
