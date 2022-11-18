namespace PickyBride.Common.Generators;

public class IdIntGenerator
{
    private int _currIdValue;

    public IdIntGenerator()
    {
        _currIdValue = 0;
    }

    public int GenerateId()
    {
        _currIdValue++;

        return _currIdValue;
    }
}