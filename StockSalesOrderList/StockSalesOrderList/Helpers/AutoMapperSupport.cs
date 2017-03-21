using AutoMapper;
using StockSalesOrderList.Models;
using StockSalesOrderList.Models.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Helpers
{
    public class AutoMapperSupport
    {
        public static void Configure()
        {
            try
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.AddProfile<UserApiModelMappingProfile>();
                    cfg.AddProfile<RoleApiModelMappingProfile>();
                    cfg.AddProfile<MakerApiModelMappingProfile>();
                    cfg.AddProfile<ProductApiModelMappingProfile>();
                    cfg.AddProfile<GroupApiModelMappingProfile>();
                    cfg.AddProfile<ContainerApiModelMappingProfile>();
                });
                Mapper.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }


    #region WebAPIマッピング
    public class UserApiModelMappingProfile : Profile
    {
        public UserApiModelMappingProfile()
        {
            CreateMap<UserModel, UserApiModel>()
                .ForMember(d => d.NewExpiration, o => o.Ignore())
                .ForMember(d => d.NewPassword, o => o.Ignore());
        }
    }

    public class RoleApiModelMappingProfile : Profile
    {
        public RoleApiModelMappingProfile()
        {
            CreateMap<RoleModel, RoleApiModel>();
        }
    }

    public class MakerApiModelMappingProfile : Profile
    {
        public MakerApiModelMappingProfile()
        {
            CreateMap<MakerModel, MakerApiModel>();
        }
    }

    public class ProductApiModelMappingProfile : Profile
    {
        public ProductApiModelMappingProfile()
        {
            CreateMap<ProductModel, ProductApiModel>()
                .ForMember(d => d.MakerCode, o => o.MapFrom(s => s.MakerModel.Code))
                .ForMember(d => d.MakerName, o => o.MapFrom(s => s.MakerModel.Name));
        }
    }

    public class GroupApiModelMappingProfile : Profile
    {
        public GroupApiModelMappingProfile()
        {
            CreateMap<GroupModel, GroupApiModel>()
                .ForMember(d => d.MakerCode, o => o.MapFrom(s => s.MakerModel.Code))
                .ForMember(d => d.MakerName, o => o.MapFrom(s => s.MakerModel.Name))
                .ForMember(d => d.ContainerName, o => o.MapFrom(s => s.ContainerModel.Name));
        }
    }

    public class ContainerApiModelMappingProfile : Profile
    {
        public ContainerApiModelMappingProfile()
        {
            CreateMap<ContainerModel, ContainerApiModel>();
        }
    }
    #endregion
}
