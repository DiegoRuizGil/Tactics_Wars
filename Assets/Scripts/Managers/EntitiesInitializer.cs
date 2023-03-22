using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class EntitiesInitializer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private TeamEnum _team;

    [Header("Entities Prefabs")]
    [SerializeField]
    private GameObject[] _prefabs;

    [Header("Prefabs Postions")]
    [SerializeField]
    private Vector3[] _positions;

    public void AddEntitiesToGrid()
    {
        for (int i = 0; i < _prefabs.Length; i++)
        {
            if (i >= _positions.Length)
                continue;
            Vector3 position = _positions[i];

            Entity entity = GameObject.Instantiate(_prefabs[i], position, Quaternion.identity).GetComponent<Entity>();
            if (entity == null)
                continue;

            entity.Team = _team;
            entity.transform.parent = this.transform;

            Node node = Grid.Instance.GetNode(entity.transform.position);
            if (node != null)
            {
                node.AddEntity(entity);

                // se cambiara cuando se implementen los distintos tipos de edificios
                if (entity is UnitBuilding)
                {
                    foreach (Node neighbour in node.Neighbours)
                        neighbour.CanBuildUB = true;
                }
            }
        }
    }
}
