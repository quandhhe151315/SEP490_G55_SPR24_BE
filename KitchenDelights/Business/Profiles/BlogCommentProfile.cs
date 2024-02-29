using AutoMapper;
using Business.DTO;
using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Profiles
{
    public class BlogCommentProfile : Profile
    {
        public BlogCommentProfile() 
        {
            CreateMap<BlogComment, BlogCommentDTO>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.LastName} {src.User.MiddleName} {src.User.FirstName}".Replace("  ", " ").Trim()))
                                                    .ForMember(dest => dest.SubComments, opt => opt.MapFrom(src => src.InverseParent));
        }
    }
}
