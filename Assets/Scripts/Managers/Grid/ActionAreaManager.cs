using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ActionAreaManager : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField]
    private Tilemap _actionAreaTilemap;
    [SerializeField]
    private Tilemap _movementAreaTilemap;

    [Header("Area Tiles")]
    [SerializeField]
    private TileBase _walkZoneTile;

    [Header("Final Arrow Tiles")]
    [SerializeField]
    private TileBase _finalDownArrowTile;
    [SerializeField]
    private TileBase _finalLeftArrowTile;
    [SerializeField]
    private TileBase _finalRightArrowTile;
    [SerializeField]
    private TileBase _finalUpArrowTile;

    [Header("Straight Arrow Tiles")]
    [SerializeField]
    private TileBase _middleHorizontalArrowTile;
    [SerializeField]
    private TileBase _middleVerticalArrowTile;

    [Header("Corner Arrow Tiles")]
    [SerializeField] // Left Up - Down Right
    private TileBase _cornerLUDRArrowTile;
    [SerializeField] // Right Up - Down Left
    private TileBase _cornerRUDLArrowTile;
    [SerializeField] // Up Left - Right Down
    private TileBase _cornerULRDArrowTile;
    [SerializeField] // Up Right - Left Down
    private TileBase _cornerURLDArrowTile;

    private Dictionary<string, TileBase> _middleArrowMap;
    private Dictionary<string, TileBase> _finalArrowMap;

    void Start()
    {
        _middleArrowMap = new Dictionary<string, TileBase> {
            {"VERTICAL", _middleVerticalArrowTile},
            {"HORIZONTAL", _middleHorizontalArrowTile},
            // corners
            {"LUDR", _cornerLUDRArrowTile},
            {"RUDL", _cornerRUDLArrowTile},
            {"ULRD", _cornerULRDArrowTile},
            {"URLD", _cornerURLDArrowTile}
        };

        _finalArrowMap = new Dictionary<string, TileBase> {
            {"UP", _finalUpArrowTile},
            {"DOWN", _finalDownArrowTile},
            {"RIGHT", _finalRightArrowTile},
            {"LEFT", _finalLeftArrowTile}
        };
    }

    public void CleanTilemap()
    {
        _movementAreaTilemap.ClearAllTiles();
        _actionAreaTilemap.ClearAllTiles();
    }

    public void DrawActionArea(List<Vector3> positions)
    {
        foreach (Vector3 position in positions)
        {
            Vector3Int tilePosition = Vector3Int.FloorToInt(position);
            _actionAreaTilemap.SetTile(tilePosition, _walkZoneTile);
        }
    }

    public void DrawArrow(List<Vector3> path)
    {
        _movementAreaTilemap.ClearAllTiles();

        if (path.Count <= 2)
        {
            Vector3 previousPos = path[0];
            Vector3 nextPos = path[1];
            _movementAreaTilemap.SetTile(Vector3Int.FloorToInt(nextPos),
                _finalArrowMap[GetTopDirection(previousPos, nextPos)]);
        }
        else
        {
            Vector3 previousDirection;
            Vector3 nextDirection;
            for (int i = 1; i < path.Count - 1; i++)
            {
                previousDirection = GetDirectionalVector(path[i - 1], path[i]);
                nextDirection = GetDirectionalVector(path[i], path[i + 1]);

                _movementAreaTilemap.SetTile(Vector3Int.FloorToInt(path[i]),
                    _middleArrowMap[GetDirection(previousDirection, nextDirection)]);
            }

            Vector3 previousPos = path[path.Count - 2];
            Vector3 nextPos = path[^1];

            _movementAreaTilemap.SetTile(Vector3Int.FloorToInt(nextPos),
                _finalArrowMap[GetTopDirection(previousPos, nextPos)]);
        }
    }

    private string GetTopDirection(Vector3 start, Vector3 final)
    {
        if (final.y > start.y)
            return "UP";
        if (final.y < start.y)
            return "DOWN";
        if (final.x > start.x)
            return "RIGHT";
        if (final.x < start.x)
            return "LEFT";

        return "UP";
    }

    private string GetDirection(Vector3 previousDirecton, Vector3 nextDirection)
    {
        if (previousDirecton == Vector3.up && nextDirection == Vector3.up
            || previousDirecton == Vector3.down && nextDirection == Vector3.down)
            return "VERTICAL";

        if (previousDirecton == Vector3.left && nextDirection == Vector3.left
            || previousDirecton == Vector3.right && nextDirection == Vector3.right)
            return "HORIZONTAL";

        // Left Up - Down Right
        if (previousDirecton == Vector3.left && nextDirection == Vector3.up
            || previousDirecton == Vector3.down && nextDirection == Vector3.right)
            return "LUDR";

        // Right Up - Down Left
        if (previousDirecton == Vector3.right && nextDirection == Vector3.up
            || previousDirecton == Vector3.down && nextDirection == Vector3.left)
            return "RUDL";

        // Up Left - Right Down
        if (previousDirecton == Vector3.up && nextDirection == Vector3.left
            || previousDirecton == Vector3.right && nextDirection == Vector3.down)
            return "ULRD";

        // Up Right - Left Down
        if (previousDirecton == Vector3.up && nextDirection == Vector3.right
            || previousDirecton == Vector3.left && nextDirection == Vector3.down)
            return "URLD";

        return "VERTCIAL"; // default value
    }

    private Vector3 GetDirectionalVector(Vector3 initialPos, Vector3 nextPos)
    {
        return new Vector3(nextPos.x - initialPos.x,
            nextPos.y - initialPos.y,
            nextPos.z - initialPos.z).normalized;
    }
}
