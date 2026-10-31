using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public player player;
    public Transform cam;
    public GameObject[] obstaclePrefabs;
    public GameObject crystalPrefab;
    // Vertical distance between rows of obstacles
    public float rowGap = 5f;
    // Chance that a row also has a crystal in one of its free lanes
    [Range(0f, 1f)] public float crystalChance = 0.4f;
    // Rows are created this far below the camera and removed this far above it
    public float spawnDistance = 8f;
    public float despawnDistance = 8f;

    private float nextRowY;
    private readonly List<GameObject> spawned = new List<GameObject>();

    void Start()
    {
        if (cam == null) cam = Camera.main.transform;
        nextRowY = cam.position.y - spawnDistance;
    }

    void Update()
    {
        while (nextRowY > cam.position.y - spawnDistance)
        {
            SpawnRow(nextRowY);
            nextRowY -= rowGap;
        }

        for (int i = spawned.Count - 1; i >= 0; i--)
        {
            // Picked-up crystals are already destroyed (null)
            if (spawned[i] == null)
            {
                spawned.RemoveAt(i);
            }
            else if (spawned[i].transform.position.y - cam.position.y > despawnDistance)
            {
                Destroy(spawned[i]);
                spawned.RemoveAt(i);
            }
        }
    }

    void SpawnRow(float y)
    {
        // The lanes are the spots the player can stop on
        float[] lanes = { -player.speed, 0f, player.speed };
        for (int i = lanes.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lanes[i], lanes[j]) = (lanes[j], lanes[i]);
        }

        // Block 1 or 2 lanes, never all 3, so there's always a way through
        int blocked = Random.Range(1, lanes.Length);
        for (int i = 0; i < blocked; i++)
        {
            Spawn(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], lanes[i], y);
        }

        if (Random.value < crystalChance)
        {
            Spawn(crystalPrefab, lanes[blocked], y);
        }
    }

    void Spawn(GameObject prefab, float x, float y)
    {
        spawned.Add(Instantiate(prefab, new Vector3(x, y, 0f), Quaternion.identity, transform));
    }
}
