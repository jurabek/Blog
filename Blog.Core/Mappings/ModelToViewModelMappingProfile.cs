﻿using AutoMapper;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Mappings
{
    public class ModelToViewModelMappingProfile : Profile
    {
        public ModelToViewModelMappingProfile()
        {
            CreateMap<User, ChangeProfileViewModel>();

            CreateMap<RegisterViewModel, User>()
                .ForMember(u => u.UserName, map => map.MapFrom(vm => vm.Email));

            CreateMap<User, UserViewModel>()
                .ForMember(vm => vm.Roles, opt => opt.MapFrom(u => u.Roles.Select(ur => ur.Role).Select(r => r.Name)))
                .ForMember(vm => vm.Permissions,
                                    opt => opt.MapFrom(u =>
                                            u.Roles.Select(ur => ur.Role)
                                                   .SelectMany(r => r.Permissions)
                                                   .Select(rp => rp.Permission)
                                                   .Select(p => p.Description)));
        }
    }
}
