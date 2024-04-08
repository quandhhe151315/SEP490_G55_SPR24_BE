using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Business.Profiles;
using Data.Entity;
using Data.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Test
{
    public class IngredientManagerTest
    {
        private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
        private readonly IMapper _mapper;

        public IngredientManagerTest()
        {
            //Initial setup
            _ingredientRepositoryMock = new Mock<IIngredientRepository>();
            _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<IngredientProfile>();
            }));
        }

        [Fact]
        public async void GetIngredient_GetIngredientList_ExistInRepo()
        {
            var ingredients = IngredientsSample();
            List<IngredientDTO> ingredientDTOs = [];
            ingredientDTOs.AddRange(ingredients.Select(_mapper.Map<Ingredient, IngredientDTO>));
            _ingredientRepositoryMock.Setup(x => x.GetAllIngredients()).ReturnsAsync(ingredients.ToList());

            IIngredientManager _ingredientManager = new IngredientManager(_ingredientRepositoryMock.Object, _mapper);
            var result = await _ingredientManager.GetAllIngredients();

            result.Should().BeOfType<List<IngredientDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(3);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetIngredient_GetIngredientById_IngredientExistInRepo()
        {
            //Arrange
            var ingredients = IngredientsSample();
            List<IngredientDTO> ingredientDTOs = [];
            ingredientDTOs.AddRange(ingredients.Select(_mapper.Map<Ingredient, IngredientDTO>));
            _ingredientRepositoryMock.Setup(x => x.GetIngredientById(1)).ReturnsAsync(ingredients.Find(x => x.IngredientId == 1)); //Mock Advertisement repository GetAdvertisementById(int id) method

            //Act
            IIngredientManager _ingredientManager = new IngredientManager(_ingredientRepositoryMock.Object, _mapper);
            var result = await _ingredientManager.GetIngredientById(1);
            var actual = ingredientDTOs.Find(x => x.IngredientId == 1);

            //Assert (using FluentAssertions)
            result.Should().NotBeNull().And.BeOfType<IngredientDTO>().And.BeEquivalentTo(actual!);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetIngredient_GetIngredientById_IngredientNotExistInRepo()
        {
            var ingredients = IngredientsSample();
            _ingredientRepositoryMock.Setup(x => x.GetIngredientById(-1)).ReturnsAsync(ingredients.FirstOrDefault(x => x.IngredientId == -1));

            IIngredientManager _ingredientManager = new IngredientManager(_ingredientRepositoryMock.Object, _mapper);
            var result = await _ingredientManager.GetIngredientById(-1);
            var actual = ingredients.FirstOrDefault(x => x.IngredientId == -1);

            result.Should().BeNull();
            actual.Should().BeNull();
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetIngredient_GetIngredientByName_IngredientExistInRepo()
        {
            //Arrange
            var ingredients = IngredientsSample();
            List<IngredientDTO> ingredientDTOs = [];
            ingredientDTOs.AddRange(ingredients.Select(_mapper.Map<Ingredient, IngredientDTO>));
            _ingredientRepositoryMock.Setup(x => x.GetIngredientByName("Thịt bò")).ReturnsAsync(ingredients.Where(x => x.IngredientName!.Equals("Thịt bò")).ToList()); //Mock Advertisement repository GetAdvertisementById(int id) method

            //Act
            IIngredientManager _ingredientManager = new IngredientManager(_ingredientRepositoryMock.Object, _mapper);
            var result = await _ingredientManager.GetIngredientsByName("Thịt bò");
            var actual = ingredientDTOs.Where(x => x.IngredientName!.Equals("Thịt bò")).ToList();

            //Assert (using FluentAssertions)
            result.Should().NotBeNull().And.BeOfType<List<IngredientDTO>>().And.BeEquivalentTo(actual!);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetIngredient_GetIngredientByName_IngredientNotExistInRepo()
        {
            //Arrange
            var ingredients = IngredientsSample();
            List<IngredientDTO> ingredientDTOs = [];
            ingredientDTOs.AddRange(ingredients.Select(_mapper.Map<Ingredient, IngredientDTO>));
            _ingredientRepositoryMock.Setup(x => x.GetIngredientByName("ABCDE")).ReturnsAsync(ingredients.Where(x => x.IngredientName!.Equals("ABCDE")).ToList()); //Mock Advertisement repository GetAdvertisementById(int id) method

            //Act
            IIngredientManager _ingredientManager = new IngredientManager(_ingredientRepositoryMock.Object, _mapper);
            var result = await _ingredientManager.GetIngredientsByName("ABCDE");
            var actual = ingredientDTOs.Where(x => x.IngredientName!.Equals("ABCDE")).ToList();

            result.Should().BeNullOrEmpty();
            actual.Should().BeNullOrEmpty();
        }

        [Fact]
        public async void CreateIngredient_CreateWithIngredientRequestDTO_IngredientNotExistInRepo()
        {
            var ingredients = IngredientsSample();
            IngredientRequestDTO ingredientDTO = new()
            {
                IngredientId = 4,
                IngredientName = "Thịt cừu",
                IngredientUnit = "g",
                IngredientStatus = 1
            };
            _ingredientRepositoryMock.Setup(x => x.CreateIngredient(It.IsAny<Ingredient>())).Callback<Ingredient>(ingredients.Add);
            _ingredientRepositoryMock.Setup(x => x.GetIngredientById(4)).ReturnsAsync(ingredients.FirstOrDefault(x => x.IngredientId == 4));

            IIngredientManager _ingredientManager = new IngredientManager(_ingredientRepositoryMock.Object, _mapper);
            var boolResult = await _ingredientManager.CreateIngredient(ingredientDTO);
            var countResult = ingredients.Count;

            boolResult.Should().BeTrue();
            countResult.Should().Be(4);
        }

        [Fact]
        public async void UpdateIngredient_UpdateIngredientIngredientExistInRepo()
        {
            var ingredients = IngredientsSample();
            IngredientRequestDTO ingredientDTO = new()
            {
                IngredientId = 1,
                IngredientName = "Thịt cừu",
                IngredientUnit = "kg",
                IngredientStatus = 1
            };
            _ingredientRepositoryMock.Setup(x => x.GetIngredientById(1)).ReturnsAsync(ingredients.FirstOrDefault(x => x.IngredientId == 1));
            _ingredientRepositoryMock.Setup(x => x.UpdateIngredient(It.IsAny<Ingredient>())).Callback<Ingredient>((ingredient) => ingredients[0] = ingredient);

            IIngredientManager _ingredientManager = new IngredientManager(_ingredientRepositoryMock.Object, _mapper);
            var boolResult = await _ingredientManager.UpdateIngredient(ingredientDTO);
            var updatedIngredient = ingredients.FirstOrDefault(x => x.IngredientId == 1);

            boolResult.Should().BeTrue();
            updatedIngredient.Should().NotBeNull();
            updatedIngredient!.IngredientName.Should().BeSameAs(ingredientDTO.IngredientName);
            updatedIngredient!.IngredientUnit.Should().BeSameAs(ingredientDTO.IngredientUnit);
        }

        [Fact]
        public async void UpdateIngredient_UpdateIngredientIngredientNotExistInRepo()
        {
            var ingredients = IngredientsSample();
            IngredientRequestDTO ingredientDTO = new()
            {
                IngredientId = 4,
                IngredientName = "Thịt cừu",
                IngredientUnit = "kg",
                IngredientStatus = 1
            };
            _ingredientRepositoryMock.Setup(x => x.GetIngredientById(4)).ReturnsAsync(ingredients.FirstOrDefault(x => x.IngredientId == 4));
            _ingredientRepositoryMock.Setup(x => x.UpdateIngredient(It.IsAny<Ingredient>())).Callback<Ingredient>((ingredient) => ingredients[0] = ingredient);

            IIngredientManager _ingredientManager = new IngredientManager(_ingredientRepositoryMock.Object, _mapper);
            var boolResult = await _ingredientManager.UpdateIngredient(ingredientDTO);
            var updatedIngredient = ingredients.FirstOrDefault(x => x.IngredientId == 4);

            boolResult.Should().BeFalse();
            updatedIngredient.Should().BeNull();
        }

        [Fact]
        public async void DeleteIngredient_DeleteIngredient_IngredientExistInRepo()
        {
            var ingredients = IngredientsSample();
            IngredientRequestDTO ingredientDTO = new()
            {
                IngredientId = 1,
                IngredientStatus = 0
            };
            _ingredientRepositoryMock.Setup(x => x.GetIngredientById(1)).ReturnsAsync(ingredients.FirstOrDefault(x => x.IngredientId == 1));
            _ingredientRepositoryMock.Setup(x => x.UpdateIngredient(It.IsAny<Ingredient>())).Callback<Ingredient>((ingredient) => ingredients[0] = ingredient);

            IIngredientManager _ingredientManager = new IngredientManager(_ingredientRepositoryMock.Object, _mapper);
            var boolResult = await _ingredientManager.DeleteIngredient(ingredientDTO.IngredientId);
            var updatedIngredient = ingredients.FirstOrDefault(x => x.IngredientId == 1);

            boolResult.Should().BeTrue();
            updatedIngredient.Should().NotBeNull();
            updatedIngredient!.IngredientStatus.ToString().Should().BeSameAs(ingredientDTO.IngredientStatus.ToString());
        }

        [Fact]
        public async void DeleteIngredient_DeleteIngredient_IngredientNotExistInRepo()
        {
            var ingredients = IngredientsSample();
            IngredientRequestDTO ingredientDTO = new()
            {
                IngredientId = 4,
                IngredientStatus = 1
            };
            _ingredientRepositoryMock.Setup(x => x.GetIngredientById(1)).ReturnsAsync(ingredients.FirstOrDefault(x => x.IngredientId == 1));
            _ingredientRepositoryMock.Setup(x => x.DeleteIngredient(It.IsAny<Ingredient>())).Callback<Ingredient>((ingredient) => ingredients[0] = ingredient);

            IIngredientManager _ingredientManager = new IngredientManager(_ingredientRepositoryMock.Object, _mapper);
            var boolResult = await _ingredientManager.DeleteIngredient(ingredientDTO.IngredientId);
            var updatedIngredient = ingredients.FirstOrDefault(x => x.IngredientId == 4);

            boolResult.Should().BeFalse();
            updatedIngredient.Should().BeNull();
        }

        private static List<Ingredient> IngredientsSample()
        {
            List<Ingredient> output = [
                new Ingredient()
                {
                    IngredientId = 1,
                    IngredientName = "Thịt bò",
                    IngredientUnit = "g",
                    IngredientStatus = 1
                },
                new Ingredient()
                {
                    IngredientId = 1,
                    IngredientName = "Thịt gà",
                    IngredientUnit = "g",
                    IngredientStatus = 1
                },
                new Ingredient()
                {
                    IngredientId = 1,
                    IngredientName = "Thịt lợn",
                    IngredientUnit = "g",
                    IngredientStatus = 1
                },
            ];
            return output;
        }
    }
}
