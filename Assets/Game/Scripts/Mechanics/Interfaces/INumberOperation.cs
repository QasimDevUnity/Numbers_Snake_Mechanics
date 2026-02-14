public interface INumberOperation
{
    int Apply(int currentValue);
}


public interface IInputProvider
{
    float GetHorizontal();
}


public interface IInteractable
{
    void Interact(SnakeManager snake);
}
