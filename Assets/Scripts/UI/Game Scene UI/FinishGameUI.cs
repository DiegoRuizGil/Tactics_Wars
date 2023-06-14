using UnityEngine;
using TMPro;

public class FinishGameUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _finishGameText;
    [SerializeField]
    private GameObject _panel;
    [SerializeField]
    private GameObject _gameFinishedMenu;

    public void ShowFinishGameMenu(TeamEnum winner)
    {
        if (winner == GameManager.Instance.PlayerTeam)
        {
            _finishGameText.text = "YOU HAVE WON!!";
        }
        else
        {
            _finishGameText.text = "YOU HAVE LOST...";
        }

        _panel.SetActive(true);
        _gameFinishedMenu.SetActive(true);
    }
}
