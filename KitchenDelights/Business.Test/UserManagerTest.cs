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