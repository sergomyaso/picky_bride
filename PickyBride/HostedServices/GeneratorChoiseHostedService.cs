using Microsoft.Extensions.Hosting;
using PickyBride.Usecases.ChooseUsecase;
using PickyBride.Usecases.GenerateChooseUsecase;

namespace PickyBride.HostedServices;

public class GeneratorChoiseHostedService : IHostedService
{
    private GenerateChooseUsecase _chooseUsecase;
    private int _countAttempts;

    public GeneratorChoiseHostedService(GenerateChooseUsecase chooseUsecase, GeneratorHostedServiceInput input)
    {
        _countAttempts = input.CountAttempt;
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
        _chooseUsecase.GenerateAttempts(new GenerateChooseInput(_countAttempts, countContenders));
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}