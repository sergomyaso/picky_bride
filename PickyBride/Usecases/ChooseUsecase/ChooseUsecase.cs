using PickyBride.Domain.Entities;
using PickyBride.Domain.Repository.ContenderRepository;

namespace PickyBride.Usecases.ChooseUsecase;

public class ChooseUsecase
{
    private Hall _hall;
    private Princess _princess;
    private IContenderRepository _repository;

    public ChooseUsecase(IContenderRepository repository, Hall hall, Princess princess)
    {
        _repository = repository;
        _hall = hall;
        _princess = princess;
    }

    public SimulateChooseOutput SimulateChoose(SimulateChooseInput input)
    {
        var contenders = _repository.GetAttemptContenders(input.AttemptNumber);
        if (contenders == null || contenders.Count == 0)
        {
            throw new Exception();
        }
        
        _hall.FillHall(contenders);
        _princess.PreparePrincess(input.CountContenders);
        
        
        for (var nextContender = _hall.GetNextContender();
             nextContender != null;
             nextContender = _hall.GetNextContender())
        {
            if (_princess.IsContenderFutureHusband(nextContender))
            {
                break;
            }
        }
        
        return new SimulateChooseOutput(_princess.GetHappinessLevel());
    }
}