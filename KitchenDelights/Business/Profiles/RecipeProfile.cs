﻿using AutoMapper;
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
                .ForMember(dest => dest.IngredientUnit, opt => opt.MapFrom(src => src.Ingredient.IngredientUnit));
            CreateMap<RecipeIngredientDTO, RecipeIngredient>();
            CreateMap<RecipeRating, RecipeRatingDTO>();
            CreateMap<RecipeRatingDTO, RecipeRating>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CountryDTO, Country>();
            CreateMap<Country, CountryDTO>();
            CreateMap<Recipe, RecipeDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.MiddleName} {src.User.LastName}".Replace("  ", " ").Trim()))
                .ForMember(dest => dest.RecipeIngredients, opt => opt.MapFrom(src => src.RecipeIngredients))
                .ForMember(dest => dest.RecipeRatings, opt => opt.MapFrom(src => src.RecipeRatings))
                ;
            CreateMap<RecipeDTO, Recipe>();
        }
    }
}