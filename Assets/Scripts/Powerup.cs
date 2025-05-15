using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupType powerupType;
    public AudioClip pickupSound; // Assign in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPowerupHandler handler = other.GetComponent<PlayerPowerupHandler>();
            if (handler != null)
            {
                handler.ApplyPowerup(powerupType);
            }

            // Play pickup sound
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}
