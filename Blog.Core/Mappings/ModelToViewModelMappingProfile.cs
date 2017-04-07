using AutoMapper;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using System.Linq;

namespace Blog.Core.Mappings
{
    public class ModelToViewModelMappingProfile : Profile
    {
        public ModelToViewModelMappingProfile()
        {
            CreateMap<User, UpdateProfileViewModel>();

            CreateMap<RegisterViewModel, User>()
                .ForMember(u => u.UserName, map => map.MapFrom(vm => vm.Email));

            CreateMap<User, UsersViewModel>()
                .ForMember(vm => vm.Roles, opt => opt.MapFrom(u => u.Roles.Select(ur => ur.Role)))
                .ForMember(vm => vm.Permissions,
                                    opt => opt.MapFrom(u =>
                                            u.Roles.Select(ur => ur.Role)
                                                   .SelectMany(r => r.Permissions)
                                                   .Select(rp => rp.Permission)));


            CreateMap<RoleViewModel, IdentityRole>();
            CreateMap<IdentityRole, RoleViewModel>();

            CreateMap<ArticleViewModel, Article>()
                .ForMember(m => m.PublishedDate, vm => vm.MapFrom(x => x.DateTime))
                .ForMember(m => m.PictureUrl, vm => vm.MapFrom(x => x.Image));

            CreateMap<Article, ArticleViewModel>()
                .ForMember(vm => vm.ShortBody, m => m.MapFrom(x => x.Body.Split('.').Take(5)))
                .ForMember(vm => vm.Image, m => m.MapFrom(x => "..//..//Images//Blog//" + x.PictureUrl))
                .ForMember(vm => vm.Author, m => m.MapFrom(x => x.User));
        }
    }
}
