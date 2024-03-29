using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Business.Profiles;
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
        public async void CreateUser_NotCreateWithCreateUserDTO_UserNotExistInRepo()
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