using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    [Header("Unit Building Settings")]
    [SerializeField]
    private UnitInfoSO[] _unitsInfo;

    public UnitInfoSO[] UnitsInfo { get { return _unitsInfo; } }
}
