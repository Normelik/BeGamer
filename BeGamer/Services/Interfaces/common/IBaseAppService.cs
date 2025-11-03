namespace BeGamer.Services.Interfaces.common
{
    public interface IBaseAppService<TEntity,TDto, TCreateDto, TUpdateDto>
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(Guid id);
        //Task<TDto> CreateAsync(TCreateDto createDto);
        //Task<TDto> UpdateAsync(Guid id, TUpdateDto updateDto);
        Task<bool> DeleteAsync(Guid id);
        //Task<bool> ExistsById(Guid id);
    }
}
