using System.Collections;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    public GameObject particleSystemPrefab;
    private ParticleSystem particleSystemInstance;
    public float destructionDelay = 0.2f; // Delay before destroying the particle system

    public void SpawnParticles()
    {
        if (particleSystemPrefab != null)
        {
            // Destroy the existing particle system if it exists
            if (particleSystemInstance != null)
            {
                Destroy(particleSystemInstance.gameObject);
            }

            GameObject newParticleSystem = Instantiate(particleSystemPrefab, transform.position, transform.rotation);
            particleSystemInstance = newParticleSystem.GetComponent<ParticleSystem>();
            particleSystemInstance.Play();
        }
    }

}
