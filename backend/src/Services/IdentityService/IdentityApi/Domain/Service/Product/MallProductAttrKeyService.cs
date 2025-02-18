using Cloud.Mapster;
using Cloud.Models;
using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Cloud.Snowflake;
using Cloud.Utilities.Json;
using Domain.Entity.Product;
using Domain.IService.Identity;
using Domain.IService.Product;
using Domain.Service.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Identity.Shared.Dto.Admin.Output;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Domain.Service.Product
{
    /// <summary>
    /// 产品属性表key
    /// </summary>
    public class MallProductAttrKeyService(IListCacheRepository<MallProductAttrKey> listCacheMallProductAttrKeyRepository, IListCacheRepository<MallProductAttrValue> listCacheMallProductAttrValueRepository, ICloudUnitOfWork unitWork, IAuthenticationPrincipalService authenticationService, ISnowflakeIdWorker snowflakeIdWorker) : BaseListCacheService<MallProductAttrKey, MallProductAttrKeyDto>(listCacheMallProductAttrKeyRepository), IMallProductAttrKeyService
    {
        private readonly ICloudUnitOfWork _unitWork = unitWork;
        private readonly IListCacheRepository<MallProductAttrKey> _listCacheMallProductAttrKeyRepository = listCacheMallProductAttrKeyRepository;
        private readonly IListCacheRepository<MallProductAttrValue> _listCacheMallProductAttrValueRepository = listCacheMallProductAttrValueRepository;
        private readonly IAuthenticationPrincipalService _authenticationService = authenticationService;
        private readonly ISnowflakeIdWorker _snowflakeIdWorker= snowflakeIdWorker;

        public async Task<List<MallProductAttrKey>> GetListAsync()
        {
            return  await _listCacheMallProductAttrKeyRepository.GetListAsync();
        }
        public async Task<MallProductAttrKeyInfoOutput> FindSingleDetailsByIdAsync(IdQueryCommonInput input)
        {
            var mallProductAttrKeyDto = (await _listCacheMallProductAttrKeyRepository.GetSingleAsync(x => x.Id == input.Id)).MapTo<MallProductAttrKey, MallProductAttrKeyDto>();
            if (mallProductAttrKeyDto == default)
            {
                return default;
            }
            var mallProductAttrValueDtos = (await _listCacheMallProductAttrValueRepository.QueryAsync(x => x.MallProductAttrKeyId == input.Id)).OrderBy(x => x.SortNo).MapToList<MallProductAttrValue, MallProductAttrValueDto>();
            var MallProductAttrKeyInfoOutput = new MallProductAttrKeyInfoOutput
            {
                MallProductAttrKeyDto = mallProductAttrKeyDto,
                MallProductAttrValueDtos = mallProductAttrValueDtos
            };
            return MallProductAttrKeyInfoOutput;
        }

        public async Task<IEnumerable<MallProductAttrKeyInfoOutput>> GetAllDetailsListByCategoryIdAsync(IdQueryCommonInput input)
        {
            var mallProductAttrKeyDtos = (await _listCacheMallProductAttrKeyRepository.QueryAsync(x => x.MallProductCategoryId == input.Id)).OrderBy(x => x.SortNo).MapToList<MallProductAttrKey, MallProductAttrKeyDto>();
            if (mallProductAttrKeyDtos == default || mallProductAttrKeyDtos.Count == 0)
            {
                return null;
            }
            var MallProductAttrKeyInfoOutputs = new List<MallProductAttrKeyInfoOutput>();
            mallProductAttrKeyDtos.ForEach( async x =>
            {
                var mallProductAttrValueDtos =(await  _listCacheMallProductAttrValueRepository.QueryAsync(d => d.MallProductAttrKeyId == x.Id)).OrderBy(x => x.SortNo).MapToList<MallProductAttrValue, MallProductAttrValueDto>();
                var MallProductAttrKeyInfoOutput = new MallProductAttrKeyInfoOutput
                {
                    MallProductAttrKeyDto = x,
                    MallProductAttrValueDtos = mallProductAttrValueDtos
                };
                MallProductAttrKeyInfoOutputs.Add(MallProductAttrKeyInfoOutput);
            });
            return MallProductAttrKeyInfoOutputs;
        }

        public async Task AddOrUpdateAsync(MallProductAttrKeyInfoOutput input)
        {
            var mallProductAttrKeyEntity = input.MallProductAttrKeyDto.MapTo<MallProductAttrKey>();
            var mallProductAttrValueEntities = input.MallProductAttrValueDtos.MapToList<MallProductAttrValueDto, MallProductAttrValue>();
            if (mallProductAttrKeyEntity.MallProductTypeId == default)
            {
                throw new MyException("商品类型不能为空！", CommonConst.No);
            }
            if (mallProductAttrKeyEntity.MallProductCategoryId == default)
            {
                throw new MyException("所属目录不能为空！", CommonConst.No);
            }
            if (string.IsNullOrWhiteSpace(mallProductAttrKeyEntity.AttrKeyName))
            {
                throw new MyException("属性名称不能为空！", CommonConst.No);
            }
            if (mallProductAttrValueEntities == default || mallProductAttrValueEntities.Count == 0) throw new MyException("属性值不能为空，请至少新增一条属性值记录！", CommonConst.No);

            mallProductAttrKeyEntity.CreatedBy = await _authenticationService.GetAuthenticatedUserIdAsync();
            if (mallProductAttrKeyEntity.Id == default)
            {
                if (await _unitWork.ExistsAsync<MallProductAttrKey>(x => x.MallProductTypeId == mallProductAttrKeyEntity.MallProductTypeId && x.MallProductCategoryId == mallProductAttrKeyEntity.MallProductCategoryId && x.AttrKeyName == mallProductAttrKeyEntity.AttrKeyName))
                {
                    throw new MyException($"当前商品类型-所属目录下属性名称【{mallProductAttrKeyEntity.AttrKeyName}】已存在！", CommonConst.No);
                }
                _unitWork.Add(mallProductAttrKeyEntity);
                mallProductAttrValueEntities.ForEach(x =>
                {
                    x.Id = 0;
                    x.MallProductAttrKeyId = mallProductAttrKeyEntity.Id;
                });
                _unitWork.AddRange(mallProductAttrValueEntities);
            }
            else
            {
                if (await _unitWork.ExistsAsync<MallProductAttrKey>(x => x.MallProductTypeId == mallProductAttrKeyEntity.MallProductTypeId && x.MallProductCategoryId == mallProductAttrKeyEntity.MallProductCategoryId && x.AttrKeyName == mallProductAttrKeyEntity.AttrKeyName && x.Id != mallProductAttrKeyEntity.Id))
                {
                    throw new MyException($"当前商品类型-所属目录下属性名称【{mallProductAttrKeyEntity.AttrKeyName}】已存在！", CommonConst.No);
                }

                var oldMallProductAttrValueEntities = _unitWork.Query<MallProductAttrValue>(x => x.MallProductAttrKeyId == mallProductAttrKeyEntity.Id).ToList();

                #region 编辑的时候不能删
                if (oldMallProductAttrValueEntities != default)
                {
                    foreach (var item in oldMallProductAttrValueEntities)
                    {
                        if (!mallProductAttrValueEntities.Exists(x => x.Id == item.Id))
                        {
                            throw new MyException($"属性值【{item.AttrValueName}】不能删除！", CommonConst.No);
                        }
                    }
                }
                #endregion

                mallProductAttrValueEntities.ForEach(x =>
                {
                    if (x.Id > 0)
                    {
                        _unitWork.Update(x);
                    }
                    else
                    {
                        x.Id = _snowflakeIdWorker.NextId();
                        x.MallProductAttrKeyId = mallProductAttrKeyEntity.Id;
                        _unitWork.Add(x);
                    }

                });

                _unitWork.Update(mallProductAttrKeyEntity);
            }
            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<MallProductAttrKey>();
            await _unitWork.RemoveListCacheAsync<MallProductAttrValue>();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        public override async Task LogicDeleteAsync(IdQueryCommonInput input)
        {
            if (await _unitWork.ExistsAsync<MallProductAttr>(x => x.MallProductAttrKeyId == input.Id))
            {
                throw new MyException("已存在该属性的商品，无法删除！");
            }
            _unitWork.ExecuteDelete<MallProductAttrKey>(x => x.Id == input.Id);
            _unitWork.ExecuteDelete<MallProductAttrValue>(x => x.MallProductAttrKeyId == input.Id);
            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<MallProductAttrKey>();
            await _unitWork.RemoveListCacheAsync<MallProductAttrValue>();
        }

        public async Task<string> GetAttrNamesAsync(string attrValues, List<MallProductAttrKey> list = default)
        {
            if (string.IsNullOrWhiteSpace(attrValues)) return "";
            if (list == default) list =await _listCacheMallProductAttrKeyRepository.GetListAsync();

            var valueList =await _listCacheMallProductAttrValueRepository.GetListAsync();
            List<AdminMallProductAttrOutput> attrModelList = JsonUtility.Deserialize<List<AdminMallProductAttrOutput>>(attrValues);
            MallProductAttrKey entity = default;
            attrModelList.ForEach(item =>
            {
                entity = list.FirstOrDefault(x => x.Id == item.AttrKeyId);
                item.AttrKeyName = entity?.AttrKeyName;
                item.AttrValueName = valueList.FirstOrDefault(x => x.Id == item.AttrValueId)?.AttrValueName;
            });
            return JsonUtility.Serialize(attrModelList);
        }

        public async Task<string> GetAttrStringAsync(string attrValues, List<MallProductAttrKey> list = default)
        {
            if (string.IsNullOrWhiteSpace(attrValues)) return "";
            if (list == default) list =await  _listCacheMallProductAttrKeyRepository.GetListAsync();

            var valueList =await  _listCacheMallProductAttrValueRepository.GetListAsync();
            List<AdminMallProductAttrOutput> attrModelList = JsonUtility.Deserialize<List<AdminMallProductAttrOutput>>(attrValues);
            MallProductAttrKey entity = default;
            string attrNames = "";
            attrModelList.ForEach(item =>
            {
                entity = list.FirstOrDefault(x => x.Id == item.AttrKeyId);
                item.AttrKeyName = entity?.AttrKeyName;
                item.AttrValueName = valueList.FirstOrDefault(x => x.Id == item.AttrValueId)?.AttrValueName;
                attrNames += $"{item.AttrKeyName}:{item.AttrValueName}   ";
            });
            return attrNames;
        }
    }
}
