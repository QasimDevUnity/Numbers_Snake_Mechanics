public class AddOperation : INumberOperation
{
    private int addValue;
    public AddOperation(int value) { addValue = value; }

    public int Apply(int currentValue) => currentValue + addValue;
}

public class MultiplyOperation : INumberOperation
{
    private int value;
    public MultiplyOperation(int value) => this.value = value;

    public int Apply(int currentValue) => currentValue * value;
}