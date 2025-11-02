namespace BeGamer.Services.Interfaces.common
{
    public interface IBaseAppService<TDto, TCreateDto, TUpdateDto>
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(Guid id);
        Task<TDto> CreateAsync(TCreateDto createDto);
        Task<TDto> UpdateAsync(Guid id, TUpdateDto updateDto);
        Task<bool> DeleteAsync(Guid id);
        bool ExistsById(Guid id);
    }
}
