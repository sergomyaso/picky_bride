using Microsoft.Extensions.Hosting;
using PickyBride.Usecases.ChooseUsecase;

namespace PickyBride.HostedServices;

public class PrincessHostedService : IHostedService
{
    private ChooseUsecase _chooseUsecase;
    private int _attemptNumber;

    public PrincessHostedService(ChooseUsecase chooseUsecase, PrincessHostedServiceInput input)
    {
        _attemptNumber = input.AttemptNumber;
        _chooseUsecase = chooseUsecase;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(RunAsync);
        return Task.CompletedTask;
    }

    public void RunAsync()
    {
        int countContenders = 100;

        var output = _chooseUsecase.SimulateChoose(new SimulateChooseInput(countContenders, _attemptNumber));
        Console.Out.Write(
            $"Princess get level of happiness {output.PrincessHappinessLevel} from attempt {_attemptNumber}");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}