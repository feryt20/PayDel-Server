using System.Threading.Tasks;
using AutoMapper;
using PayDel.Data.DatabaseContext;
using PayDel.Presentation.Controllers.Site.Admin;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using PayDel.Repo.Infrastructures;
using XUTest.Moq;
using System.Linq.Expressions;
using System;
using PayDel.Data.Models;
using System.Linq;
using PayDel.Data.Dtos.Site.Admin;
using Microsoft.AspNetCore.Mvc;
using PayDel.Common.ErrorsAndMessages;

namespace XUTest.ControllerTest
{
    public class UsersControllerUnitTests
    {
        private readonly Mock<IUnitOfWork<PayDelDbContext>> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        //private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<UsersController>> _mockLogger;
        private readonly UsersController _controller;

        public UsersControllerUnitTests()
        {
            _mockRepo = new Mock<IUnitOfWork<PayDelDbContext>>();
            _mockMapper = new Mock<IMapper>();
            //_mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<UsersController>>();
            _controller = new UsersController(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
            //0d47394e-672f-4db7-898c-bfd8f32e2af7
            //haysmathis@barkarama.com
            //123789
        }

        #region GetUserTests
        [Fact]
        public async Task GetUser_Success_GetUserHimself()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            var users = UsersControllerData.GetUser();
            var userForDetailedDto = UsersControllerData.GetUserForDetailedDto();
            _mockRepo.Setup(x => x._UserRepository
                .GetAllAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                    It.IsAny<string>())).ReturnsAsync(() => users);
            //
            _mockMapper.Setup(x => x.Map<UserProfileDto>(It.IsAny<User>()))
                .Returns(userForDetailedDto);


            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.GetUsers(It.IsAny<string>());
            var okResult = result as OkObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(okResult);
            Assert.IsType<UserProfileDto>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async Task GetUser_Fail_GetAnOtherUser()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------


            //Act----------------------------------------------------------------------------------------------------------------------------------

            //Assert-------------------------------------------------------------------------------------------------------------------------------

        }

        #endregion

        #region UpdateUserTests
        [Fact]
        public async Task UpdateUser_Success_UpdateUserHimself()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            var users = UsersControllerData.GetUser();
            //var userForDetailedDto = UsersControllerMockData.GetUserForDetailedDto();
            _mockRepo.Setup(x => x._UserRepository
                .GetByIdAsync(
                    It.IsAny<string>())).ReturnsAsync(() => users.First());

            _mockRepo.Setup(x => x._UserRepository
                .Update(
                    It.IsAny<User>()));

            _mockRepo.Setup(x => x.SaveAcync()).ReturnsAsync(1);
            //
            _mockMapper.Setup(x => x.Map(It.IsAny<UserForUpdateDto>(), It.IsAny<User>()))
                .Returns(users.First());

            //Act----------------------------------------------------------------------------------------------------------------------------------

            var result = await _controller.UpdateUser(It.IsAny<string>(), It.IsAny<UserForUpdateDto>());
            var okResult = result as NoContentResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------

            Assert.NotNull(okResult);
            Assert.Equal(204, okResult.StatusCode);
        }
        [Fact]
        public async Task UpdateUser_Fail_UpdateAnOtherUser()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------
            var users = UsersControllerData.GetUser();
            //var userForDetailedDto = UsersControllerMockData.GetUserForDetailedDto();
            _mockRepo.Setup(x => x._UserRepository
                .GetByIdAsync(
                    It.IsAny<string>())).ReturnsAsync(() => users.First());

            _mockRepo.Setup(x => x._UserRepository
                .Update(
                    It.IsAny<User>()));

            _mockRepo.Setup(x => x.SaveAcync()).ReturnsAsync(0);
            //
            _mockMapper.Setup(x => x.Map(It.IsAny<UserForUpdateDto>(), It.IsAny<User>()))
                .Returns(users.First());
            //

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _controller.UpdateUser(It.IsAny<string>(), UsersControllerData.userForUpdateDto_Fail);
            var badResult = result as BadRequestObjectResult;
            //Assert-------------------------------------------------------------------------------------------------------------------------------

            Assert.NotNull(badResult);
            Assert.IsType<ReturnMessage>(badResult.Value);
            Assert.Equal(400, badResult.StatusCode);
        }
        [Fact]
        public async Task UpdateUser_Fail_ModelStateError()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------


            //Act----------------------------------------------------------------------------------------------------------------------------------

            //Assert-------------------------------------------------------------------------------------------------------------------------------


        }
        #endregion

        #region ChangeUserPasswordTests
        [Fact]
        public async Task ChangeUserPassword_Success_Himself()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------


            //Act----------------------------------------------------------------------------------------------------------------------------------

            //Assert-------------------------------------------------------------------------------------------------------------------------------

        }
        [Fact]
        public async Task ChangeUserPassword_Fail_AnOtherUser()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------


            //Act----------------------------------------------------------------------------------------------------------------------------------

            //Assert-------------------------------------------------------------------------------------------------------------------------------

        }
        [Fact]
        public async Task ChangeUserPassword_Fail_Himself_WrongOldPassword()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------


            //Act----------------------------------------------------------------------------------------------------------------------------------

            //Assert-------------------------------------------------------------------------------------------------------------------------------

        }
        [Fact]
        public async Task ChangeUserPassword_Fail_ModelStateError()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------


            //Act----------------------------------------------------------------------------------------------------------------------------------

            //Assert-------------------------------------------------------------------------------------------------------------------------------


        }
        #endregion
    }
}
