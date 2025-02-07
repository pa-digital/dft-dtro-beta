namespace DfT.DTRO.Repositories.Implementation;

/// <inheritdoc cref="IRepository{T}"/>
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DtroContext _context;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="context">Database context</param>
    public Repository(DtroContext context) => _context = context;

    /// <inheritdoc cref="IRepository{T}"/>
    public async Task<T> CreateAsync(T entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc cref="IRepository{T}"/>
    public async Task<IEnumerable<T>> ReadAsync() => await _context.Set<T>().ToListAsync();

    /// <inheritdoc cref="IRepository{T}"/>
    public async Task<T> ReadAsync(Guid id) => await _context.Set<T>().FindAsync(id);

    /// <inheritdoc cref="IRepository{T}"/>
    public async Task<T> UpdateAsync(T entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc cref="IRepository{T}"/>
    public async Task<bool> DeleteAsync(T entity)
    {
        var existingEntity = await ReadAsync(entity.Id);
        if (existingEntity == null)
        {
            return false;
        }

        existingEntity.Status = "inactive";
        await _context.SaveChangesAsync();
        return true;
    }
}