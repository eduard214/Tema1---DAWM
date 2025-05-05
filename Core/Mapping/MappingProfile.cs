using AutoMapper;
using Core.Dtos.Requests.Customers;
using Core.Dtos.Requests.Orders;
using Core.Dtos.Responses.Customers;
using Core.Dtos.Responses.Orders;
using Database.Entities;

namespace Core.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddCustomerRequest, Customer>();
        CreateMap<AddOrderRequest, Order>();

        CreateMap<Customer, CustomerResponse>();
        CreateMap<Order, OrderResponse>();
    }
}