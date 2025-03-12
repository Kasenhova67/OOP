class ActionManager
{
    private Stack<IAction> actions = new Stack<IAction>();
    private Stack<IAction> redoActions = new Stack<IAction>();

    public void ExecuteAction(IAction action)
    {
        action.Execute();
        actions.Push(action);
        redoActions.Clear();
    }

    public void Undo()
    {
        if (actions.Count > 0)
        {
            IAction action = actions.Pop();
            action.Undo();
            redoActions.Push(action);
        }
    }

    public void Redo()
    {
        if (redoActions.Count > 0)
        {
            IAction action = redoActions.Pop();
            action.Execute();
            actions.Push(action);
        }
    }
}