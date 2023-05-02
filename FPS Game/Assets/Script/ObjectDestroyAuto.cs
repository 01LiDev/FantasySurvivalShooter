using UnityEngine;

public class ObjectDestroyAuto : MonoBehaviour
{
    public float destroyTime = 5f; // time in seconds after which the object should be destroyed
    public bool destroyOnCollision = false; // whether the object should be destroyed on collision

    private void Start()
    {
        // Destroy the object after destroyTime seconds have elapsed
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the object collides with another object and destroyOnCollision is true,
        // destroy the object immediately
        if (destroyOnCollision)
        {
            Destroy(gameObject);
        }
    }
}
