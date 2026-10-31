using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public player player;
    public Transform cam;
    public GameObject[] obstaclePrefabs;
    public GameObject crystalPrefab;
    // Vertical distance between rows of obstacles
    public float rowGap = 9f;
    // Chance that a row also has a crystal in one of its free lanes
    [Range(0f, 1f)] public float crystalChance = 0.4f;
    // Rows are created this far below the camera and removed this far above it
    public float spawnDistance = 10f;
    public float despawnDistance = 8f;

    // Chance that a blocked corner lane gets a FlyTrap (raise to make it more common)
    [Range(0f, 1f)] public float flyTrapChance = 0.6f;
    // FlyTrap fixed X/Z positions for the left and right corners
    public float flyTrapLeftX = -6.73f;
    public float flyTrapRightX = 4.93013191f;
    public float flyTrapZ = -1f;
    // Added to the row's Y so the FlyTrap keeps scrolling with the rows
    public float flyTrapYOffset = 0f;

    private float nextRowY;
    private readonly List<GameObject> spawned = new List<GameObject>();
    private readonly List<GameObject> normalPrefabs = new List<GameObject>();
    private GameObject flyTrapPrefab;

    void Start()
    {
        if (cam == null) cam = Camera.main.transform;
        nextRowY = cam.position.y - spawnDistance;

        foreach (GameObject p in obstaclePrefabs)
        {
            if (p.name == "FlyTrap") flyTrapPrefab = p;
            else normalPrefabs.Add(p);
        }
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
            bool isCorner = !Mathf.Approximately(lanes[i], 0f);

            // FlyTrap replaces the obstacle in a corner lane, at its fixed corner position
            if (isCorner && flyTrapPrefab != null && Random.value < flyTrapChance)
            {
                bool isLeft = lanes[i] < 0f;
                float x = isLeft ? flyTrapLeftX : flyTrapRightX;
                GameObject trap = SpawnAt(flyTrapPrefab, new Vector3(x, y + flyTrapYOffset, flyTrapZ));

                // Flip the X axis when it's on the left
                if (isLeft)
                {
                    Vector3 s = trap.transform.localScale;
                    s.x = -s.x;
                    trap.transform.localScale = s;
                }
                continue;
            }

            // Everything else never uses FlyTrap
            GameObject prefab = normalPrefabs.Count > 0
                ? normalPrefabs[Random.Range(0, normalPrefabs.Count)]
                : obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Spawn(prefab, lanes[i], y);
        }

        if (Random.value < crystalChance)
        {
            Spawn(crystalPrefab, lanes[blocked], y);
        }
    }

    void Spawn(GameObject prefab, float x, float y)
    {
        SpawnAt(prefab, new Vector3(x, y, 0f));
    }

    GameObject SpawnAt(GameObject prefab, Vector3 position)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity, transform);
        spawned.Add(obj);
        return obj;
    }
}