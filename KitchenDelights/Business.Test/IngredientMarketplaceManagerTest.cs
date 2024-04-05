using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Business.Profiles;
using Data.Entity;
using Data.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Business.Test;

public class IngredientMarketplaceManagerTest
{
    private Mock<IIngredientMarketplaceRepository> _mockIngredientMarketplace;
    private IMapper _mapper;

    public IngredientMarketplaceManagerTest() {
        _mockIngredientMarketplace = new Mock<IIngredientMarketplaceRepository>();
        _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<MarketplaceProfile>();
            }));
    }

    [Fact]
    public async void GetIngredientMarketplaces_GetIngredientMarketplaceList() {
        var mocklist = GetIngredientMarketplace();
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplaces()).ReturnsAsync(mocklist.ToList());

        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var result = await _manager.GetIngredientMarketplaces();
        var actual = mocklist.Where(x => x.Marketplace.MarketplaceStatus == 1);

        result.Should().BeOfType<List<IngredientMarketplaceDTO>>();
        result.Count.Should().Be(actual.Count());
    }

    [Fact]
    public async void GetIngredientMarketplaces_GetIngredientMarketplaceList_ValidMarketplaceId() {
        var mocklist = GetIngredientMarketplace();
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplaces()).ReturnsAsync(mocklist.ToList());

        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var result = await _manager.GetIngredientMarketplaces(2);
        var actual = mocklist.Where(x => x.MarketplaceId == 2 && x.Marketplace.MarketplaceStatus == 1);

        result.Should().BeOfType<List<IngredientMarketplaceDTO>>();
        result.Count.Should().Be(actual.Count());
    }

    [Fact]
    public async void GetIngredientMarketplaces_GetIngredientMarketplaceList_BoundaryMarketplaceId() {
        var mocklist = GetIngredientMarketplace();
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplaces()).ReturnsAsync(mocklist.ToList());
        
        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var result = await _manager.GetIngredientMarketplaces(0);
        var actual = mocklist.Where(x => x.MarketplaceId == 0 && x.Marketplace.MarketplaceStatus == 1);

        result.Should().BeOfType<List<IngredientMarketplaceDTO>>();
        result.Count.Should().Be(actual.Count());
    }

    [Fact]
    public async void GetIngredientMarketplaces_GetIngredientMarketplaceList_AbnormalMarketplaceId() {
        var mocklist = GetIngredientMarketplace();
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplaces()).ReturnsAsync(mocklist.ToList());
        
        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var result = await _manager.GetIngredientMarketplaces(-1);
        var actual = mocklist.Where(x => x.MarketplaceId == -1 && x.Marketplace.MarketplaceStatus == 1);

        result.Should().BeOfType<List<IngredientMarketplaceDTO>>();
        result.Count.Should().Be(actual.Count());
    }

    [Fact]
    public async void CreateIngredientMarketplace_CreateNewIngredientMarketplace_AllValid() {
        var mocklist = GetIngredientMarketplace();
        IngredientMarketplaceDTO toAdd = new() {
            IngredientId = 3,
            MarketplaceId = 3,
            MarketplaceLink = "marketplace 3 link"
        };
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplace(toAdd.IngredientId.Value,
            toAdd.MarketplaceId.Value)).ReturnsAsync(mocklist.FirstOrDefault(x => x.IngredientId == toAdd.IngredientId && x.MarketplaceId == toAdd.MarketplaceId));
        _mockIngredientMarketplace.Setup(x => x.CreateIngredientMarketplace(It.IsAny<IngredientMarketplace>())).Callback(mocklist.Add);

        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var result = await _manager.CreateIngredientMarketplace(toAdd);

        result.Should().BeTrue();
        mocklist.Count.Should().Be(5);
    }

    [Fact]
    public async void CreateIngredientMarketplace_NotCreateNewIngredientMarketplace_InvalidIngredientId() {
        var mocklist = GetIngredientMarketplace();
        IngredientMarketplaceDTO toAdd = new() {
            IngredientId = -1,
            MarketplaceId = 3,
            MarketplaceLink = "marketplace 3 link"
        };
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplace(toAdd.IngredientId.Value,
            toAdd.MarketplaceId.Value)).ReturnsAsync(mocklist.FirstOrDefault(x => x.IngredientId == toAdd.IngredientId && x.MarketplaceId == toAdd.MarketplaceId));
        _mockIngredientMarketplace.Setup(x => x.CreateIngredientMarketplace(It.IsAny<IngredientMarketplace>())).Throws(new Exception());

        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var result = await _manager.CreateIngredientMarketplace(toAdd);

        result.Should().BeFalse();
        mocklist.Count.Should().Be(4);
    }

    [Fact]
    public async void CreateIngredientMarketplace_NotCreateNewIngredientMarketplace_InvalidMarketplaceId() {
        var mocklist = GetIngredientMarketplace();
        IngredientMarketplaceDTO toAdd = new() {
            IngredientId = 3,
            MarketplaceId = -1,
            MarketplaceLink = "marketplace 3 link"
        };
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplace(toAdd.IngredientId.Value,
            toAdd.MarketplaceId.Value)).ReturnsAsync(mocklist.FirstOrDefault(x => x.IngredientId == toAdd.IngredientId && x.MarketplaceId == toAdd.MarketplaceId));
        _mockIngredientMarketplace.Setup(x => x.CreateIngredientMarketplace(It.IsAny<IngredientMarketplace>())).Throws(new Exception());

        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var result = await _manager.CreateIngredientMarketplace(toAdd);

        result.Should().BeFalse();
        mocklist.Count.Should().Be(4);
    }

    [Fact]
    public async void CreateIngredientMarketplace_NotCreateNewIngredientMarketplace_AllInvalid() {
        var mocklist = GetIngredientMarketplace();
        IngredientMarketplaceDTO toAdd = new() {
            IngredientId = -1,
            MarketplaceId = -1,
            MarketplaceLink = "marketplace 3 link"
        };
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplace(toAdd.IngredientId.Value,
            toAdd.MarketplaceId.Value)).ReturnsAsync(mocklist.FirstOrDefault(x => x.IngredientId == toAdd.IngredientId && x.MarketplaceId == toAdd.MarketplaceId));
        _mockIngredientMarketplace.Setup(x => x.CreateIngredientMarketplace(It.IsAny<IngredientMarketplace>())).Throws(new Exception());

        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var result = await _manager.CreateIngredientMarketplace(toAdd);

        result.Should().BeFalse();
        mocklist.Count.Should().Be(4);
    }

    [Fact]
    public async void UpdateIngredientMarketplace_UpdateExistingIngredientMarketplace_AllValid() {
        var mocklist = GetIngredientMarketplace();
        IngredientMarketplaceDTO toUpdate = new() {
            IngredientId = 1,
            MarketplaceId = 1,
            MarketplaceLink = "new link"
        };
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplace(toUpdate.IngredientId.Value,
            toUpdate.MarketplaceId.Value)).ReturnsAsync(mocklist.FirstOrDefault(x => x.IngredientId == toUpdate.IngredientId && x.MarketplaceId == toUpdate.MarketplaceId));
        _mockIngredientMarketplace.Setup(x => x.UpdateIngredientMarketplace(It.IsAny<IngredientMarketplace>()))
                                        .Callback<IngredientMarketplace>(ingmarketplace => mocklist[0] = ingmarketplace);

        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var boolResult = await _manager.UpdateIngredientMarketplace(toUpdate);
        var actual = mocklist.FirstOrDefault(x => x.IngredientId == toUpdate.IngredientId && x.MarketplaceId == toUpdate.MarketplaceId);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.MarketplaceLink.Should().BeEquivalentTo(toUpdate.MarketplaceLink);
    }

    [Fact]
    public async void UpdateIngredientMarketplace_NotUpdateExistingIngredientMarketplace_InvalidIngredientId() {
        var mocklist = GetIngredientMarketplace();
        IngredientMarketplaceDTO toUpdate = new() {
            IngredientId = -1,
            MarketplaceId = 1,
            MarketplaceLink = "new link"
        };
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplace(toUpdate.IngredientId.Value,
            toUpdate.MarketplaceId.Value)).ReturnsAsync(mocklist.FirstOrDefault(x => x.IngredientId == toUpdate.IngredientId && x.MarketplaceId == toUpdate.MarketplaceId));
        _mockIngredientMarketplace.Setup(x => x.UpdateIngredientMarketplace(It.IsAny<IngredientMarketplace>()))
                                        .Throws(new Exception());

        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var boolResult = await _manager.UpdateIngredientMarketplace(toUpdate);
        var actual = mocklist.FirstOrDefault(x => x.IngredientId == toUpdate.IngredientId && x.MarketplaceId == toUpdate.MarketplaceId);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    [Fact]
    public async void UpdateIngredientMarketplace_NotUpdateExistingIngredientMarketplace_InvalidMarketplaceId() {
        var mocklist = GetIngredientMarketplace();
        IngredientMarketplaceDTO toUpdate = new() {
            IngredientId = 1,
            MarketplaceId = -1,
            MarketplaceLink = "new link"
        };
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplace(toUpdate.IngredientId.Value,
            toUpdate.MarketplaceId.Value)).ReturnsAsync(mocklist.FirstOrDefault(x => x.IngredientId == toUpdate.IngredientId && x.MarketplaceId == toUpdate.MarketplaceId));
        _mockIngredientMarketplace.Setup(x => x.UpdateIngredientMarketplace(It.IsAny<IngredientMarketplace>()))
                                        .Throws(new Exception());

        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var boolResult = await _manager.UpdateIngredientMarketplace(toUpdate);
        var actual = mocklist.FirstOrDefault(x => x.IngredientId == toUpdate.IngredientId && x.MarketplaceId == toUpdate.MarketplaceId);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    [Fact]
    public async void UpdateIngredientMarketplace_NotUpdateExistingIngredientMarketplace_AllInvalid() {
        var mocklist = GetIngredientMarketplace();
        IngredientMarketplaceDTO toUpdate = new() {
            IngredientId = -1,
            MarketplaceId = -1,
            MarketplaceLink = "new link"
        };
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplace(toUpdate.IngredientId.Value,
            toUpdate.MarketplaceId.Value)).ReturnsAsync(mocklist.FirstOrDefault(x => x.IngredientId == toUpdate.IngredientId && x.MarketplaceId == toUpdate.MarketplaceId));
        _mockIngredientMarketplace.Setup(x => x.UpdateIngredientMarketplace(It.IsAny<IngredientMarketplace>()))
                                        .Throws(new Exception());

        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var boolResult = await _manager.UpdateIngredientMarketplace(toUpdate);
        var actual = mocklist.FirstOrDefault(x => x.IngredientId == toUpdate.IngredientId && x.MarketplaceId == toUpdate.MarketplaceId);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    [Fact]
    public async void DeleteIngredientMarketplace_DeleteExistingIngredientMarketplace_ExistInRepo() {
        var mocklist = GetIngredientMarketplace();
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplace(1,1)).ReturnsAsync(mocklist.FirstOrDefault(x => x.IngredientId == 1 && x.MarketplaceId == 1));
        _mockIngredientMarketplace.Setup(x => x.DeleteIngredientMarketplace(It.IsAny<IngredientMarketplace>()))
                                  .Callback<IngredientMarketplace>(ing => mocklist.Remove(ing));

        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var boolResult = await _manager.DeleteIngredientMarketplace(1,1);

        boolResult.Should().BeTrue();
        mocklist.Count.Should().Be(3);
    }

    [Fact]
    public async void DeleteIngredientMarketplace_NotDeleteExistingIngredientMarketplace_InvalidIngredientId() {
        var mocklist = GetIngredientMarketplace();
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplace(-1,1)).ReturnsAsync(mocklist.FirstOrDefault(x => x.IngredientId == -1 && x.MarketplaceId == 1));
        _mockIngredientMarketplace.Setup(x => x.DeleteIngredientMarketplace(It.IsAny<IngredientMarketplace>()))
                                  .Callback<IngredientMarketplace>(ing => mocklist.Remove(ing));

        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var boolResult = await _manager.DeleteIngredientMarketplace(-1,1);

        boolResult.Should().BeFalse();
        mocklist.Count.Should().Be(4);
    }

    [Fact]
    public async void DeleteIngredientMarketplace_NotDeleteExistingIngredientMarketplace_InvalidMarketplaceId() {
        var mocklist = GetIngredientMarketplace();
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplace(1,-1)).ReturnsAsync(mocklist.FirstOrDefault(x => x.IngredientId == 1 && x.MarketplaceId == -1));
        _mockIngredientMarketplace.Setup(x => x.DeleteIngredientMarketplace(It.IsAny<IngredientMarketplace>()))
                                  .Callback<IngredientMarketplace>(ing => mocklist.Remove(ing));

        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var boolResult = await _manager.DeleteIngredientMarketplace(1,-1);

        boolResult.Should().BeFalse();
        mocklist.Count.Should().Be(4);
    }

    [Fact]
    public async void DeleteIngredientMarketplace_NotDeleteExistingIngredientMarketplace_AllInvalid() {
        var mocklist = GetIngredientMarketplace();
        _mockIngredientMarketplace.Setup(x => x.GetIngredientMarketplace(-1,-1)).ReturnsAsync(mocklist.FirstOrDefault(x => x.IngredientId == -1 && x.MarketplaceId == -1));
        _mockIngredientMarketplace.Setup(x => x.DeleteIngredientMarketplace(It.IsAny<IngredientMarketplace>()))
                                  .Callback<IngredientMarketplace>(ing => mocklist.Remove(ing));

        IIngredientMarketplaceManager _manager = new IngredientMarketplaceManager(_mockIngredientMarketplace.Object, _mapper);
        var boolResult = await _manager.DeleteIngredientMarketplace(-1,-1);

        boolResult.Should().BeFalse();
        mocklist.Count.Should().Be(4);
    }

    private List<IngredientMarketplace> GetIngredientMarketplace() {
        List<IngredientMarketplace> output = [
            new IngredientMarketplace() {
                IngredientId = 1,
                Ingredient = new Ingredient() {
                    IngredientId = 1,
                    IngredientName = "mock ingredient name"
                },
                MarketplaceId = 1,
                Marketplace = new Marketplace() {
                    MarketplaceId = 1,
                    MarketplaceLogo = "marketplace 1 logo",
                    MarketplaceStatus = 1
                },
                MarketplaceLink = "marketplace 1 ingredient link"
            },
            new IngredientMarketplace() {
                IngredientId = 1,
                Ingredient = new Ingredient() {
                    IngredientId = 1,
                    IngredientName = "mock ingredient name"
                },
                MarketplaceId = 2,
                Marketplace = new Marketplace() {
                    MarketplaceId = 2,
                    MarketplaceLogo = "marketplace 2 logo",
                    MarketplaceStatus = 1
                },
                MarketplaceLink = "marketplace 2 ingredient link"
            },
            new IngredientMarketplace() {
                IngredientId = 2,
                Ingredient = new Ingredient() {
                    IngredientId = 2,
                    IngredientName = "mock ingredient 2 name"
                },
                MarketplaceId = 1,
                Marketplace = new Marketplace() {
                    MarketplaceId = 1,
                    MarketplaceLogo = "marketplace 1 logo",
                    MarketplaceStatus = 1
                },
                MarketplaceLink = "marketplace 1 ingredient link"
            },
            new IngredientMarketplace() {
                IngredientId = 2,
                Ingredient = new Ingredient() {
                    IngredientId = 2,
                    IngredientName = "mock ingredient 2 name"
                },
                MarketplaceId = 2,
                Marketplace = new Marketplace() {
                    MarketplaceId = 2,
                    MarketplaceLogo = "marketplace 2 logo",
                    MarketplaceStatus = 1
                },
                MarketplaceLink = "marketplace 2 ingredient link"
            },
        ];
        return output;
    }
}
