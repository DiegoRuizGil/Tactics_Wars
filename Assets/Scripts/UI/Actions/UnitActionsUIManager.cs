using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionsUIManager : MonoBehaviour
{
    [Header("Unit Actions")]
    [SerializeField]
    private Button _moveButton;
    [SerializeField]
    private Button _attackButton;
    [SerializeField]
    private Button _finalizeButton;
    [SerializeField]
    private Button _buildButton;
    [SerializeField]
    private Button _repairButton;

    private const int NUM_OF_BUTTONS = 5;
    private Button[] _actionsButtons;
    private Unit _unit;
    private Node _unitNode;

    void Awake()
    {
        _actionsButtons = new Button[NUM_OF_BUTTONS];

        _actionsButtons[0] = _moveButton;
        _actionsButtons[1] = _attackButton;
        _actionsButtons[2] = _finalizeButton;
        _actionsButtons[3] = _buildButton;
        _actionsButtons[4] = _repairButton;
    }

    public void ShowUnitActions(Unit unit)
    {
        _unit = unit;
        _unitNode = Grid.Instance.GetNode(_unit.transform.position);

        if (_unit.HasFinished)
        {
            DeactivateButtons();
        }
        else
        {
            ShowMoveButton();
            ShowAttackButton();
            ShowFinalizeButton();
            ShowBuildButton();
            ShowRepairButton();
        }
    }

    private void DeactivateButtons()
    {
        foreach (Button button in _actionsButtons)
        {
            button.interactable = false;
        }
    }

    private void ShowMoveButton()
    {
        _moveButton.interactable = !_unit.HasMoved;
    }

    private void ShowAttackButton()
    {
        List<Vector3> attackArea = Pathfinding.Instance.GetAttackArea(
            _unit.transform.position,
            _unit.AttackRange);

        _attackButton.interactable = attackArea.Count > 0;
    }

    private void ShowFinalizeButton()
    {
        _finalizeButton.interactable = true;
    }

    private void ShowBuildButton()
    {
        if (_unit.gameObject.GetComponent<BuildingGenerator>() == null)
        {
            _buildButton.gameObject.SetActive(false);
            return;
        }

        _buildButton.gameObject.SetActive(true);

        if (_unitNode.CanBuildUnitBuilding(_unit.Team))
            _buildButton.interactable = true;
        else if (_unitNode.CanBuildFarm(_unit.Team))
            _buildButton.interactable = true;
        else if (_unitNode.Resource != ResourceType.NONE && _unitNode.GetEntity(0) == null)
            _buildButton.interactable = true;
        else
            _buildButton.interactable = false;
    }

    private void ShowRepairButton()
    {
        if (_unit.UnitType != UnitType.ALDEANO)
        {
            _repairButton.gameObject.SetActive(false);
            return;
        }

        _repairButton.gameObject.SetActive(true);

        Entity building = _unitNode.GetEntity(0);
        if (building != null)
        {
            _repairButton.interactable = building.MaxHealth != building.CurrentHealth;
        }
        else
        {
            _repairButton.interactable = false;
        }
    }
}
