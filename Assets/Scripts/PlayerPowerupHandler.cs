using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerPowerupHandler : MonoBehaviour
{
    public PlayerHealth playerHealth;               // Drag PlayerHealth component
    public PlayerMovementScript playerMovement;     // Drag PlayerMovementScript
    public ZombieSpawner zombieSpawner;             // Drag ZombieSpawner from scene
    public PowerupUIManager powerupUI;              // Drag PowerupUIManager from scene

    private GunScript gunScript;                    // Automatically detected each frame

    void Update()
    {
        if (gunScript == null || !gunScript.gameObject.activeInHierarchy)
        {
            gunScript = FindCurrentGunScript();
        }
    }

    private GunScript FindCurrentGunScript()
    {
        GunScript[] guns = FindObjectsOfType<GunScript>();
        foreach (GunScript g in guns)
        {
            if (g.gameObject.activeInHierarchy)
                return g;
        }

        Debug.LogWarning("No active GunScript found!");
        return null;
    }

    public void ApplyPowerup(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.MaxAmmo:
                if (gunScript != null) gunScript.RefillAmmo();
                break;

            case PowerupType.DoubleDamage:
                if (gunScript != null)
                {
                    StartCoroutine(DoubleDamageRoutine());
                    powerupUI.ShowPowerup(PowerupType.DoubleDamage, 20f);
                }
                break;

            case PowerupType.FullHealth:
                RestoreFullHealth();
                break;

            case PowerupType.SpeedBoost:
                StartCoroutine(SpeedBoostRoutine());
                powerupUI.ShowPowerup(PowerupType.SpeedBoost, 20f);
                break;

            case PowerupType.InfiniteAmmo:
                if (gunScript != null)
                {
                    StartCoroutine(InfiniteAmmoRoutine());
                    powerupUI.ShowPowerup(PowerupType.InfiniteAmmo, 15f);
                }
                break;

            case PowerupType.TimeFreeze:
                StartCoroutine(TimeFreezeRoutine());
                powerupUI.ShowPowerup(PowerupType.TimeFreeze, 10f);
                break;
        }
    }

    IEnumerator DoubleDamageRoutine()
    {
        gunScript.SetDoubleDamage(true);
        yield return new WaitForSeconds(20f);
        gunScript.SetDoubleDamage(false);
    }

    IEnumerator SpeedBoostRoutine()
    {
        int originalSpeed = playerMovement.maxSpeed;
        playerMovement.maxSpeed = Mathf.RoundToInt(originalSpeed * 1.5f);
        yield return new WaitForSeconds(20f);
        playerMovement.maxSpeed = originalSpeed;
    }

    IEnumerator InfiniteAmmoRoutine()
    {
        gunScript.SetInfiniteAmmo(true);
        yield return new WaitForSeconds(15f);
        gunScript.SetInfiniteAmmo(false);
    }

    IEnumerator TimeFreezeRoutine()
    {
        zombieSpawner.FreezeAllZombies(true);
        yield return new WaitForSeconds(10f);
        zombieSpawner.FreezeAllZombies(false);
    }

    private void RestoreFullHealth()
    {
        playerHealth.TakeDamage(-playerHealth.maxHealth); // heal full by subtracting negative damage
    }
}
