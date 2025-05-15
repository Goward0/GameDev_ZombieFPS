using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public GameObject bossZombiePrefab; 
    public int bossWaveInterval = 10;    

    public Transform[] spawnPoints;
    public int zombiesPerWave = 4;

    private int waveNumber = 1;
    private List<GameObject> activeZombies = new List<GameObject>();
    private bool spawning = false;

    void Start()
    {
        SpawnZombies();
    }

    void Update()
    {
        activeZombies.RemoveAll(z => z == null);

        if (!spawning && activeZombies.Count == 0)
        {
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator StartNextWave()
{
    spawning = true;
    yield return new WaitForSeconds(3f);

    waveNumber++;

    // Increase zombiesPerWave by 3 every 2 waves after round 5
    if (waveNumber > 5 && waveNumber % 2 == 0)
    {
        zombiesPerWave += 3;
    }

    GameManager.Instance.NextWave(waveNumber);
    SpawnZombies();

    spawning = false;
}


    void SpawnZombies()
    {
        activeZombies.Clear();
        StartCoroutine(SpawnZombiesStaggered());
    }

    IEnumerator SpawnZombiesStaggered()
    {
        int totalToSpawn = zombiesPerWave;
        int spawnerIndex = 0;

        for (int i = 0; i < totalToSpawn; i++)
        {
            Transform spawnPoint = spawnPoints[spawnerIndex];
            GameObject zombie;

            //  Spawn boss zombie at start of boss wave
            if (GameManager.Instance.CurrentWave % bossWaveInterval == 0 && i == 0)
            {
                zombie = Instantiate(bossZombiePrefab, spawnPoint.position, Quaternion.identity);

                ZombieAI bossAI = zombie.GetComponent<ZombieAI>();
                if (bossAI != null)
                {
                    bossAI.isBoss = true;
                    bossAI.health = 1000f;
                    bossAI.attackDamage = 40f;
                    zombie.transform.localScale *= 1.5f;
                }
            }
            else
            {
                zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
            }

            activeZombies.Add(zombie);
            spawnerIndex = (spawnerIndex + 1) % spawnPoints.Length;

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    public void OnZombieDeath(GameObject zombie)
    {
        activeZombies.Remove(zombie);
    }

    public void FreezeAllZombies(bool frozen)
    {
        foreach (var zombie in FindObjectsOfType<ZombieAI>())
        {
            zombie.SetFrozen(frozen);
        }
    }
}
