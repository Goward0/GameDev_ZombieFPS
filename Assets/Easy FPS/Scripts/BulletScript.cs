using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float maxDistance = 1000000;
    public GameObject decalHitWall;
    public float floatInfrontOfWall = 0.05f;
    public GameObject bloodEffect;
    public LayerMask ignoreLayer;
    private float bulletDamage = 25f;

    public void SetDamage(float dmg)
    {
        bulletDamage = dmg;
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, ~ignoreLayer))
        {
            // Wall hit
            if (hit.transform.CompareTag("LevelPart") && decalHitWall)
                Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));

            // Blood effect
            if (hit.transform.CompareTag("Dummie") && bloodEffect)
                Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));

            // Headshot detection
            float damageToApply = bulletDamage;
            if (hit.collider.CompareTag("Head"))
            {
                damageToApply *= 2f; // Headshot multiplier
                Debug.Log("Headshot!");
            }

            // Apply damage
            ZombieAI zombie = hit.transform.GetComponent<ZombieAI>();
            if (zombie != null)
                zombie.TakeDamage(damageToApply);

            Destroy(gameObject);
        }

        Destroy(gameObject, 0.1f);
    }
}
