using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public Transform cam;
    private SpriteRenderer[] tiles;
    private float tileHeight;

    void Start()
    {
        if (cam == null) cam = Camera.main.transform;
        tiles = GetComponentsInChildren<SpriteRenderer>();
        tileHeight = tiles[0].bounds.size.y;
    }

    // Runs after the camera has moved, so tiles are placed for this frame's view
    private void LateUpdate()
    {
        foreach (SpriteRenderer tile in tiles)
        {
            // Tile has scrolled fully above the camera -> send it below the lowest tile
            if (tile.transform.position.y - cam.position.y > tileHeight)
            {
                tile.transform.position += Vector3.down * tileHeight * tiles.Length;
            }
        }
    }
}
