using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using TMPro;

public class Pruebas : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private Transform _posA;
    [SerializeField]
    private Transform _posB;

    public void ShowWinner(TeamEnum winner)
    {
        _text.text += winner.ToString();
    }

    [ContextMenu("Pathfinding")]
    public void CalculatePath()
    {
        List<Node> path = AStarPathfinding.Instance.GetPath(_posA.position, _posB.position, TeamEnum.BLUE);

        string s = ">> ";
        foreach (Node node in path)
        {
            s += $"({node.Position.x}, {node.Position.y}) ";
        }
        Debug.Log($"Nuevo camino generado: {s}");

        Debug.Log($"Max Int Value: {int.MaxValue}");
    }
}
