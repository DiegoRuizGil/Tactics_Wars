using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntityInfoMenu : MonoBehaviour
{
    [Header("Images")]
    [SerializeField]
    private Image _unitImage;
    [SerializeField]
    private Image _buildingImage;

    [Space(10)]
    [SerializeField]
    private TextMeshProUGUI _entityName;

    [Header("Stats")]
    [SerializeField]
    private GameObject _health;
    [SerializeField]
    private GameObject _attack;
    [SerializeField]
    private GameObject _attackRange;
    [SerializeField]
    private GameObject _movementRange;
    [SerializeField]
    private GameObject _food;
    [SerializeField]
    private GameObject _gold;

    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            Node node = Grid.Instance.GetNode(mousePosition);
            if (node != null)
            {
                Entity entity = node.GetTopEntity();
                if (entity != null)
                    ShowEntityInfo(entity);
                else
                    ClearInfo();
            }
        }
    }

    private void ShowEntityInfo(Entity entity)
    {
        _entityName.text = entity.Name;

        _health.GetComponentInChildren<TextMeshProUGUI>().text = $"{entity.CurrentHealth}/{entity.MaxHealth}";
        _health.SetActive(true);

        if (entity is Unit)
        {
            Unit unit = entity as Unit;

            _unitImage.sprite = unit.EntityImage;
            Material material = _unitImage.material;
            material.SetFloat("_IsRedTeam", unit.Team == TeamEnum.BLUE ? 0f : 1f);
            _unitImage.gameObject.SetActive(true);
            _buildingImage.gameObject.SetActive(false);

            _attack.GetComponentInChildren<TextMeshProUGUI>().text = $"{unit.Damage}";
            _attack.SetActive(true);

            _attackRange.GetComponentInChildren<TextMeshProUGUI>().text = $"{unit.AttackRange}";
            _attackRange.SetActive(true);

            _movementRange.GetComponentInChildren<TextMeshProUGUI>().text = $"{unit.MovementRange}";
            _movementRange.SetActive(true);

            _food.SetActive(false);
            _gold.SetActive(false);
        }
        else if (entity is Building)
        {
            Building building = entity as Building;

            _buildingImage.sprite = building.EntityImage;
            Material material = _buildingImage.material;
            material.SetFloat("_IsRedTeam", building.Team == TeamEnum.BLUE ? 0f : 1f);
            _buildingImage.gameObject.SetActive(true);
            _unitImage.gameObject.SetActive(false);

            ResourceGenerator[] generators = building.GetComponents<ResourceGenerator>();

            _food.SetActive(false);
            _gold.SetActive(false);
            foreach (ResourceGenerator generator in generators)
            {
                if (generator.ResourceType == ResourceType.FOOD)
                {
                    _food.GetComponentInChildren<TextMeshProUGUI>().text = $"+{generator.ResourceAmount}";
                    _food.SetActive(true);
                }
                else if (generator.ResourceType == ResourceType.GOLD)
                {
                    _gold.GetComponentInChildren<TextMeshProUGUI>().text = $"+{generator.ResourceAmount}";
                    _gold.SetActive(true);
                }
            }

            _attack.SetActive(false);
            _attackRange.SetActive(false);
            _movementRange.SetActive(false);
        }
    }

    private void ClearInfo()
    {
        _entityName.text = "";

        _unitImage.gameObject.SetActive(false);
        _buildingImage.gameObject.SetActive(false);

        _health.SetActive(false);
        _attack.SetActive(false);
        _attackRange.SetActive(false);
        _movementRange.SetActive(false);
        _food.SetActive(false);
        _gold.SetActive(false);
    }

    public Vector3 GetMouseWorldPosition()
    {
        return _camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
