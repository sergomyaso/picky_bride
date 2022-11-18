using System.Collections;
using System.Data;
using System.Net.Http.Json;
using System.Numerics;
using System.Text.Json;
using System.Web;
using PickyBride.Common.Generators;
using PickyBride.Domain.Entities;
using PickyBride.Domain.Repository.ContenderRepository;

namespace PickyBride.Infrastructure.ContenderRepository.RDT;

// https://randomdatatools.ru/developers/
public class RDTContenderRepository : IContenderRepository
{
    private const int MinQualityValue = 1;
    private const int MaxQualityValue = 100;

    private readonly string _baseUrl;
    private readonly Random _randomGenerator;
    private readonly IdIntGenerator _idGenerator;
    private readonly HttpClient _client;

    public RDTContenderRepository(HttpClient client, IdIntGenerator idGenerator, Random random)
    {
        _randomGenerator = random;
        _idGenerator = idGenerator;
        _client = client;
        _baseUrl = "https://api.randomdatatools.ru/"; // TODO пропихнуть через ENV
    }

    public Queue<Contender> GetAll(ContenderGetAllFilter filter)
    {
        var names = FindContendersNames(filter);
        if (names == null)
        {
            // TODO добавить логику выгруза людей из дампа
            throw new ContenderNotFoundException();
        }

        var contenders = new List<Contender>();

        names.ForEach((item) => contenders.Add(CreateContenderFromDto(item)));

        return new Queue<Contender>(contenders);
    }

    private List<RDTContenderDto>? FindContendersNames(ContenderGetAllFilter filter)
    {
        var uriBuilder = new UriBuilder(_baseUrl);
        var queryParams = HttpUtility.ParseQueryString(uriBuilder.Query);

        queryParams["gender"] = "man";
        queryParams["count"] = filter.Count.ToString();
        queryParams["typeName"] = "rare";
        queryParams["params"] = "LastName,FirstName";

        uriBuilder.Query = queryParams.ToString();
        
        var reqTask = _client.GetFromJsonAsync<List<RDTContenderDto>>(uriBuilder.Uri.ToString());
        reqTask.Wait();

        return reqTask.Result;
    }

    private Contender CreateContenderFromDto(RDTContenderDto dto)
    {
        return new Contender(
            _idGenerator.GenerateId(),
            dto.FirstName,
            dto.LastName,
            _randomGenerator.Next(MinQualityValue, MaxQualityValue)
        );
    }
}