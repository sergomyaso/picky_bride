using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PickyBride.Common.Generators;
using PickyBride.Domain.Entities;
using PickyBride.Domain.Entities.FriendEntity;
using PickyBride.Domain.Repository.ContenderRepository;
using PickyBride.HostedServices;
using PickyBride.Infrastructure.ContenderRepository.RDT;
using PickyBride.Usecases.ChooseUsecase;

namespace PickyBride;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<PrincessHostedService>();
                services.AddScoped<ChooseUsecase>();
                services.AddScoped<IContenderRepository, RDTContenderRepository>();
                services.AddScoped<HttpClient>();
                services.AddScoped<IdIntGenerator>();
                services.AddScoped<Random>();
                services.AddScoped<Princess>();
                services.AddScoped<Friend>();
                services.AddScoped<Hall>();
            });
}