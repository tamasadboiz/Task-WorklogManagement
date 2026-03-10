using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_WorklogManagement.Application.Models;
using Task_WorklogManagement.Domain.DTOs;
using Task_WorklogManagement.Domain.Entities;
using Task_WorklogManagement.Domain.Models;

namespace Task_WorklogManagement.Application.AutoMappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserRegisterDTO>();

            CreateMap<UserRegisterDTO, User>()
                .ForMember(d => d.UserId, opt => opt.Ignore())
                .ForMember(d => d.PasswordHash, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
                .ForMember(d => d.RoleId, opt => opt.MapFrom(s => s.RoleId ?? 3));

            CreateMap<User, UserModel>()
                .ForMember(d => d.RoleName, opt => opt.MapFrom(s => s.Role != null ? s.Role.RoleName : "MEMBER"));

            CreateMap<User, AuthenticatedUserModel>()
                .ForMember(d => d.RoleName, opt => opt.MapFrom(s => s.Role != null ? s.Role.RoleName : "MEMBER"))
                .ForMember(d => d.AccessToken, opt => opt.Ignore())
                .ForMember(d => d.RefreshToken, opt => opt.Ignore());
        }
    }
}
