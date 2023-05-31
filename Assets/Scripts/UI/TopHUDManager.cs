using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopHUDManager : MonoBehaviour
{
    [Header("Team Icons UI")]
    [SerializeField] private Image _blueTeamIcon;
    [SerializeField] private Image _redTeamIcon;

    [Header("Player Info")]
    [SerializeField] private TextMeshProUGUI _foodAmountText;
    [SerializeField] private TextMeshProUGUI _goldAmountText;
    [SerializeField] private TextMeshProUGUI _entitiesAmountText;


    [Header("Buttons UI")]
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private Button _exitButton;

    public void UpdateHUD(TeamEnum team)
    {
        UpdateTeamIcon(team);
        UpdateResourcesAmount();
        SetInteracionEndTurn(team);
    }

    private void UpdateTeamIcon(TeamEnum team)
    {
        Color blueIconColor = _blueTeamIcon.color;
        Color redIconColor = _redTeamIcon.color;

        blueIconColor.a = team == TeamEnum.BLUE ? 1f : 0.25f;
        redIconColor.a = team == TeamEnum.RED ? 1f : 0.25f;

        _blueTeamIcon.color = blueIconColor;
        _redTeamIcon.color = redIconColor;
    }

    public void UpdateResourcesAmount()
    {
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
