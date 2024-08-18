using Common.Base.Interfaces;
using TracksLyrics.Domain.Models.Mongo;


namespace TracksLyrics.Domain.Interfaces.Repositories.Mongo;

public interface ILyricRepository : IBaseRepository<LyricModel, Guid>
{
}