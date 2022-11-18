namespace PickyBride.Domain.Repository.ContenderRepository;

public class ContenderNotFoundException : ApplicationException
{
    public ContenderNotFoundException()
    {
    }

    public ContenderNotFoundException(string message) : base(message)
    {
    }
}