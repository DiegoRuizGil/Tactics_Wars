using UnityEngine;

public class InputNoActionState : InputBaseState
{
    public InputNoActionState(InputManager context, InputStateFactory factory)
        : base(context, factory) { }

    public override void EnterState()
    {
        Debug.Log("<color=green>NoAction</color>: Entering the state");
        Context.SelectedUnit = null;
        if (Context.OnEntityDeselectedEvent != null)
            Context.OnEntityDeselectedEvent.Raise();
    }

    public override void UpdateState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Context.GetMouseWorldPosition();
            Node node = Grid.Instance.GetNode(mousePosition);
            if (node != null)
            {
                Entity entity = node.GetTopEntity();
                if (entity == null)
                    Context.SetSelectedEntity(null);
                else if (entity.Team != GameManager.Instance.PlayerTeam)
                    Context.SetSelectedEntity(null);
                else if (entity.Team == GameManager.Instance.PlayerTeam)
                    Context.SetSelectedEntity(entity);
            }
        }
    }
}
