using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasingRelease : MonoBehaviour
{
    public GameObject casingPrefab;
    public Transform casingSpawnPoint;
    public float casingForce = 10f;

    public void ReleaseCasing()
    {
        GameObject casingInstance = Instantiate(casingPrefab, casingSpawnPoint.position, casingSpawnPoint.rotation);
        Rigidbody casingRb = casingInstance.GetComponent<Rigidbody>();

        if (casingRb == null)
        {
            casingRb = casingInstance.AddComponent<Rigidbody>();
        }

        casingRb.AddForce(casingSpawnPoint.right * casingForce, ForceMode.Impulse);
    }
}
