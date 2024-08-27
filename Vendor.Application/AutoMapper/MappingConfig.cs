using AutoMapper;
using Vendor.Application.DTOs.VendorDTO;
using Vendor.Application.Requests.Vendor;
using Vendor.Domain.Entities;

namespace Vendor.Application.AutoMapper
{
    public  class MappingConfig:Profile
    {

        public MappingConfig() {

            CreateMap<Domain.Entities.Vendor, VendorDto>()
           .ForMember(dest => dest.Markets,
               opt => opt.MapFrom(src => src.VendorMarket.Select(vm => vm.Market).ToList()));
            CreateMap<Domain.Entities.Vendor, VendorDataDto>()
                .ForMember(dest => dest.Markets,
                    opt => opt.MapFrom(src => src.VendorMarket.Select(vm => vm.Market).ToList()));
            CreateMap<AddVendorCommand, Domain.Entities.Vendor>();
            CreateMap<Domain.Entities.Vendor, CreateDto>();
            CreateMap<Domain.Entities.Vendor, MarketDto>();
            CreateMap<Service, ServiceDto>();
            CreateMap<Market, MarketDto>();
        }
    }
}
