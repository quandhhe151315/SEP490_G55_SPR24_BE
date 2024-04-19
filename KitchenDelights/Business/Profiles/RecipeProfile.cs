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
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<RecipeIngredient, RecipeIngredientDTO>()
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient.IngredientName))
                .ForMember(dest => dest.IngredientUnit, opt => opt.MapFrom(src => src.Ingredient.IngredientUnit))
                .ForMember(dest => dest.IngredientMarketplaces, opt => opt.MapFrom(src => src.Ingredient.IngredientMarketplaces));
            CreateMap<RecipeIngredientDTO, RecipeIngredient>();

            CreateMap<RecipeRating, RecipeRatingDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => $"{src.User.LastName} {src.User.MiddleName} {src.User.FirstName}".Replace("  ", " ").Trim()))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar));
            CreateMap<RecipeRatingDTO, RecipeRating>();

            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CountryDTO, Country>();
            CreateMap<Country, CountryDTO>();
            CreateMap<Recipe, RecipeDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.LastName} {src.User.MiddleName} {src.User.FirstName}".Replace("  ", " ").Trim()))
                .ForMember(dest => dest.RecipeIngredients, opt => opt.MapFrom(src => src.RecipeIngredients))
                .ForMember(dest => dest.RecipeRatings, opt => opt.MapFrom(src => src.RecipeRatings.Where(x => x.RatingStatus != 0)));
            CreateMap<RecipeDTO, Recipe>();

            CreateMap<RecipeIngredientRequestDTO, RecipeIngredient>();
            CreateMap<RecipeIngredient, RecipeIngredientRequestDTO>();

            CreateMap<RecipeRequestDTO, Recipe>()
                .ForMember(dest => dest.RecipeIngredients, opt => opt.MapFrom(src => src.RecipeIngredients));
            CreateMap<Recipe, RecipeRequestDTO>();
        }
    }
}
