using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PickyBride.Common.Generators;
using PickyBride.Domain.Entities;
using PickyBride.Domain.Entities.FriendEntity;
using PickyBride.Domain.Repository.ContenderRepository;
using PickyBride.HostedServices;
using PickyBride.Infrastructure.ContenderRepository.RDT;
using PickyBride.Usecases.ChooseUsecase;
using PickyBride.Usecases.GenerateChooseUsecase;

namespace PickyBride;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            return;
        }

        switch (args[0])
        {
            case "generate":
                CreateGenerateAttemptHostBuilder(args).Build().Run();
                break;
            case "simulate":
                CreateChooseHostBuilder(args).Build().Run();
                break;
            default:
                Console.Out.WriteLine("Unsupported args");
                break;
        }
    }

    public static IHostBuilder CreateChooseHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<PrincessHostedService>();
                services.AddScoped<PrincessHostedServiceInput>(x => new PrincessHostedServiceInput(int.Parse(args[1])));
                services.AddScoped<ChooseUsecase>();
                services.AddScoped<IContenderRepository, RDTContenderRepository>();
                services.AddScoped<HttpClient>();
                services.AddScoped<IdIntGenerator>();
                services.AddScoped<Random>();
                services.AddScoped<Princess>();
                services.AddScoped<Friend>();
                services.AddScoped<Hall>();
            });

    public static IHostBuilder CreateGenerateAttemptHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<GeneratorChoiseHostedService>();
                services.AddScoped<GeneratorHostedServiceInput>(
                    x => new GeneratorHostedServiceInput(int.Parse(args[1])));
                services.AddScoped<GenerateChooseUsecase>();
                services.AddScoped<IContenderRepository, RDTContenderRepository>();
                services.AddScoped<HttpClient>();
                services.AddScoped<IdIntGenerator>();
                services.AddScoped<Random>();
            });
}