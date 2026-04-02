using AutoMapper;
using SSS.Application.Features.Payments.CreatePayment;
using SSS.Application.Features.Payments.GetUserPayments.Common;
using SSS.Domain.Entities.Payment;

namespace SSS.Application.Features.Payments.Common;

public sealed class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        // CreatePayment
        CreateMap<CreatePaymentCommand, UserPayment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(_ => "VND"))
            .ForMember(dest => dest.Amount, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => Domain.Enums.PaymentStatus.Pending))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<UserPayment, CreatePaymentDto>()
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => (int)src.Amount));

        // PaymentStatus
        CreateMap<UserPayment, PaymentStatusDto>()
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id));

        // GetUserPayments
        CreateMap<UserPayment, UserPaymentDto>()
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id));
    }
}
