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


namespace Business.Test
{
    public class UserManagerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IMapper _mapper;

        public UserManagerTest()
        {
            //Initial setup
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<UserProfile>();
                options.AddProfile<StatusProfile>();
                options.AddProfile<RoleProfile>();
                options.AddProfile<AddressProfile>();
            }));
        }

        [Fact]
        public async void CreateUser_CreateWithRegisterDTO_UserNotExistInRepo()
        {
            var users = UsersSample();
            RegisterRequestDTO register = new()
            {
                Username = "register",
                Email = "register@mail.com",
                Password = "3A7A4EE1E7AE7D0C9F95193571200B157A6BE0FFA59EDC8F84D39E4AD2D4D9FE:C433EF5F9226A6E7DE52883236AC00EF:301200:SHA256",
                StatusId = 1,
                RoleId = 5
            };
            _userRepositoryMock.Setup(x => x.CreateUser(It.IsAny<User>())).Callback<User>(users.Add);
            _userRepositoryMock.Setup(x => x.GetUser("register@mail.com")).ReturnsAsync(users.FirstOrDefault(user => user.Email.Equals("register@mail.com")));

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var boolResult = await _userManager.CreateUser(register);
            var countResult = users.Count;

            boolResult.Should().BeTrue();
            countResult.Should().Be(4);
        }

        [Fact]
        public async void CreateUser_NotCreateWithRegisterDTO_UserExistInRepo()
        {
            var users = UsersSample();
            RegisterRequestDTO register = new()
            {
                Username = "duplicate",
                Email = "mock2@mail.com",
                Password = "3A7A4EE1E7AE7D0C9F95193571200B157A6BE0FFA59EDC8F84D39E4AD2D4D9FE:C433EF5F9226A6E7DE52883236AC00EF:301200:SHA256",
                StatusId = 1,
                RoleId = 5
            };
            _userRepositoryMock.Setup(x => x.CreateUser(It.IsAny<User>())).Callback<User>(users.Add);
            _userRepositoryMock.Setup(x => x.GetUser("mock2@mail.com")).ReturnsAsync(users.FirstOrDefault(user => user.Email.Equals("mock2@mail.com")));

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var boolResult = await _userManager.CreateUser(register);
            var countResult = users.Count;

            boolResult.Should().BeFalse();
            countResult.Should().Be(3);
        }

        [Fact]
        public async void CreateUser_CreateWithCreateUserDTO_UserNotExistInRepo()
        {
            var users = UsersSample();
            CreateUserDTO register = new()
            {
                Username = "register",
                FirstName = "firstname",
                MiddleName = "middlename",
                LastName = "lastname",
                Email = "create@mail.com",
                Phone = "0904285035",
                Avatar = "avatar",
                Password = "3A7A4EE1E7AE7D0C9F95193571200B157A6BE0FFA59EDC8F84D39E4AD2D4D9FE:C433EF5F9226A6E7DE52883236AC00EF:301200:SHA256",
                StatusId = 1,
                RoleId = 5
            };
            _userRepositoryMock.Setup(x => x.CreateUser(It.IsAny<User>())).Callback<User>(users.Add);
            _userRepositoryMock.Setup(x => x.GetUser("create@mail.com")).ReturnsAsync(users.FirstOrDefault(user => user.Email.Equals("create@mail.com")));

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var boolResult = await _userManager.CreateUser(register);
            var countResult = users.Count;

            boolResult.Should().BeTrue();
            countResult.Should().Be(4);
        }

        [Fact]
        public async void CreateUser_NotCreateWithCreateUserDTO_UserExistInRepo()
        {
            var users = UsersSample();
            CreateUserDTO register = new()
            {
                Username = "register",
                FirstName = "firstname",
                MiddleName = "middlename",
                LastName = "lastname",
                Email = "mock3@mail.com",
                Phone = "0904285035",
                Avatar = "avatar",
                Password = "3A7A4EE1E7AE7D0C9F95193571200B157A6BE0FFA59EDC8F84D39E4AD2D4D9FE:C433EF5F9226A6E7DE52883236AC00EF:301200:SHA256",
                StatusId = 1,
                RoleId = 5
            };
            _userRepositoryMock.Setup(x => x.CreateUser(It.IsAny<User>())).Callback<User>(users.Add);
            _userRepositoryMock.Setup(x => x.GetUser("mock3@mail.com")).ReturnsAsync(users.FirstOrDefault(user => user.Email.Equals("mock3@mail.com")));

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var boolResult = await _userManager.CreateUser(register);
            var countResult = users.Count;

            boolResult.Should().BeFalse();
            countResult.Should().Be(3);
        }

        [Fact]
        public async void CreateResetToken_CreateToken_UserExistInRepo()
        {
            var users = UsersSample();
            ForgotPasswordDTO forgot = new()
            {
                Email = "mock3@mail.com",
                ResetToken = "mockToken"
            };
            _userRepositoryMock.Setup(x => x.GetUser("mock3@mail.com")).ReturnsAsync(users.FirstOrDefault(user => user.Email.Equals("mock3@mail.com")));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[2] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var boolResult = await _userManager.CreateResetToken(forgot);
            var updatedUser = users.FirstOrDefault(x => x.Email.Equals("mock3@mail.com"));

            boolResult.Should().BeTrue();
            updatedUser.Should().NotBeNull();
            updatedUser!.ResetToken.Should().BeSameAs(forgot.ResetToken);
            updatedUser!.ResetExpire.Should().BeAfter(DateTime.Now);
        }

        [Fact]
        public async void CreateResetToken_NotCreateToken_UserNotExistInRepo()
        {
            var users = UsersSample();
            ForgotPasswordDTO forgot = new()
            {
                Email = "notExist@mail.com",
                ResetToken = "mockToken"
            };
            _userRepositoryMock.Setup(x => x.GetUser("notExist@mail.com")).ReturnsAsync(users.FirstOrDefault(user => user.Email.Equals("notExist@mail.com")));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[2] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var boolResult = await _userManager.CreateResetToken(forgot);
            var updatedUser = users.FirstOrDefault(x => x.Email.Equals("notExist@mail.com"));

            boolResult.Should().BeFalse();
            updatedUser.Should().BeNull();
        }

        [Fact]
        public async void ForgetPassword_PasswordChange_UserExistInRepo()
        {
            var users = UsersSample();
            users[2].ResetToken = "mockToken";
            users[2].ResetExpire = DateTime.Now.AddHours(1);
            ForgotPasswordDTO forgot = new()
            {
                Email = "mock3@mail.com",
                Password = "newPassword",
                ResetToken = "mockToken"
            };
            _userRepositoryMock.Setup(x => x.GetUser("mock3@mail.com")).ReturnsAsync(users.FirstOrDefault(user => user.Email.Equals("mock3@mail.com")));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[2] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var intResult = await _userManager.ForgetPassword(forgot);
            var updatedUser = users.FirstOrDefault(x => x.Email.Equals("mock3@mail.com"));

            intResult.Should().Be(3);
            updatedUser.Should().NotBeNull();
            updatedUser!.ResetToken.Should().BeNull();
            updatedUser!.ResetExpire.Should().BeNull();
            updatedUser!.PasswordHash.Should().BeSameAs("newPassword");
        }

        [Fact]
        public async void ForgetPassword_PasswordNotChange_UserNotExistInRepo()
        {
            var users = UsersSample();
            users[2].ResetToken = "mockToken";
            users[2].ResetExpire = DateTime.Now.AddHours(1);
            ForgotPasswordDTO forgot = new()
            {
                Email = "notExist@mail.com",
                Password = "newPassword",
                ResetToken = "mockToken"
            };
            _userRepositoryMock.Setup(x => x.GetUser("notExist@mail.com")).ReturnsAsync(users.FirstOrDefault(user => user.Email.Equals("notExist@mail.com")));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[2] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var intResult = await _userManager.ForgetPassword(forgot);
            var updatedUser = users.FirstOrDefault(x => x.Email.Equals("notExist@mail.com"));

            intResult.Should().Be(0);
            updatedUser.Should().BeNull();
        }

        [Fact]
        public async void ForgetPassword_PasswordNotChange_WrongToken()
        {
            var users = UsersSample();
            users[2].ResetToken = "mockToken";
            users[2].ResetExpire = DateTime.Now.AddHours(1);
            ForgotPasswordDTO forgot = new()
            {
                Email = "mock3@mail.com",
                Password = "newPassword",
                ResetToken = "wrongToken"
            };
            _userRepositoryMock.Setup(x => x.GetUser("mock3@mail.com")).ReturnsAsync(users.FirstOrDefault(user => user.Email.Equals("mock3@mail.com")));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[2] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var intResult = await _userManager.ForgetPassword(forgot);
            var updatedUser = users.FirstOrDefault(x => x.Email.Equals("mock3@mail.com"));

            intResult.Should().Be(1);
            updatedUser.Should().NotBeNull();
        }

        [Fact]
        public async void ForgetPassword_PasswordChange_ExpireToken()
        {
            var users = UsersSample();
            users[2].ResetToken = "mockToken";
            users[2].ResetExpire = DateTime.Now.AddHours(-1);
            ForgotPasswordDTO forgot = new()
            {
                Email = "mock3@mail.com",
                Password = "newPassword",
                ResetToken = "mockToken"
            };
            _userRepositoryMock.Setup(x => x.GetUser("mock3@mail.com")).ReturnsAsync(users.FirstOrDefault(user => user.Email.Equals("mock3@mail.com")));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[2] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var intResult = await _userManager.ForgetPassword(forgot);
            var updatedUser = users.FirstOrDefault(x => x.Email.Equals("mock3@mail.com"));

            intResult.Should().Be(2);
            updatedUser.Should().NotBeNull();
        }

        [Fact]
        public async void ChangePassword_PasswordChange_UserExistInRepo()
        {
            var users = UsersSample();
            ChangePasswordDTO change = new()
            {
                UserId = 1,
                Password = "newPassword" 
            };
            _userRepositoryMock.Setup(x => x.GetUser(1)).ReturnsAsync(users.FirstOrDefault(x => x.UserId == 1));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[0] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var boolResult = await _userManager.ChangePassword(change);
            var updatedUser = users.FirstOrDefault(x => x.UserId == 1);

            boolResult.Should().BeTrue();
            updatedUser.Should().NotBeNull();
            updatedUser!.PasswordHash.Should().BeSameAs("newPassword");
        }

        [Fact]
        public async void ChangePassword_PasswordNotChange_UserNotExistInRepo()
        {
            var users = UsersSample();
            ChangePasswordDTO change = new()
            {
                UserId = 4,
                Password = "newPassword" 
            };
            _userRepositoryMock.Setup(x => x.GetUser(4)).ReturnsAsync(users.FirstOrDefault(x => x.UserId == 4));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[3] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var boolResult = await _userManager.ChangePassword(change);
            var updatedUser = users.FirstOrDefault(x => x.UserId == 4);

            boolResult.Should().BeFalse();
            updatedUser.Should().BeNull();
        }

        [Fact]
        public async void UpdateProfile_ProfileChange_UserExistInRepo()
        {
            var users = UsersSample();
            UserDTO profile = new()
            {
                UserId = 1,
                FirstName = "newFirstName",
                MiddleName = "newMiddleName",
                LastName = "newLastName",
                Email = "newMail@mail.com",
                Phone = "0000000000",
                Addresses = [
                    new AddressDTO() {
                        AddressId = 1,
                        AddressDetails = "mock address 1"
                    },
                    new AddressDTO() {
                        AddressId = 2,
                        AddressDetails = "mock address 2"
                    }
                ],
                Avatar = "newAvatarLink"
            };
            _userRepositoryMock.Setup(x => x.GetUser(1)).ReturnsAsync(users.FirstOrDefault(x => x.UserId == 1));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[0] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var boolResult = await _userManager.UpdateProfile(profile);
            var updatedUser = users.FirstOrDefault(x => x.UserId == 1);

            boolResult.Should().BeTrue();
            updatedUser.Should().NotBeNull();
            updatedUser!.FirstName.Should().BeSameAs(profile.FirstName);
            updatedUser!.MiddleName.Should().BeSameAs(profile.MiddleName);
            updatedUser!.LastName.Should().BeSameAs(profile.LastName);
            updatedUser!.Email.Should().BeSameAs(profile.Email);
            updatedUser!.Phone.Should().BeSameAs(profile.Phone);
            updatedUser!.Addresses.Should().BeEquivalentTo(profile.Addresses);
            updatedUser!.Avatar.Should().BeSameAs(profile.Avatar);
        }

        [Fact]
        public async void UpdateProfile_ProfileNotChange_UserNotExistInRepo()
        {
            var users = UsersSample();
            UserDTO profile = new()
            {
                UserId = 4,
                FirstName = "newFirstName",
                MiddleName = "newMiddleName",
                LastName = "newLastName",
                Email = "newMail@mail.com",
                Phone = "0000000000",
                Addresses = [
                    new AddressDTO() {
                        AddressId = 1,
                        AddressDetails = "mock address 1"
                    },
                    new AddressDTO() {
                        AddressId = 2,
                        AddressDetails = "mock address 2"
                    }
                ],
                Avatar = "newAvatarLink"
            };
            _userRepositoryMock.Setup(x => x.GetUser(4)).ReturnsAsync(users.FirstOrDefault(x => x.UserId == 4));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[3] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var boolResult = await _userManager.UpdateProfile(profile);
            var updatedUser = users.FirstOrDefault(x => x.UserId == 4);

            boolResult.Should().BeFalse();
            updatedUser.Should().BeNull();
        }

        [Fact]
        public async void GetUser_GetUserByEmail_UserExistInRepo()
        {
            var users = UsersSample();
            List<UserDTO> userDTOs = [];
            userDTOs.AddRange(users.Select(_mapper.Map<User, UserDTO>));
            _userRepositoryMock.Setup(x => x.GetUser("mock1@mail.com")).ReturnsAsync(users.FirstOrDefault(x => x.Email.Equals("mock1@mail.com")));
            
            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var result = await _userManager.GetUser("mock1@mail.com");
            var actual = userDTOs.FirstOrDefault(x => x.Email.Equals("mock1@mail.com"));

            result.Should()
                  .NotBeNull().And
                  .BeOfType<UserDTO>().And
                  .BeEquivalentTo(actual!);
        }

        [Fact]
        public async void GetUser_ReturnNull_UserNotExistInRepoEmail()
        {
            var users = UsersSample();
            List<UserDTO> userDTOs = [];
            userDTOs.AddRange(users.Select(_mapper.Map<User, UserDTO>));
            _userRepositoryMock.Setup(x => x.GetUser("notExist@mail.com")).ReturnsAsync(users.FirstOrDefault(x => x.Email.Equals("notExist@mail.com")));
            
            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var result = await _userManager.GetUser("notExist@mail.com");
            var actual = userDTOs.FirstOrDefault(x => x.Email.Equals("notExist@mail.com"));

            result.Should().BeNull();
            actual.Should().BeNull();
        }

        [Fact]
        public async void GetUser_ReturnNull_EmptyEmailString()
        {
            var users = UsersSample();
            List<UserDTO> userDTOs = [];
            userDTOs.AddRange(users.Select(_mapper.Map<User, UserDTO>));
            _userRepositoryMock.Setup(x => x.GetUser("")).ReturnsAsync(users.FirstOrDefault(x => x.Email.Equals("")));
            
            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var result = await _userManager.GetUser("");
            var actual = userDTOs.FirstOrDefault(x => x.Email.Equals(""));

            result.Should().BeNull();
            actual.Should().BeNull();
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetUser_GetUserById_UserExistInRepo()
        {
            //Arrange
            var users = UsersSample();
            List<UserDTO> userDTOs = [];
            userDTOs.AddRange(users.Select(_mapper.Map<User, UserDTO>));
            _userRepositoryMock.Setup(x => x.GetUser(1)).ReturnsAsync(users.Find(user => user.UserId == 1)); //Mock User repository GetUser(int id) method

            //Act
            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var result = await _userManager.GetUser(1);
            var actual = userDTOs.Find(user => user.UserId == 1);

            //Assert (using FluentAssertions)
            result.Should().NotBeNull().And.BeOfType<UserDTO>().And.BeEquivalentTo(actual!);
        }

        [Fact]
        public async void GetUser_ReturnNull_UserNotExistInRepoId()
        {
            var users = UsersSample();
            List<UserDTO> userDTOs = [];
            userDTOs.AddRange(users.Select(_mapper.Map<User, UserDTO>));
            _userRepositoryMock.Setup(x => x.GetUser(-1)).ReturnsAsync(users.Find(user => user.UserId == -1)); //Mock User repository GetUser(int id) method

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var result = await _userManager.GetUser(-1);
            var actual = userDTOs.FirstOrDefault(user => user.UserId == -1);

            result.Should().BeNull();
            actual.Should().BeNull();
        }

        [Fact]
        public async void UpdateRole_RoleChanged_UserExistInRepo()
        {
            var users = UsersSample();
            _userRepositoryMock.Setup(x => x.GetUser(1)).ReturnsAsync(users.Find(user => user.UserId == 1));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[0] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var boolResult = await _userManager.UpdateRole(1, 5);
            var actual = users.FirstOrDefault(user => user.UserId == 1);

            boolResult.Should().BeTrue();
            actual.Should().NotBeNull();
            actual!.RoleId.Should().Be(5);
        }

        [Fact]
        public async void UpdateRole_RoleNotChanged_UserNotExistInRepo()
        {
            var users = UsersSample();
            _userRepositoryMock.Setup(x => x.GetUser(-1)).ReturnsAsync(users.Find(user => user.UserId == -1));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[-1] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var boolResult = await _userManager.UpdateRole(-1, 5);
            var actual = users.FirstOrDefault(user => user.UserId == -1);

            boolResult.Should().BeFalse();
            actual.Should().BeNull();
        }

        [Fact]
        public async void UpdateStatus_StatusChanged_UserExistInRepo()
        {
            var users = UsersSample();
            _userRepositoryMock.Setup(x => x.GetUser(1)).ReturnsAsync(users.Find(user => user.UserId == 1));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[0] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var boolResult = await _userManager.UpdateStatus(1, 2);
            var actual = users.FirstOrDefault(user => user.UserId == 1);

            boolResult.Should().BeTrue();
            actual.Should().NotBeNull();
            actual!.StatusId.Should().Be(2);
        }

        [Fact]
        public async void UpdateStatus_StatusNotChanged_UserNotExistInRepo()
        {
            var users = UsersSample();
            _userRepositoryMock.Setup(x => x.GetUser(-1)).ReturnsAsync(users.Find(user => user.UserId == -1));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[-1] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var boolResult = await _userManager.UpdateStatus(-1, 2);
            var actual = users.FirstOrDefault(user => user.UserId == -1);

            boolResult.Should().BeFalse();
            actual.Should().BeNull();
        }

        [Fact]
        public async void Interact_InteractionPointIncrease_UserExistInRepo() 
        {
            var users = UsersSample();
            _userRepositoryMock.Setup(x => x.GetUser(1)).ReturnsAsync(users.Find(user => user.UserId == 1));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[0] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var intResult = await _userManager.Interact(1, "blog");
            var actual = users.FirstOrDefault(user => user.UserId == 1);

            intResult.Should().Be(2);
            actual.Should().NotBeNull();
            actual!.Interaction.Should().Be(3);
        }

        [Fact]
        public async void Interact_InteractionPointDecrease_UserExistInRepo() 
        {
            var users = UsersSample();
            users[0].Interaction = 29;
            _userRepositoryMock.Setup(x => x.GetUser(1)).ReturnsAsync(users.Find(user => user.UserId == 1));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[0] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var intResult = await _userManager.Interact(1, "blog");
            var actual = users.FirstOrDefault(user => user.UserId == 1);

            intResult.Should().Be(3);
            actual.Should().NotBeNull();
            actual!.Interaction.Should().Be(2);
        }

        [Fact]
        public async void Interact_Return0_UserNotExistInRepo() 
        {
            var users = UsersSample();
            _userRepositoryMock.Setup(x => x.GetUser(-1)).ReturnsAsync(users.Find(user => user.UserId == -1));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[-1] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var intResult = await _userManager.Interact(-1, "blog");
            var actual = users.FirstOrDefault(user => user.UserId == -1);

            intResult.Should().Be(0);
            actual.Should().BeNull();
        }

        [Fact]
        public async void Interact_Return1_WrongInteractionType() 
        {
            var users = UsersSample();
            _userRepositoryMock.Setup(x => x.GetUser(1)).ReturnsAsync(users.Find(user => user.UserId == 1));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[0] = user);

            IUserManager _userManager = new UserManager(_userRepositoryMock.Object, _mapper);
            var intResult = await _userManager.Interact(1, "wrongType");
            var actual = users.FirstOrDefault(user => user.UserId == 1);

            intResult.Should().Be(1);
            actual.Should().NotBeNull();
            actual!.Interaction.Should().Be(0);
        }

        private static List<User> UsersSample()
        {
            List<User> output = [
                new User() {
                    UserId = 1,
                    Username = "mock_1",
                    FirstName = "firstname_1",
                    MiddleName = "middlename_1",
                    LastName = "lastname_1",
                    Email = "mock1@mail.com",
                    Phone = "0904285035",
                    Avatar = "mock-avatar-link",
                    PasswordHash = "3A7A4EE1E7AE7D0C9F95193571200B157A6BE0FFA59EDC8F84D39E4AD2D4D9FE:C433EF5F9226A6E7DE52883236AC00EF:301200:SHA256",
                    StatusId = 1,
                    Status = new Status() {
                        StatusId = 1,
                        StatusName = "Active"
                    },
                    RoleId = 1,
                    Role = new Role() {
                        RoleId = 1,
                        RoleName = "Admin"
                    },
                    Interaction = 0
                },
                new User() {
                    UserId = 2,
                    Username = "mock_2",
                    FirstName = "firstname_2",
                    MiddleName = "middlename_2",
                    LastName = "lastname_2",
                    Email = "mock2@mail.com",
                    Phone = "0904285035",
                    Avatar = "mock-avatar-link",
                    PasswordHash = "3A7A4EE1E7AE7D0C9F95193571200B157A6BE0FFA59EDC8F84D39E4AD2D4D9FE:C433EF5F9226A6E7DE52883236AC00EF:301200:SHA256",
                    StatusId = 1,
                    Status = new Status() {
                        StatusId = 1,
                        StatusName = "Active"
                    },
                    RoleId = 1,
                    Role = new Role() {
                        RoleId = 1,
                        RoleName = "Admin"
                    },
                    Interaction = 0
                },
                new User() {
                    UserId = 3,
                    Username = "mock_3",
                    FirstName = "firstname_3",
                    MiddleName = "middlename_3",
                    LastName = "lastname_3",
                    Email = "mock3@mail.com",
                    Phone = "0904285035",
                    Avatar = "mock-avatar-link",
                    PasswordHash = "3A7A4EE1E7AE7D0C9F95193571200B157A6BE0FFA59EDC8F84D39E4AD2D4D9FE:C433EF5F9226A6E7DE52883236AC00EF:301200:SHA256",
                    StatusId = 1,
                    Status = new Status() {
                        StatusId = 1,
                        StatusName = "Active"
                    },
                    RoleId = 1,
                    Role = new Role() {
                        RoleId = 1,
                        RoleName = "Admin"
                    },
                    Interaction = 0
                },
                ];
            return output;
        }
    }
}