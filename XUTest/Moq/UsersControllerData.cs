using PayDel.Data.Dtos.Site.Admin;
using PayDel.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUTest.Moq
{
    public static class UsersControllerData
    {
        public static IEnumerable<User> GetUser()
        {
            var userList = new List<User>()
            {
                new User
                {
                    Id = "0d47394e-672f-4db7-898c-bfd8f32e2af7",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    DateOfBirth = DateTime.Now,
                    LastActive = DateTime.Now,
                    PasswordHash =new byte[255],
                    PasswordSalt = new byte[255],
                    UserName = "haysmathis@barkarama.com",
                    Name = "Holloway Vasquez",
                    PhoneNumber = "55",
                    Address = "55",
                    Gender = true,
                    City = "55",
                    IsActive = true,
                    Photos = new List<Photo>()
                    {
                        new Photo()
                        {
                            Id = "0d47394e-672f-4db7-898c-bfd8f32e2af",
                            UserId = "0d47394e-672f-4db7-898c-bfd8f32e2af7",
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            Url = "qq",
                            Alt = "qq",
                            IsMain = true,
                            Description = "qq",
                        }
                    }
                }
            };
            return userList;
        }

        public static UserProfileDto GetUserForDetailedDto()
        {
            return new UserProfileDto()
            {
                Id = "0d47394e-672f-4db7-898c-bfd8f32e2af7",
                UserName = "haysmathis@barkarama.com",
                Name = "Holloway Vasquez",
                PhoneNumber = "55",
                Address = "55",
                Gender = true,
                City = "55",
                Age = 15,
                LastActive = DateTime.Now,
                ImageUrl = "qqq"
            };
        }

        public static readonly UserForUpdateDto userForUpdateDto_Fail = new UserForUpdateDto()
        {
            Name = "kldlsdnf"
        };
    }
}
