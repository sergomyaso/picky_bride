using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using PickyBride.Common.Generators;
using PickyBride.Domain.Entities;
using PickyBride.Domain.Repository.ContenderRepository;
using PickyBride.Infrastructure.Db;
using StackExchange.Redis;

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
    private readonly IDatabase _db;

    public RDTContenderRepository(HttpClient client, IdIntGenerator idGenerator, Random random)
    {
        _randomGenerator = random;
        _idGenerator = idGenerator;
        _client = client;
        _baseUrl = "https://api.randomdatatools.ru/"; // TODO пропихнуть через ENV
        _db = RedisConn.GetRedisDatabase();
    }

    public void SaveAttemptContenders(int attempt, Queue<Contender> contenders)
    {
        var res = _db.StringSet(attempt.ToString(), JsonSerializer.Serialize(contenders));

        Console.Out.WriteLine(res);
    }

    public Queue<Contender>? GetAttemptContenders(int attempt)
    {
        var val = _db.StringGet(attempt.ToString());

        return JsonSerializer.Deserialize<Queue<Contender>>(val.ToString());
    }

    public Queue<Contender> GenerateContenders(GenerateContendersFilter filter)
    {
        var names = FindContendersNames(filter);
        if (names == null || names.Count == 0)
        {
            // если ничего не пришло от апишки, берем из хардкода, чтобы наверняка :) 
            names = FindLocalContendersNames(filter);
            if (names == null || names.Count == 0)
            {
                throw new ContenderNotFoundException();
            }
        }

        var contenders = new List<Contender>();

        names.ForEach((item) => contenders.Add(CreateContenderFromDto(item)));

        return new Queue<Contender>(contenders);
    }

    private List<RDTContenderDto>? FindLocalContendersNames(GenerateContendersFilter filter)
    {
        var jsonNames = GetLocalContenderJsonNames();

        return JsonSerializer.Deserialize<List<RDTContenderDto>>(jsonNames);
    }

    private List<RDTContenderDto>? FindContendersNames(GenerateContendersFilter filter)
    {
        var uriBuilder = new UriBuilder(_baseUrl);
        var queryParams = HttpUtility.ParseQueryString(uriBuilder.Query);

        queryParams["gender"] = "man";
        queryParams["count"] = filter.Count.ToString();
        queryParams["typeName"] = "rare";
        queryParams["params"] = "LastName,FirstName";

        uriBuilder.Query = queryParams.ToString();

        try
        {
            var reqTask = _client.GetFromJsonAsync<List<RDTContenderDto>>(uriBuilder.Uri.ToString());
            reqTask.Wait();

            return reqTask.Result;
        }
        catch (Exception e)
        {
            return null;
        }
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

    private string GetLocalContenderJsonNames()
    {
        return @"[
  {
    ""LastName"": ""Бильбасов"",
    ""FirstName"": ""Лаврентий""
  },
  {
    ""LastName"": ""Ковтунов"",
    ""FirstName"": ""Петр""
  },
  {
    ""LastName"": ""Славаков"",
    ""FirstName"": ""Филипп""
  },
  {
    ""LastName"": ""Вольпов"",
    ""FirstName"": ""Николай""
  },
  {
    ""LastName"": ""Ярославцев"",
    ""FirstName"": ""Ефим""
  },
  {
    ""LastName"": ""Кидирбаев"",
    ""FirstName"": ""Антон""
  },
  {
    ""LastName"": ""Шибалов"",
    ""FirstName"": ""Ростислав""
  },
  {
    ""LastName"": ""Дворянкин"",
    ""FirstName"": ""Тарас""
  },
  {
    ""LastName"": ""Кантонистов"",
    ""FirstName"": ""Александр""
  },
  {
    ""LastName"": ""Голованов"",
    ""FirstName"": ""Степан""
  },
  {
    ""LastName"": ""Чукреев"",
    ""FirstName"": ""Георгий""
  },
  {
    ""LastName"": ""Зощенко"",
    ""FirstName"": ""Никита""
  },
  {
    ""LastName"": ""Соломинцев"",
    ""FirstName"": ""Максим""
  },
  {
    ""LastName"": ""Херман"",
    ""FirstName"": ""Степан""
  },
  {
    ""LastName"": ""Убейсобакин"",
    ""FirstName"": ""Лев""
  },
  {
    ""LastName"": ""Азаренков"",
    ""FirstName"": ""Арсений""
  },
  {
    ""LastName"": ""Эверский"",
    ""FirstName"": ""Виталий""
  },
  {
    ""LastName"": ""Андронов"",
    ""FirstName"": ""Георгий""
  },
  {
    ""LastName"": ""Лаптев"",
    ""FirstName"": ""Павел""
  },
  {
    ""LastName"": ""Канаев"",
    ""FirstName"": ""Константин""
  },
  {
    ""LastName"": ""Ярмольник"",
    ""FirstName"": ""Илья""
  },
  {
    ""LastName"": ""Цой"",
    ""FirstName"": ""Макар""
  },
  {
    ""LastName"": ""Спиридонов"",
    ""FirstName"": ""Юлиан""
  },
  {
    ""LastName"": ""Алистратов"",
    ""FirstName"": ""Валерий""
  },
  {
    ""LastName"": ""Мысляев"",
    ""FirstName"": ""Семен""
  },
  {
    ""LastName"": ""Борисюк"",
    ""FirstName"": ""Василий""
  },
  {
    ""LastName"": ""Эмских"",
    ""FirstName"": ""Вениамин""
  },
  {
    ""LastName"": ""Цыганов"",
    ""FirstName"": ""Антон""
  },
  {
    ""LastName"": ""Чаадаев"",
    ""FirstName"": ""Макар""
  },
  {
    ""LastName"": ""Вазов"",
    ""FirstName"": ""Илья""
  },
  {
    ""LastName"": ""Курсалин"",
    ""FirstName"": ""Константин""
  },
  {
    ""LastName"": ""Фаммус"",
    ""FirstName"": ""Петр""
  },
  {
    ""LastName"": ""Поджио"",
    ""FirstName"": ""Геннадий""
  },
  {
    ""LastName"": ""Васильев"",
    ""FirstName"": ""Даниил""
  },
  {
    ""LastName"": ""Алистратов"",
    ""FirstName"": ""Савва""
  },
  {
    ""LastName"": ""Николаевский"",
    ""FirstName"": ""Леонид""
  },
  {
    ""LastName"": ""Бердяев"",
    ""FirstName"": ""Никита""
  },
  {
    ""LastName"": ""Янчуров"",
    ""FirstName"": ""Валерий""
  },
  {
    ""LastName"": ""Быстров"",
    ""FirstName"": ""Степан""
  },
  {
    ""LastName"": ""Грибалев"",
    ""FirstName"": ""Илья""
  },
  {
    ""LastName"": ""Алогрин"",
    ""FirstName"": ""Ростислав""
  },
  {
    ""LastName"": ""Капралов"",
    ""FirstName"": ""Данила""
  },
  {
    ""LastName"": ""Чучанов"",
    ""FirstName"": ""Артем""
  },
  {
    ""LastName"": ""Минкин"",
    ""FirstName"": ""Ростислав""
  },
  {
    ""LastName"": ""Ягфаров"",
    ""FirstName"": ""Геннадий""
  },
  {
    ""LastName"": ""Земляков"",
    ""FirstName"": ""Филипп""
  },
  {
    ""LastName"": ""Лутугин"",
    ""FirstName"": ""Леонтий""
  },
  {
    ""LastName"": ""Воеводин"",
    ""FirstName"": ""Валентин""
  },
  {
    ""LastName"": ""Щетинин"",
    ""FirstName"": ""Аркадий""
  },
  {
    ""LastName"": ""Козин"",
    ""FirstName"": ""Юлиан""
  },
  {
    ""LastName"": ""Сиялов"",
    ""FirstName"": ""Илья""
  },
  {
    ""LastName"": ""Потрепалов"",
    ""FirstName"": ""Петр""
  },
  {
    ""LastName"": ""Кудяшов"",
    ""FirstName"": ""Евгений""
  },
  {
    ""LastName"": ""Палюлин"",
    ""FirstName"": ""Макар""
  },
  {
    ""LastName"": ""Тенишев"",
    ""FirstName"": ""Яков""
  },
  {
    ""LastName"": ""Ахметов"",
    ""FirstName"": ""Филипп""
  },
  {
    ""LastName"": ""Стрекалов"",
    ""FirstName"": ""Леонид""
  },
  {
    ""LastName"": ""Топорков"",
    ""FirstName"": ""Ефим""
  },
  {
    ""LastName"": ""Амелин"",
    ""FirstName"": ""Геннадий""
  },
  {
    ""LastName"": ""Выгузов"",
    ""FirstName"": ""Валентин""
  },
  {
    ""LastName"": ""Шимякин"",
    ""FirstName"": ""Денис""
  },
  {
    ""LastName"": ""Сарана"",
    ""FirstName"": ""Семен""
  },
  {
    ""LastName"": ""Уицкий"",
    ""FirstName"": ""Вениамин""
  },
  {
    ""LastName"": ""Кулигин"",
    ""FirstName"": ""Роман""
  },
  {
    ""LastName"": ""Ижутин"",
    ""FirstName"": ""Савва""
  },
  {
    ""LastName"": ""Котяш"",
    ""FirstName"": ""Денис""
  },
  {
    ""LastName"": ""Владимиров"",
    ""FirstName"": ""Виктор""
  },
  {
    ""LastName"": ""Яушев"",
    ""FirstName"": ""Антон""
  },
  {
    ""LastName"": ""Новичков"",
    ""FirstName"": ""Федор""
  },
  {
    ""LastName"": ""Белоглазов"",
    ""FirstName"": ""Аркадий""
  },
  {
    ""LastName"": ""Кравцов"",
    ""FirstName"": ""Ефим""
  },
  {
    ""LastName"": ""Янушковский"",
    ""FirstName"": ""Афанасий""
  },
  {
    ""LastName"": ""Дружинин"",
    ""FirstName"": ""Егор""
  },
  {
    ""LastName"": ""Рязанцев"",
    ""FirstName"": ""Александр""
  },
  {
    ""LastName"": ""Эзрин"",
    ""FirstName"": ""Филипп""
  },
  {
    ""LastName"": ""Кашников"",
    ""FirstName"": ""Валентин""
  },
  {
    ""LastName"": ""Кружков"",
    ""FirstName"": ""Роман""
  },
  {
    ""LastName"": ""Мячиков"",
    ""FirstName"": ""Денис""
  },
  {
    ""LastName"": ""Дробышев"",
    ""FirstName"": ""Константин""
  },
  {
    ""LastName"": ""Лобза"",
    ""FirstName"": ""Валерий""
  },
  {
    ""LastName"": ""Окладников"",
    ""FirstName"": ""Антон""
  },
  {
    ""LastName"": ""Чехов"",
    ""FirstName"": ""Федор""
  },
  {
    ""LastName"": ""Котельников"",
    ""FirstName"": ""Георгий""
  },
  {
    ""LastName"": ""Худяков"",
    ""FirstName"": ""Игнат""
  },
  {
    ""LastName"": ""Парамонов"",
    ""FirstName"": ""Трофим""
  },
  {
    ""LastName"": ""Яимов"",
    ""FirstName"": ""Сергей""
  },
  {
    ""LastName"": ""Катькин"",
    ""FirstName"": ""Семен""
  },
  {
    ""LastName"": ""Ярошенко"",
    ""FirstName"": ""Ефим""
  },
  {
    ""LastName"": ""Насонов"",
    ""FirstName"": ""Максим""
  },
  {
    ""LastName"": ""Минеев"",
    ""FirstName"": ""Тимофей""
  },
  {
    ""LastName"": ""Сомкин"",
    ""FirstName"": ""Егор""
  },
  {
    ""LastName"": ""Власов"",
    ""FirstName"": ""Филипп""
  },
  {
    ""LastName"": ""Яхимович"",
    ""FirstName"": ""Михаил""
  },
  {
    ""LastName"": ""Терещенко"",
    ""FirstName"": ""Георгий""
  },
  {
    ""LastName"": ""Бурдуковский"",
    ""FirstName"": ""Иван""
  },
  {
    ""LastName"": ""Шипицин"",
    ""FirstName"": ""Илья""
  },
  {
    ""LastName"": ""Барышников"",
    ""FirstName"": ""Емельян""
  },
  {
    ""LastName"": ""Ермолаев"",
    ""FirstName"": ""Валентин""
  },
  {
    ""LastName"": ""Ляхов"",
    ""FirstName"": ""Роман""
  },
  {
    ""LastName"": ""Грибов"",
    ""FirstName"": ""Юлиан""
  }
]";
    }
}