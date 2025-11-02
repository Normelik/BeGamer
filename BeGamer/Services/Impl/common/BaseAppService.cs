using BeGamer.Services.Interfaces.common;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Services.Impl.common
{
    public abstract class BaseAppService<TDto, TCreateDto, TUpdateDto> : IBaseAppService<TDto, TCreateDto, TUpdateDto>
    {
        protected readonly DbContext _context;
        protected readonly ILogger<BaseAppService<TDto, TCreateDto, TUpdateDto>> _logger;

        public BaseAppService(DbContext context, ILogger<BaseAppService<TDto, TCreateDto, TUpdateDto>> logger)
        {
            _context = context;
            _logger = logger;
        }


        public abstract Task<TDto> CreateAsync(TCreateDto createDto);
        public abstract Task<bool> DeleteAsync(Guid id);
        public abstract bool ExistsById(Guid id);
        public abstract Task<IEnumerable<TDto>> GetAllAsync();
        public abstract Task<TDto?> GetByIdAsync(Guid id);
        public abstract Task<TDto> UpdateAsync(Guid id, TUpdateDto updateDto);
    }
}
