using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    public void ManageEntities(TeamEnum team)
    {
        if (team != GameManager.Instance.PlayerTeam)
        {
            StartCoroutine(nameof(ManageUnits), team);
        }
    }

    private IEnumerator ManageUnits(TeamEnum team)
    {
        List<Unit> units = GameManager.Instance.UnitLists[team];

        if (units.Count > 0)
        {
            int index = 0;
            BehaviourTree.Tree unitTree = null;
            while (index < units.Count)
            {
                if (unitTree == null)
                {
                    if (units[index].TryGetComponent(out BehaviourTree.Tree tree))
                    {
                        unitTree = tree;
                        unitTree.enabled = true;
                    }
                    else
                    {
                        index++;
                    }
                }
                else
                {
                    if (units[index].HasFinished)
                    {
                        unitTree.enabled = false;
                        unitTree = null;
                        index++;
                    }
                }

                yield return null;
            }
        }

        yield return null;

        ManageBuildings(team);

        GameManager.Instance.FinalizeCurrentTurn();
    }

    private void ManageBuildings(TeamEnum team)
    {
        
    }
}
