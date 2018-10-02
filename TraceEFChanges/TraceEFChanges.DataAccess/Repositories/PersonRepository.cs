using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TraceEFChanges.DataAccess.Model;

namespace TraceEFChanges.DataAccess.Repositories
{
    public class PersonRepository
    {
        private readonly TraceContext _traceContext;

        public PersonRepository()
        {
            _traceContext = new TraceContext();
        }

        public async Task<PersonEntity> Add(PersonEntity entity)
        {
            await _traceContext.Persons.AddAsync(entity);
            await _traceContext.SaveChangesAsync();

            return entity;
        }

        public async Task<PersonEntity> GetFirst()
        {
            return await _traceContext.Persons
                .FirstOrDefaultAsync();
        }

        public async Task<PersonEntity> Update(PersonEntity entity)
        {
            var updatedEntity = _traceContext.Persons.Update(entity);
            await _traceContext.SaveChangesAsync();

            return updatedEntity.Entity;
        }
    }
}
