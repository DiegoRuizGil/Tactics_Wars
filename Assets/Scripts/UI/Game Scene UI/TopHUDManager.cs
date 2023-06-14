using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopHUDManager : MonoBehaviour
{
    [Header("Turn Info")]
    [SerializeField] private TextMeshProUGUI _turnText;
    [SerializeField] private Image _blueTeamIcon;
    [SerializeField] private Image _redTeamIcon;

    [Header("Player Info")]
    [SerializeField] private TextMeshProUGUI _foodAmountText;
    [SerializeField] private TextMeshProUGUI _goldAmountText;
    [SerializeField] private TextMeshProUGUI _entitiesAmountText;


    [Header("Buttons UI")]
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private Button _exitButton;

    [Space(10)]
    [SerializeField] private InputManager _inputManager;

    private bool _activateButtons = false;

    private void Update()
    {
        if (_inputManager.CurrentState is InputWaitingState)
        {
            _endTurnButton.interactable = false;
            _exitButton.interactable = false;

            _activateButtons = true;
        }
        else if (_activateButtons)
        {
            _endTurnButton.interactable = true;
            _exitButton.interactable = true;

            _activateButtons = false;
        }
    }

    public void UpdateHUD(TeamEnum team)
    {
        UpdateTurnInfo(team);
        UpdatePlayerInfo();
        SetInteracionEndTurn(team);
    }

    private void UpdateTurnInfo(TeamEnum team)
    {
        _turnText.text = $"{Mathf.CeilToInt(GameManager.Instance.Turn / 2f)}";

        Color blueIconColor = _blueTeamIcon.color;
        Color redIconColor = _redTeamIcon.color;

        blueIconColor.a = team == TeamEnum.BLUE ? 1f : 0.25f;
        redIconColor.a = team == TeamEnum.RED ? 1f : 0.25f;

        _blueTeamIcon.color = blueIconColor;
        _redTeamIcon.color = redIconColor;
    }

    public void UpdatePlayerInfo()
    {
        _turnText.text = $"{Mathf.CeilToInt(GameManager.Instance.Turn / 2f)}";

        TeamEnum playerTeam = GameManager.Instance.PlayerTeam;

        _foodAmountText.text = GameManager.Instance.FoodResources[playerTeam].ToString();
        _goldAmountText.text = GameManager.Instance.GoldResources[playerTeam].ToString();

        int entitiesAmount = GameManager.Instance.UnitLists[playerTeam].Count;
        int maxEntities = GameManager.Instance.MaxUnitAmount;
        _entitiesAmountText.text = $"{entitiesAmount}/{maxEntities}";
    }

    private void SetInteracionEndTurn(TeamEnum team)
    {
        _endTurnButton.interactable = team == GameManager.Instance.PlayerTeam;
        _exitButton.interactable = team == GameManager.Instance.PlayerTeam;
    }
}
