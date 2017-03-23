using AutoMapper;
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
        }
    }
}
