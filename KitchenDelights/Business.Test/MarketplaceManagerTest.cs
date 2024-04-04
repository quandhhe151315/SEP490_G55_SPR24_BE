using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Business.Profiles;
using Castle.Components.DictionaryAdapter.Xml;
using Data.Entity;
using Data.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Business.Test;

public class MarketplaceManagerTest
{
    private Mock<IMarketplaceRepository> _mockMarketplaceRepository;
    private IMapper _mapper;

    public MarketplaceManagerTest() {
        _mockMarketplaceRepository = new Mock<IMarketplaceRepository>();
        _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<MarketplaceProfile>();
            }));
    }

    [Fact]
    public async void GetMarketplaces_GetMarketplaceList() {
        var marketplaces = GetMarketplaces();
        _mockMarketplaceRepository.Setup(x => x.GetMarketplaces()).ReturnsAsync(marketplaces.Where(x => x.MarketplaceStatus != 0).ToList());

        IMarketplaceManager _marketplaceManager = new MarketplaceManager(_mockMarketplaceRepository.Object, _mapper);
        var result = await _marketplaceManager.GetMarketplaces();
        
        result.Should().BeOfType<List<MarketplaceDTO>>();
        result.Count.Should().Be(marketplaces.Count);
    }

    [Fact]
    public void CreateMarketplace_CreateNewMarketplace() {
        var marketplaces = GetMarketplaces();
        MarketplaceDTO toAdd = new() {
            MarketplaceName = "Add Marketplace",
            MarketplaceLogo = "Add Marketplace logo",
            MarketplaceStatus = 1
        };
        _mockMarketplaceRepository.Setup(x => x.CreateMarketplace(It.IsAny<Marketplace>())).Callback(marketplaces.Add);

        IMarketplaceManager _marketplaceManager = new MarketplaceManager(_mockMarketplaceRepository.Object, _mapper);
        _marketplaceManager.CreateMarketplace(toAdd);

        marketplaces.Count.Should().Be(4);
        _mockMarketplaceRepository.Verify(x => x.CreateMarketplace(It.IsAny<Marketplace>()), Times.Once);
    }

    [Fact]
    public async void UpdateMarketplace_UpdateExistingMarketplace_AllValid() {
        var marketplaces = GetMarketplaces();
        MarketplaceDTO toUpdate = new() {
            MarketplaceId = 1,
            MarketplaceName = "new name",
            MarketplaceLogo = "new logo"
        };
        _mockMarketplaceRepository.Setup(x => x.GetMarketplace(toUpdate.MarketplaceId.Value)).ReturnsAsync(marketplaces.FirstOrDefault(x => x.MarketplaceId == toUpdate.MarketplaceId.Value));
        _mockMarketplaceRepository.Setup(x => x.UpdateMarketplace(It.IsAny<Marketplace>())).Callback<Marketplace>(marketplace => marketplaces[marketplace.MarketplaceId - 1] = marketplace);

        IMarketplaceManager _marketplaceManager = new MarketplaceManager(_mockMarketplaceRepository.Object, _mapper);
        var boolResult = await _marketplaceManager.UpdateMarketplace(toUpdate);
        var actual = marketplaces.FirstOrDefault(x => x.MarketplaceId == 1);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.MarketplaceName.Should().BeEquivalentTo(toUpdate.MarketplaceName);
        actual!.MarketplaceLogo.Should().BeEquivalentTo(toUpdate.MarketplaceLogo);
    }

    [Fact]
    public async void UpdateMarketplace_NotUpdateExistingMarketplace_BoundaryMarketplaceId() {
        var marketplaces = GetMarketplaces();
        MarketplaceDTO toUpdate = new() {
            MarketplaceId = 0,
            MarketplaceName = "new name",
            MarketplaceLogo = "new logo"
        };
        _mockMarketplaceRepository.Setup(x => x.GetMarketplace(toUpdate.MarketplaceId.Value)).ReturnsAsync(marketplaces.FirstOrDefault(x => x.MarketplaceId == toUpdate.MarketplaceId.Value));
        _mockMarketplaceRepository.Setup(x => x.UpdateMarketplace(It.IsAny<Marketplace>())).Callback<Marketplace>(marketplace => marketplaces[marketplace.MarketplaceId - 1] = marketplace);

        IMarketplaceManager _marketplaceManager = new MarketplaceManager(_mockMarketplaceRepository.Object, _mapper);
        var boolResult = await _marketplaceManager.UpdateMarketplace(toUpdate);
        var actual = marketplaces.FirstOrDefault(x => x.MarketplaceId == 0);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    [Fact]
    public async void UpdateMarketplace_NotUpdateExistingMarketplace_InvalidMarketplaceId() {
        var marketplaces = GetMarketplaces();
        MarketplaceDTO toUpdate = new() {
            MarketplaceId = -1,
            MarketplaceName = "new name",
            MarketplaceLogo = "new logo"
        };
        _mockMarketplaceRepository.Setup(x => x.GetMarketplace(toUpdate.MarketplaceId.Value)).ReturnsAsync(marketplaces.FirstOrDefault(x => x.MarketplaceId == toUpdate.MarketplaceId.Value));
        _mockMarketplaceRepository.Setup(x => x.UpdateMarketplace(It.IsAny<Marketplace>())).Callback<Marketplace>(marketplace => marketplaces[marketplace.MarketplaceId - 1] = marketplace);

        IMarketplaceManager _marketplaceManager = new MarketplaceManager(_mockMarketplaceRepository.Object, _mapper);
        var boolResult = await _marketplaceManager.UpdateMarketplace(toUpdate);
        var actual = marketplaces.FirstOrDefault(x => x.MarketplaceId == -1);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    [Fact]
    public async void UpdateStatus_UpdateExistingMarketplaceStatus_ActiveMarketplace() {
        var marketplaces = GetMarketplaces();
        _mockMarketplaceRepository.Setup(x => x.GetMarketplace(1)).ReturnsAsync(marketplaces.FirstOrDefault(x => x.MarketplaceId == 1));
        _mockMarketplaceRepository.Setup(x => x.UpdateMarketplace(It.IsAny<Marketplace>())).Callback<Marketplace>(marketplace => marketplaces[marketplace.MarketplaceId - 1] = marketplace);

        IMarketplaceManager _marketplaceManager = new MarketplaceManager(_mockMarketplaceRepository.Object, _mapper);
        var boolResult = await _marketplaceManager.UpdateStatus(1);
        var actual = marketplaces.FirstOrDefault(x => x.MarketplaceId == 1);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.MarketplaceStatus.Should().Be(2);
    }

    [Fact]
    public async void UpdateStatus_UpdateExistingMarketplaceStatus_InactiveMarketplace() {
        var marketplaces = GetMarketplaces();
        _mockMarketplaceRepository.Setup(x => x.GetMarketplace(2)).ReturnsAsync(marketplaces.FirstOrDefault(x => x.MarketplaceId == 2));
        _mockMarketplaceRepository.Setup(x => x.UpdateMarketplace(It.IsAny<Marketplace>())).Callback<Marketplace>(marketplace => marketplaces[marketplace.MarketplaceId - 1] = marketplace);

        IMarketplaceManager _marketplaceManager = new MarketplaceManager(_mockMarketplaceRepository.Object, _mapper);
        var boolResult = await _marketplaceManager.UpdateStatus(2);
        var actual = marketplaces.FirstOrDefault(x => x.MarketplaceId == 2);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.MarketplaceStatus.Should().Be(1);
    }

    [Fact]
    public async void UpdateStatus_UpdateExistingMarketplaceStatus_InvalidMarketplaceId() {
        var marketplaces = GetMarketplaces();
        _mockMarketplaceRepository.Setup(x => x.GetMarketplace(-1)).ReturnsAsync(marketplaces.FirstOrDefault(x => x.MarketplaceId == -1));
        _mockMarketplaceRepository.Setup(x => x.UpdateMarketplace(It.IsAny<Marketplace>())).Callback<Marketplace>(marketplace => marketplaces[marketplace.MarketplaceId - 1] = marketplace);

        IMarketplaceManager _marketplaceManager = new MarketplaceManager(_mockMarketplaceRepository.Object, _mapper);
        var boolResult = await _marketplaceManager.UpdateStatus(-1);
        var actual = marketplaces.FirstOrDefault(x => x.MarketplaceId == -1);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    [Fact]
    public async void DeleteMarketplace_DeleteExistingMarketplace_ValidMarketplaceId() {
        var marketplaces = GetMarketplaces();
        _mockMarketplaceRepository.Setup(x => x.GetMarketplace(1)).ReturnsAsync(marketplaces.FirstOrDefault(x => x.MarketplaceId == 1));
        _mockMarketplaceRepository.Setup(x => x.UpdateMarketplace(It.IsAny<Marketplace>())).Callback<Marketplace>(marketplace => marketplaces[marketplace.MarketplaceId - 1] = marketplace);

        IMarketplaceManager _marketplaceManager = new MarketplaceManager(_mockMarketplaceRepository.Object, _mapper);
        var boolResult = await _marketplaceManager.DeleteMarketplace(1);
        var actual = marketplaces.FirstOrDefault(x => x.MarketplaceId == 1);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.MarketplaceStatus.Should().Be(0);
    }

    [Fact]
    public async void DeleteMarketplace_NotDeleteExistingMarketplace_BoundaryMarketplaceId() {
        var marketplaces = GetMarketplaces();
        _mockMarketplaceRepository.Setup(x => x.GetMarketplace(0)).ReturnsAsync(marketplaces.FirstOrDefault(x => x.MarketplaceId == 0));
        _mockMarketplaceRepository.Setup(x => x.UpdateMarketplace(It.IsAny<Marketplace>())).Callback<Marketplace>(marketplace => marketplaces[marketplace.MarketplaceId - 1] = marketplace);

        IMarketplaceManager _marketplaceManager = new MarketplaceManager(_mockMarketplaceRepository.Object, _mapper);
        var boolResult = await _marketplaceManager.DeleteMarketplace(0);
        var actual = marketplaces.FirstOrDefault(x => x.MarketplaceId == 0);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    [Fact]
    public async void DeleteMarketplace_NotDeleteExistingMarketplace_AbnormalMarketplaceId() {
        var marketplaces = GetMarketplaces();
        _mockMarketplaceRepository.Setup(x => x.GetMarketplace(-1)).ReturnsAsync(marketplaces.FirstOrDefault(x => x.MarketplaceId == -1));
        _mockMarketplaceRepository.Setup(x => x.UpdateMarketplace(It.IsAny<Marketplace>())).Callback<Marketplace>(marketplace => marketplaces[marketplace.MarketplaceId - 1] = marketplace);

        IMarketplaceManager _marketplaceManager = new MarketplaceManager(_mockMarketplaceRepository.Object, _mapper);
        var boolResult = await _marketplaceManager.DeleteMarketplace(-1);
        var actual = marketplaces.FirstOrDefault(x => x.MarketplaceId == -1);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    private List<Marketplace> GetMarketplaces() {
        List<Marketplace> output = [
            new Marketplace() {
                MarketplaceId = 1,
                MarketplaceName = "Mock Marketplace 1",
                MarketplaceLogo = "Marketplace logo 1",
                MarketplaceStatus = 1,
            },
            new Marketplace() {
                MarketplaceId = 2,
                MarketplaceName = "Mock Marketplace 2",
                MarketplaceLogo = "Marketplace logo 2",
                MarketplaceStatus = 2,
            },
            new Marketplace() {
                MarketplaceId = 3,
                MarketplaceName = "Mock Marketplace 3",
                MarketplaceLogo = "Marketplace logo 3",
                MarketplaceStatus = 1,
            },
        ];
        return output;
    }
}
