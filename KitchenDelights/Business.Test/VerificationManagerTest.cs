using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Managers;
using Business.Profiles;
using Data.Entity;
using Data.Interfaces;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace Business.Test;

public class VerificationManagerTest
{
    private Mock<IVerificationRepository> _mockVerificationRepository;
    private IMapper _mapper;

    public VerificationManagerTest() {
        _mockVerificationRepository = new Mock<IVerificationRepository>();
        _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<VerificationProfile>();
            }));
    }

    [Fact]
    public async void GetVerifications_GetVerificationList() {
        var verifications = GetVerifications();
        _mockVerificationRepository.Setup(x => x.GetVerifications()).ReturnsAsync(verifications.ToList());

        VerificationManager _verificationManager = new VerificationManager(_mockVerificationRepository.Object, _mapper);
        var result = await _verificationManager.GetVerifications();
        
        result.Should().BeOfType<List<VerificationDTO>>();
        result.Count.Should().Be(verifications.Count);
    }

    [Fact]
    public async void CreateVerification_CreateNewVerification_ValidUser() {
        var verifications = GetVerifications();
        VerificationDTO toAdd = new() {
            UserId = 1,
            CardFront = "card-front-image-link",
            CardBack = "card-back-image-link",
            VerificationFront = "verification-front-image-link",
            VerificationBack = "verification-back-image-link",
            VerificationStatus = 0
        };
        _mockVerificationRepository.Setup(x => x.CreateVerification(It.IsAny<Verification>())).Callback(verifications.Add);

        VerificationManager _verifcationManager = new VerificationManager(_mockVerificationRepository.Object, _mapper);
        var boolResult = await _verifcationManager.CreateVerification(toAdd);

        boolResult.Should().BeTrue();
        verifications.Count.Should().Be(4);
    }

    [Fact]
    public async void CreateVerification_NotCreateNewVerification_InvalidUser() {
        var verifications = GetVerifications();
        VerificationDTO toAdd = new() {
            UserId = -1,
            CardFront = "card-front-image-link",
            CardBack = "card-back-image-link",
            VerificationFront = "verification-front-image-link",
            VerificationBack = "verification-back-image-link",
            VerificationStatus = 0
        };
        _mockVerificationRepository.Setup(x => x.CreateVerification(It.IsAny<Verification>())).Throws(new Exception("Mock SQL Exception"));

        VerificationManager _verifcationManager = new VerificationManager(_mockVerificationRepository.Object, _mapper);
        var boolResult = await _verifcationManager.CreateVerification(toAdd);

        boolResult.Should().BeFalse();
        verifications.Count.Should().Be(3);
    }

    [Fact]
    public async void UpdateVerification_UpdateExistingVerification_Accept(){
        var verifications = GetVerifications();
        VerificationDTO toUpdate = new() {
            VerificationId = 3,
            VerificationStatus = 1
        };
        _mockVerificationRepository.Setup(x => x.GetVerification(toUpdate.VerificationId.Value)).ReturnsAsync(verifications.FirstOrDefault(verification => verification.VerificationId == toUpdate.VerificationId));
        _mockVerificationRepository.Setup(x => x.UpdateVerification(It.IsAny<Verification>())).Callback<Verification>(verification => verifications[verification.VerificationId - 1] = verification);

        VerificationManager _verificationManager = new VerificationManager(_mockVerificationRepository.Object, _mapper);
        var boolResult = await _verificationManager.UpdateVerification(toUpdate);
        var actual = verifications.FirstOrDefault(x => x.VerificationId == 3);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.VerificationStatus.Should().Be(1);
    }

    [Fact]
    public async void UpdateVerification_UpdateExistingVerification_Deny(){
        var verifications = GetVerifications();
        VerificationDTO toUpdate = new() {
            VerificationId = 3,
            VerificationStatus = 2
        };
        _mockVerificationRepository.Setup(x => x.GetVerification(toUpdate.VerificationId.Value)).ReturnsAsync(verifications.FirstOrDefault(verification => verification.VerificationId == toUpdate.VerificationId));
        _mockVerificationRepository.Setup(x => x.UpdateVerification(It.IsAny<Verification>())).Callback<Verification>(verification => verifications[verification.VerificationId - 1] = verification);

        VerificationManager _verificationManager = new VerificationManager(_mockVerificationRepository.Object, _mapper);
        var boolResult = await _verificationManager.UpdateVerification(toUpdate);
        var actual = verifications.FirstOrDefault(x => x.VerificationId == 3);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.VerificationStatus.Should().Be(2);
    }

    [Fact]
    public async void UpdateVerification_NotUpdateExistingVerification_VerificationNotExistInRepo(){
        var verifications = GetVerifications();
        VerificationDTO toUpdate = new() {
            VerificationId = -1,
            VerificationStatus = 1
        };
        _mockVerificationRepository.Setup(x => x.GetVerification(toUpdate.VerificationId.Value)).ReturnsAsync(verifications.FirstOrDefault(verification => verification.VerificationId == toUpdate.VerificationId));
        _mockVerificationRepository.Setup(x => x.UpdateVerification(It.IsAny<Verification>())).Callback<Verification>(verification => verifications[verification.VerificationId - 1] = verification);

        VerificationManager _verificationManager = new VerificationManager(_mockVerificationRepository.Object, _mapper);
        var boolResult = await _verificationManager.UpdateVerification(toUpdate);
        var actual = verifications.FirstOrDefault(x => x.VerificationId == -1);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    private List<Verification> GetVerifications() {
        List<Verification> output = [
            new Verification() {
                VerificationId = 1,
                UserId = 1,
                User = new() {
                    UserId = 1,
                    FirstName = "firstname",
                    MiddleName = "middlename",
                    LastName = "lastname", 
                },
                VerificationStatus = 1
            },
            new Verification() {
                VerificationId = 2,
                UserId = 2,
                User = new() {
                    UserId = 2,
                    FirstName = "firstname",
                    MiddleName = "middlename",
                    LastName = "lastname", 
                },
                VerificationStatus = 2
            },
            new Verification() {
                VerificationId = 3,
                UserId = 3,
                User = new() {
                    UserId = 3,
                    FirstName = "firstname",
                    MiddleName = "middlename",
                    LastName = "lastname", 
                },
                VerificationStatus = 0
            }
        ];
        return output;
    }
}
