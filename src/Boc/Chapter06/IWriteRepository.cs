using LaYumba.Functional;

namespace Boc.EitherImpl.Services
{
   public interface IWriteRepository<T>
   {
      Either<Error, Unit> Save(T entity);
   }
}