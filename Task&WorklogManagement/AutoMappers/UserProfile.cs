using AutoMapper;
using System;
using Task_WorklogManagement.Domains.DTOs;
using Task_WorklogManagement.Domains.Entities;
using Task_WorklogManagement.Domains.Models;
using Task_WorklogManagement.Models;

namespace Task_WorklogManagement.AutoMappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, User>()
                .ForMember(d => d.UserId, opt => opt.Ignore())
                .ForMember(d => d.PasswordHash, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());

            CreateMap<User, UserModel>()
                .ForMember(d => d.RoleName, opt => opt.MapFrom(s => s.Role != null ? s.Role.RoleName : "MEMBER"));

            CreateMap<User, AuthenticatedUserModel>()
                .ForMember(d => d.RoleName, opt => opt.MapFrom(s => s.Role != null ? s.Role.RoleName : "MEMBER"))
                .ForMember(d => d.AccessToken, opt => opt.Ignore())
                .ForMember(d => d.RefreshToken, opt => opt.Ignore());
        }
    }
}
