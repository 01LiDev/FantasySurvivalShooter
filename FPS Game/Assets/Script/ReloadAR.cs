using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReloadAR : MonoBehaviour
{
    public int ARMagazine = 30; // Magazine capacity of the AR
    public int ARTotalAmmo = 120; // Total available ammo for the AR

    public bool IsReloading = false; // Indicates if the AR is currently being reloaded
    public Animator reloadAnimator; // Animator component to play the reload animation
    public FireAR FireAr; // Reference to the FireAR script
    public Text ARammoText; // UI Text component to display AR ammo information

    // Coroutine to play the reload animation and manage the reloading state
    public IEnumerator PlayReloadAnimation()
    {
        reloadAnimator.SetTrigger("Reload"); // Start the reload animation
        FireAr.weaponAnimator.SetBool("Firing", false); // Stop the firing animation
        yield return new WaitForSeconds(2.5f); // Wait for 2.5 seconds (animation length)

        IsReloading = false; // Set reloading state to false after the animation is finished
    }

    private void Update()
    {
        // Check if the magazine is empty, R key is pressed, and AR is not already reloading and has available ammo
        if ((ARMagazine == 0 || (Input.GetKeyDown(KeyCode.R) && ARMagazine < 30)) && !IsReloading && ARTotalAmmo > 0)
        {
            IsReloading = true; // Set the reloading state to true

            // Calculate how much ammo is needed to fill the magazine and take it from the total ammo
            int ammoToReload = Mathf.Min(30 - ARMagazine, ARTotalAmmo);
            ARTotalAmmo -= ammoToReload;
            ARMagazine += ammoToReload;

            // Start the reload animation coroutine
            StartCoroutine(PlayReloadAnimation());
        }

        // Update the ammo UI text
        ARammoText.text = "AR Ammo: " + ARMagazine + "/" + ARTotalAmmo;
    }
    public void AddAmmo(int amount)
    {
        ARTotalAmmo += amount;
    }
}
