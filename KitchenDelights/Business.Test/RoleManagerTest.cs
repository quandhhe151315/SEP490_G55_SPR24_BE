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

public class RoleManagerTest
{
    private Mock<IRoleRepository> _mockRoleRepository;
    private IMapper _mapper;

    public RoleManagerTest() {
        _mockRoleRepository = new Mock<IRoleRepository>();
        _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<RoleProfile>();
            }));
    }

    [Fact]
    public async void GetRoles_GetRoleList() {
        var roles = GetRoles();
        _mockRoleRepository.Setup(x => x.GetRoles()).ReturnsAsync(roles);

        IRoleManager _roleManager = new RoleManager(_mockRoleRepository.Object, _mapper);
        var result = await _roleManager.GetRoles();

        result.Should().BeOfType<List<RoleDTO>>();
        result.Count.Should().Be(roles.Count);
    }

    private List<Role> GetRoles() {
        List<Role> output = [
            new Role() {
                RoleId = 1,
                RoleName = "Administrator"
            },
            new Role() {
                RoleId = 2,
                RoleName = "Moderator"
            },
            new Role() {
                RoleId = 3,
                RoleName = "Writer"
            },
        ];
        return output;
    }
}
