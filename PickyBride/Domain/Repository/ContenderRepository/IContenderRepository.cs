using PickyBride.Domain.Entities;

namespace PickyBride.Domain.Repository.ContenderRepository;

public interface IContenderRepository
{
    Queue<Contender> GetAll(ContenderGetAllFilter filter);
}