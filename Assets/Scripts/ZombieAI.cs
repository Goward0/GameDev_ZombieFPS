
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public float health = 50f;
    public float attackDamage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;

    public GameObject[] powerupPrefabs;
    public GameObject ragdollPrefab;

    public AudioClip zombieGrowlSound;
    public AudioClip zombieDeathSound;
    public float growlInterval = 5f;

    public bool isBoss = false;

    public GameObject waveEffectPrefab;
    public GameObject explosionEffectPrefab;

    public AudioClip waveSound;
    public AudioClip explosionSound;

    public float radioactiveWaveCooldown = 15f;
    private float lastWaveTime;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;
    private float lastAttackTime;
    private ZombieSpawner spawner;
    private bool isDead = false;
    private float nextGrowlTime = 0f;
    private bool isFrozen = false;

    void Start()
    {
        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null) player = pObj.transform;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spawner = FindObjectOfType<ZombieSpawner>();

        if (GameManager.Instance != null && !isBoss)
        {
            int wave = GameManager.Instance.CurrentWave;

            if (wave >= 5 && wave <= 30)
                health += (wave - 4) * 5f;

            if (wave >= 10)
                attackDamage += 10f;
        }
    }

    void Update()
    {
        if (isDead || player == null || isFrozen)
        {
            if (agent != null)
                agent.isStopped = true;
            return;
        }

        if (agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
            agent.isStopped = false;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            animator?.SetTrigger("Attack");
        }

        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator?.SetBool("isWalking", isMoving);

        if (GameManager.Instance != null && GameManager.Instance.CurrentWave >= 1)
        {
            animator?.SetBool("isRunning", true);
            agent.speed = 6f;
        }
        else
        {
            animator?.SetBool("isRunning", false);
            agent.speed = 3.5f;
        }

        if (Time.time >= nextGrowlTime && zombieGrowlSound != null)
        {
            audioSource?.PlayOneShot(zombieGrowlSound);
            nextGrowlTime = Time.time + growlInterval + Random.Range(0f, 2f);
        }

        if (isBoss && Time.time >= lastWaveTime + radioactiveWaveCooldown && distance <= 15f)
        {
            PerformRadioactiveWave();
            lastWaveTime = Time.time;
        }
    }

    void PerformRadioactiveWave()
    {
        if (waveEffectPrefab != null)
            Instantiate(waveEffectPrefab, transform.position + transform.forward * 2f, Quaternion.identity);

        if (waveSound != null)
            AudioSource.PlayClipAtPoint(waveSound, transform.position);

        if (Vector3.Distance(transform.position, player.position) < 15f)
        {
            var move = player.GetComponent<PlayerMovementScript>();
            if (move != null)
            {
                move.enabled = false;
                Invoke(nameof(RemovePlayerStun), 5f);
            }
        }
    }

    void RemovePlayerStun()
    {
        var move = player.GetComponent<PlayerMovementScript>();
        if (move != null)
            move.enabled = true;
    }

    public void DealDamage()
    {
        if (player == null) return;

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            var playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth?.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;

        if (health <= 0)
        {
            isDead = true;
            GameManager.Instance?.AddKill();
            spawner?.OnZombieDeath(gameObject);

            if (zombieDeathSound != null)
                AudioSource.PlayClipAtPoint(zombieDeathSound, transform.position);

            if (isBoss && explosionEffectPrefab != null)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                if (explosionSound != null)
                    AudioSource.PlayClipAtPoint(explosionSound, transform.position);

                if (Vector3.Distance(transform.position, player.position) < 7f)
                {
                    var ph = player.GetComponent<PlayerHealth>();
                    ph?.TakeDamage(40f);
                }
            }

            if (ragdollPrefab != null)
            {
                var ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
                ragdoll.tag = "DeadRagdoll";

                foreach (var rb in ragdoll.GetComponentsInChildren<Rigidbody>())
                    rb.AddForce(-transform.forward * 3f + Vector3.up * 2f, ForceMode.Impulse);
            }

            if (isBoss && powerupPrefabs.Length > 0)
            {
                GameObject maxAmmo = null;
                foreach (var powerup in powerupPrefabs)
                {
                    if (powerup.name.Contains("MaxAmmo"))
                    {
                        maxAmmo = powerup;
                        break;
                    }
                }
                if (maxAmmo != null)
                    Instantiate(maxAmmo, transform.position + Vector3.up, Quaternion.identity);
            }
            else if (Random.value <= 0.05f && powerupPrefabs.Length > 0)
            {
                int roll = Random.Range(0, powerupPrefabs.Length);
                Instantiate(powerupPrefabs[roll], transform.position + Vector3.up, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;
    }
}
