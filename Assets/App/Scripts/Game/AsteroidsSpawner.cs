using DG.Tweening;
using UnityEngine;

public class AsteroidsSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public Transform sunTransform;
    public float spawnRadius = 12f;

    public float minLaunchSpeed = 2f;
    public float maxLaunchSpeed = 6f;
    public float trajectoryChaos = 0.3f;

    public float spawnRate = 2f;
    public float difficultyFactor = 0.98f;
    public float minSpawnRate = 0.5f;

    public GameObject bonusPrefab;
    public float bonusChance = 0.1f;
    public float automaticDespawnBonus = 15f;

    private float nextSpawnTime;

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnComet();

            spawnRate = Mathf.Max(spawnRate * difficultyFactor, minSpawnRate);
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnComet()
    {
        Vector2 spawnPos = Random.insideUnitSphere.normalized * spawnRadius;
        Vector2 directionToSun = (Vector2)sunTransform.position - spawnPos;
        Vector2 tangent = new Vector2(-directionToSun.y, directionToSun.x).normalized;

        if (Random.value > 0.5f) tangent *= -1f;

        Vector2 launchDir = Vector2.Lerp(tangent, directionToSun.normalized, Random.Range(0f, trajectoryChaos));

        float speed = Random.Range(minLaunchSpeed, maxLaunchSpeed);

        if (bonusPrefab != null && Random.value < bonusChance)
        {
            GameObject newBonus = Instantiate(bonusPrefab, spawnPos, Quaternion.identity);
            Destroy(newBonus, automaticDespawnBonus);
            Rigidbody2D rbBonus = newBonus.GetComponent<Rigidbody2D>();
            if (rbBonus != null)
            {
                rbBonus.linearVelocity = launchDir * speed;
                rbBonus.angularVelocity = Random.Range(-90f, 90f);
            }
            return;
        }

        GameObject newAst = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);

        AsteroidsBehaviour astScript = newAst.GetComponent<AsteroidsBehaviour>();
        if (astScript != null)
        {
            astScript.InitializeAsteroid(sunTransform, launchDir * speed);
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
