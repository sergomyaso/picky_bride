using PickyBride.Domain.Entities;
using PickyBride.Domain.Repository.ContenderRepository;

namespace PickyBride.Usecases.GenerateChooseUsecase;

public class GenerateChooseUsecase
{
    private IContenderRepository _repository;

    public GenerateChooseUsecase(IContenderRepository repository)
    {
        _repository = repository;
    }

    public GenerateChooseOutput GenerateAttempts(GenerateChooseInput input)
    {
        var attempts = input.CountAttempts;
        while (attempts != 0)
        {
            var contenders = _repository.GenerateContenders(new GenerateContendersFilter(input.CountContenders));
            _repository.SaveAttemptContenders(attempts, contenders);
            attempts--;
        }

        return new GenerateChooseOutput(true);
    }
}