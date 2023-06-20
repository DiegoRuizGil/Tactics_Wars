using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Variables
    [Header("Actions Settings")]
    [SerializeField]
    private int _movementSpeed = 1;

    [Header("Game Events")]
    [SerializeField]
    private UnitEvent _onUnitSelectedEvent;
    [SerializeField]
    private VoidEvent _onEntityDeselectedEvent;
    [SerializeField]
    private BuildingEvent _onBuildingSelectedEvent;
    [SerializeField]
    private ListVector3Event _updateActionAreaEvent;
    [SerializeField]
    private ListVector3Event _updateMovementArrowEvent;

    [Header("Entity Settings")]
    [SerializeField]
    private Transform _unitsParentBlueTeam;
    [SerializeField]
    private Transform _buildingsParentBlueTeam;
    [SerializeField]
    private Transform _unitsParentRedTeam;
    [SerializeField]
    private Transform _buildingsParentRedTeam;

    private Camera _camera;

    private Unit _selectedUnit;
    private Building _selectedBuilding;

    private ActionHandler _actionHandler;
    private InputBaseState _currentState;
    private InputStateFactory _states;

    private bool _canDoActions = true;
    #endregion

    #region Getters and Setters
    public int MovementSpeed { get { return _movementSpeed; } }
    public Unit SelectedUnit { get { return _selectedUnit; } set { _selectedUnit = value; } }
    public Building SelectedBuilding { get { return _selectedBuilding; } set { _selectedBuilding = value; } }
    public VoidEvent OnEntityDeselectedEvent { get { return _onEntityDeselectedEvent; } }
    public ListVector3Event UpdateActionAreaEvent { get { return _updateActionAreaEvent; } }
    public ListVector3Event UpdateMovementArrowEvent { get { return _updateMovementArrowEvent; } }
    public Transform UnitsParentBlueTeam { get { return _unitsParentBlueTeam; } }
    public Transform BuildingsParentBlueTeam { get { return _buildingsParentBlueTeam; } }
    public Transform UnitsParentRedTeam { get { return _unitsParentRedTeam; } }
    public Transform BuildingsParentRedTeam { get { return _buildingsParentRedTeam; } }
    public InputBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public ActionHandler ActionHandler { get { return _actionHandler; } set { _actionHandler = value; } }
    #endregion

    void Start()
    {
        _actionHandler = new ActionHandler();
        _states = new InputStateFactory(this);
        _currentState = _states.NoAction();
        _currentState.EnterState();
        _camera = Camera.main;
    }

    void Update()
    {
        _currentState.UpdateState();
    }

    public void SwitchState(InputBaseState state)
    {
        _currentState = state;
        state.EnterState();
    }

    public void ClearSelectedEntities() // end turn button
    {
        SetSelectedEntity(null);
    }

    public void SetSelectedEntity(Entity entity)
    {
        if (entity == null)
        {
            _selectedUnit = null;
            _selectedBuilding = null;
            _onEntityDeselectedEvent.Raise();
        }
        else if (!_canDoActions)
        {
            Debug.Log("No puedes seleccionar unidades para realizar acciones");
        }
        else if (entity is Unit)
        {
            _selectedUnit = entity as Unit;
            _onUnitSelectedEvent.Raise(_selectedUnit);
        }
        else if (entity is Building)
        {
            _selectedBuilding = entity as Building;
            
            if (_selectedBuilding.GetComponent<UnitGenerator>() != null)
                _onBuildingSelectedEvent.Raise(_selectedBuilding);
        }
    }

    public void BlockPlayerActions()
    {
        _canDoActions = false;
    }

    public void SetCanDoActions(TeamEnum team)
    {
        _canDoActions = team == GameManager.Instance.PlayerTeam;
    }

    #region OnClick Events
    public void SetMoveState()
    {
        SwitchState(_states.MoveAction());
    }

    public void SetAttackState()
    {
        SwitchState(_states.AttackAction());
    }

    public void SetBuildState(BuildingInfoSO buildingInfo)
    {
        SwitchState(_states.BuildAction(buildingInfo));
    }

    public void SetGenerateUnitState(UnitInfoSO unitInfo)
    {
        SwitchState(_states.GenerateUnitAction(unitInfo));
    }

    public void SetRepairState()
    {
        SwitchState(_states.RepairAction());
    }

    public void FinalizeUnit()
    {
        SwitchState(_states.FinalizeUnit());
    }
    #endregion

    public Vector3 GetMouseWorldPosition()
    {
        return _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    public Vector3 GetMouseNodePosition()
    {
        return Grid.Instance.GetNodeWorldPosition(GetMouseWorldPosition());
    }
}
