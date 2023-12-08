using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace CompanyEmployees
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(c => c.FullAddress,
                opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
            CreateMap<Market, MarketDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Vendor, VendorDto>();
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<MarketForCreationDto, Market>();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<VendorForCreationDto, Vendor>();
            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
            CreateMap<VendorForUpdateDto, Vendor>().ReverseMap();
            CreateMap<CompanyForUpdateDto, Company>();
            CreateMap<MarketForUpdateDto, Market>();
        }
    }
}
