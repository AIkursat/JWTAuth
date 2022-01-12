using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class GenericService<TEntity, TDto> : IServiceGeneric<TEntity, TDto> where TDto : class where TEntity : class

    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;

        }
        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
           
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
            await _genericRepository.AddAsync(newEntity);

            await _unitOfWork.CommitAsync(); // In order to reflect that to Db.

            var NewDto = ObjectMapper.Mapper.Map<TDto>(newEntity);

            return Response<TDto>.Success(NewDto, 200);

        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            
            var products = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());

            return Response<IEnumerable<TDto>>.Success(products, 200);
             

        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var product = await _genericRepository.GetByIdAsync(id);

            if(product == null)
            {
                return Response<TDto>.Fail("Id Not Found", 404, true);
            }
           return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(product), 200);

        }

        public async Task<Response<NoDataDto>> Remove(int id)
        {
            var IsExistEntity = await _genericRepository.GetByIdAsync(id);

            if(IsExistEntity == null)
            {
                return Response<NoDataDto>.Fail("Id Not Found", 404, true);
            }
            _genericRepository.Remove(IsExistEntity);

            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(204); // 204 = No Content
        }

        public async Task<Response<NoDataDto>> Update(TDto entity, int id)
        {
            var IsExistEntity = await _genericRepository.GetByIdAsync(id);
            if(IsExistEntity == null)
            {
                return Response<NoDataDto>.Fail("Id Not Found", 404, true);
            }

            var UpdateEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
            _genericRepository.Update(UpdateEntity);

            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(204);

        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var list = _genericRepository.Where(predicate);

            return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()), 200);


        }
    }
}
