using System;

public abstract class BaseAction
{
    protected bool _isRunning;
    public bool IsRunning { get { return _isRunning; } }

    private Action _actionFinished;
    public Action ActionFinished { get { return _actionFinished; } set { _actionFinished = value; } }

    public abstract void Execute();

    protected void FinishAction()
    {
        _actionFinished?.Invoke();
    }
}
