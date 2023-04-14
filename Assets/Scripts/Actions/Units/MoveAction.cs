using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private const float POSITION_OFFSET = 0.5f;
    private readonly int _unitSpeed;
    private readonly List<Vector3> _positions;
    private readonly Unit _selectedUnit;

    public MoveAction(Unit selectedUnit, List<Vector3> positions, int unitSpeed)
    {

        _selectedUnit = selectedUnit;
        _positions = positions;
        _unitSpeed = unitSpeed;
        _isRunning = true;
    }

    public override void Execute()
    {
        if (!UpdateNodes())
        {
            _isRunning = false;
            return;
        }
        
        _selectedUnit.StartCoroutine(MovementCoroutine());
    }

    private bool UpdateNodes()
    {
        if (_positions.Count == 0)
            return false;
        if (Grid.Instance.GetNode(_positions[^1]) == null)
            return false;

        Node currentNode = Grid.Instance.GetNode(_selectedUnit.transform.position);
        currentNode.RemoveTopEntity();

        Node nextNode = Grid.Instance.GetNode(_positions[^1]);
        nextNode.AddEntity(_selectedUnit);

        return true;
    }

    private IEnumerator MovementCoroutine()
    {
        _selectedUnit.Animator.SetBool("IsMoving", true);

        int index = 0;
        Vector3 nextPos = GetPositionWithOffset(_positions[index]);
        while (index < _positions.Count)
        {
            if (_selectedUnit.transform.position != nextPos)
            {
                _selectedUnit.transform.position = Vector3.MoveTowards(
                    _selectedUnit.transform.position,
                    nextPos,
                    _unitSpeed * Time.deltaTime);   
            }
            else
            {
                index += 1;
                if (index < _positions.Count)
                    nextPos = GetPositionWithOffset(_positions[index]);
            }
            yield return null;
        }

        _selectedUnit.Animator.SetBool("IsMoving", false);
        _selectedUnit.HasMoved = true;

        ActionFinished?.Invoke();
        _isRunning = false;
    }

    private Vector3 GetPositionWithOffset(Vector3 position)
    {
        return new Vector3(position.x + POSITION_OFFSET, position.y + POSITION_OFFSET, 0f);
    }
}
