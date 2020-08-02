using FluentAssertions;
using PayDel.Data.Dtos.Site.Admin;
using PayDel.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUTest.ControllerTest
{
    public class UserTest : IClassFixture<TestClientProvider<Startup>>
    {
        private HttpClient _client;
        private readonly string _UnToken;
        private readonly string _AToken;
        public UserTest(TestClientProvider<Startup> testClientProvider)
        {
            _client = testClientProvider.Client;
            _UnToken = "";
            _AToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJmYWE4YzQ0MC03OWJjLTRiMzYtOGUxYy1iN2ZkMzQ2ZTA0MzAiLCJ1bmlxdWVfbmFtZSI6ImZhcnNoYWR0MjBAeWFob28uY29tIiwibmJmIjoxNTk2MzU0Mzg1LCJleHAiOjE1OTY1MjcxODUsImlhdCI6MTU5NjM1NDM4NX0.itUZ3X99ncaWQTLawCN1rVribsBQ55T24cJs4jc-kzHEwpekUmtV7iNmkeNxHATXCTIdPpqTUTSe0SujhZDjBA";
        }
        [Fact]
        public async void GetUsers_Unauthorized_User_CantGetUsers()
        {
            // Arrange
            var request = "/v1/site/admin/Users";

            _client.DefaultRequestHeaders.Authorization
           = new AuthenticationHeaderValue("Bearer", _UnToken);


            //Act
            var response = await _client.GetAsync(request);

            //Assert
            //response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            //Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void GetUsers_Authorized_Can_GetUsers()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _AToken);
            var request = "/v1/site/admin/Users";

            //Failed
            //Succeeded
            //Act
            var response = await _client.GetAsync(request);

            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Fact]
        public async Task UpdateUser_CantUpdateAnOtherUser()
        {
            // Arrange

            string anOtherUserId = "29f541d5-2010-4052-a108-f743722e3e89";

            var request = new
            {
                Url = "/v1/site/admin/Users/" + anOtherUserId,
                Body = new
                {
                    Name = "علی حسینی",
                    PhoneNumber = "string",
                    Address = "string",
                    Gender = true,
                    City = "string"
                }
            };
            _client.DefaultRequestHeaders.Authorization
           = new AuthenticationHeaderValue("Bearer", _AToken);
            //Act
            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_CanUpdateUserHimself()
        {
            string userHimselfId = "faa8c440-79bc-4b36-8e1c-b7fd346e0430";

            var request = new
            {
                Url = "/v1/site/admin/Users/" + userHimselfId,
                Body = new
                {
                    Name = "فرشاد کلهر",
                    PhoneNumber = "string",
                    Address = "string",
                    Gender = true,
                    City = "string"
                }
            };

            _client.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", _AToken);
            //Act
            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }



        [Fact]
        public async Task UpdateUser_ModelStateError()
        {
            // Arrange
            string userHimselfId = "faa8c440-79bc-4b36-8e1c-b7fd346e0430";
            var request = new
            {
                Url = "/v1/site/admin/Users/" + userHimselfId,
                Body = new UserForUpdateDto
                {
                    Name = string.Empty,
                    PhoneNumber = string.Empty,
                    Address = string.Empty,
                    City = "لورم ایپسوم متن ساختگی با تولید سادگلورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است چاپگرها و متون بلکه روزنامه مجله در ستون و سطر آنچنان که لازم است و برای شرایط فعلی تکنولوژی مورد نیاز و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد کتابهای زیادی درلورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است چاپگرها و متون بلکه روزنامه مجله در ستون و سطر آنچنان که لازم است و برای شرایط فعلی تکنولوژی مورد نیاز و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد کتابهای زیادی درلورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است چاپگرها و متون بلکه روزنامه مجله در ستون و سطر آنچنان که لازم است و برای شرایط فعلی تکنولوژی مورد نیاز و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد کتابهای زیادی دری نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است چاپگرها و متون بلکه روزنامه مجله در ستون و سطر آنچنان که لازم است و برای شرایط فعلی تکنولوژی مورد نیاز و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد کتابهای زیادی در."
                }
            };
            _client.DefaultRequestHeaders.Authorization
           = new AuthenticationHeaderValue("Bearer", _AToken);

            var controller = new ModelStateControllerTests();


            //Act
            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();

            controller.ValidateModelState(request.Body);
            var modelState = controller.ModelState;

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.False(modelState.IsValid);
            Assert.Equal(4, modelState.Keys.Count());
            Assert.True(modelState.Keys.Contains("Name") && modelState.Keys.Contains("PhoneNumber")
                && modelState.Keys.Contains("Address") && modelState.Keys.Contains("City"));

        }
    }
}
