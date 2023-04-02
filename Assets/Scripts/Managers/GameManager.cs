using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Events")]
    [SerializeField]
    private TeamEnumEvent _onTurnUpdate;

    private Dictionary<TeamEnum, int> _foodResources;
    private Dictionary<TeamEnum, int> _goldResources;

    [SerializeField]
    private int _turn;
    [SerializeField]
    private TeamEnum _currentTeam;

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public Dictionary<TeamEnum, int> FoodResources { get { return _foodResources; } }
    public Dictionary<TeamEnum, int> GoldResources { get { return _goldResources; } }

    private void Awake()
    {
        _instance = this;

        _foodResources = new Dictionary<TeamEnum, int>
        {
            {TeamEnum.BLUE, 0},
            {TeamEnum.RED, 0}
        };

        _goldResources = new Dictionary<TeamEnum, int>
        {
            {TeamEnum.BLUE, 0},
            {TeamEnum.RED, 0}
        };

    }

    private void Start()
    {
        if (_onTurnUpdate != null)
            _onTurnUpdate.Raise(_currentTeam);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log($"[EQUIPO AZUL] Food: {_foodResources[TeamEnum.BLUE]}, Gold: {_goldResources[TeamEnum.BLUE]}");
        }
    }

    public void FinalizeCurrentTurn()
    {
        _turn++;
        _currentTeam = (_currentTeam == TeamEnum.BLUE) ? TeamEnum.RED : TeamEnum.BLUE;

        if (_onTurnUpdate != null)
            _onTurnUpdate.Raise(_currentTeam);
    }

    public void UpdateResources(TeamEnum team, ResourceType resourceType, int amount)
    {
        switch (resourceType)
        {
            case ResourceType.FOOD:
                UpdateFoodResources(team, amount);
                break;
            case ResourceType.GOLD:
                UpdateGoldResources(team, amount);
                break;
            default:
                break;
        }
    }

    private void UpdateFoodResources(TeamEnum team, int amount)
    {
        if (_foodResources[team] < amount * -1f)
            Debug.LogWarning("[FOOD RESOURCE] No se puede actualizar el rescuros a un número negativo. No se realizará la actuaización.");
        else
            _foodResources[team] += amount;
    }

    private void UpdateGoldResources(TeamEnum team, int amount)
    {
        if (_goldResources[team] < amount * -1f)
            Debug.LogWarning("[GOLD RESOURCE] No se puede actualizar el rescuros a un número negativo. No se realizará la actuaización.");
        else
            _goldResources[team] += amount;
    }
}
