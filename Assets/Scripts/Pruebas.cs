using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class pruebas : MonoBehaviour
{
    public Transform initialPoint;
    public Transform finalPoint;

    public TileBase pathTile;
    public Tilemap tilemap;

    private List<Vector3> path;

    private void Start()
    {
        object o = null;

        Entity e = o as Entity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            path = AStarPathfinding.Instance.GetPath(
                    initialPoint.position,
                    finalPoint.position,
                    TeamEnum.BLUE
                );

            if (path.Count < 2)
            {
                Debug.Log("No se encontro camino");
            }
            else
            {
                foreach (Vector3 pos in path)
                {
                    tilemap.SetTile(Vector3Int.FloorToInt(pos), pathTile);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            tilemap.ClearAllTiles();
        }
    }
}
