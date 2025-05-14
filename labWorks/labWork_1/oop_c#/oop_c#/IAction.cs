// Abstraction for actions (Undo/Redo)
interface IAction
{
    void Execute();
    void Undo();
}