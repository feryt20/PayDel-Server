//using System;
//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Text;
//using PayDel.Common.Interface;
//using PayDel.Data.Models;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Identity;
//using PayDel.Data.Dtos.Token;
//using PayDel.Data.DatabaseContext;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using Microsoft.AspNetCore.Http;
//using Ganss.XSS;
//using DnsClient;

//namespace PayDel.Common.Helpers
//{
//    public class Utilities : IUtilities
//    {
//        private readonly IConfiguration _config;
//        private readonly TokenSetting _tokenSetting;
//        private readonly UserManager<User> _userManager;
//        private readonly PayDelDbContext _db;
//        private readonly IHttpContextAccessor _http;
//        private readonly ILookupClient _lookupClient;
//        public Utilities(PayDelDbContext dbContext, IConfiguration config, UserManager<User> userManager, IHttpContextAccessor http
//            , ILookupClient lookupClient)
//        {
//            _http = http;
//            _config = config;
//            _userManager = userManager;
//            _userManager = userManager;
//            _db = dbContext;
//            var tokenSettingSection = _config.GetSection("TokenSetting");
//            _tokenSetting = tokenSettingSection.Get<TokenSetting>();
//            _lookupClient = lookupClient;
//        }

//        public async Task<string> GenerateJwtTokenAsync(User user, bool isRemember)
//        {
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
//                new Claim(ClaimTypes.Name,user.UserName)
//            };

//            var roles = await _userManager.GetRolesAsync(user);
//            foreach (var role in roles)
//            {
//                claims.Add(new Claim(ClaimTypes.Role, role));
//            }

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

//            var tokenDes = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(claims),
//                Expires = isRemember ? DateTime.Now.AddDays(1) : DateTime.Now.AddHours(2),
//                SigningCredentials = creds
//            };

//            var tokenHandler = new JwtSecurityTokenHandler();

//            var token = tokenHandler.CreateToken(tokenDes);

//            return tokenHandler.WriteToken(token);
//        }

//        #region tokenCreateNew

//        public async Task<TokenResponseDto> GenerateNewTokenAsync(TokenRequestDto tokenRequestDto, bool needPassword)
//        {
//            if (needPassword)
//            {
//                var user = await _db.Users.Include(p => p.Photos)
//                                .SingleOrDefaultAsync(p => p.UserName == tokenRequestDto.UserName);

//                if (user != null && await _userManager.CheckPasswordAsync(user, tokenRequestDto.Password))
//                {
//                    //create new token
//                    var newRefreshToken = CreateRefreshToken(_tokenSetting.ClientId, user.Id, tokenRequestDto.IsRemember);
//                    //remove older tokens
//                    var oldRefreshToken = await _db.MyTokens.Where(p => p.UserId == user.Id).ToListAsync();

//                    if (oldRefreshToken.Any())
//                    {
//                        foreach (var ort in oldRefreshToken)
//                        {
//                            _db.MyTokens.Remove(ort);
//                        }
//                    }
//                    //add new refresh token to db
//                    _db.MyTokens.Add(newRefreshToken);

//                    _db.SaveChanges();

//                    var accessToken = await CreateAccessTokenAsync(user, newRefreshToken.Value);

//                    return new TokenResponseDto()
//                    {
//                        token = accessToken.token,
//                        refresh_token = accessToken.refresh_token,
//                        status = true,
//                        user = user
//                    };
//                }
//                else
//                {
//                    return new TokenResponseDto()
//                    {
//                        status = false,
//                        message = "کاربری با این یوزر و پس وجود ندارد"
//                    };
//                }
//            }
//            else
//            {
//                var user = await _db.Users.Include(p => p.Photos)
//                .SingleOrDefaultAsync(p => p.UserName == tokenRequestDto.UserName);

//                if (user != null)
//                {
//                    //create new token
//                    var newRefreshToken = CreateRefreshToken(_tokenSetting.ClientId, user.Id, tokenRequestDto.IsRemember);
//                    //remove older tokens
//                    var oldRefreshToken = await _db.MyTokens.Where(p => p.UserId == user.Id).ToListAsync();

//                    if (oldRefreshToken.Any())
//                    {
//                        foreach (var ort in oldRefreshToken)
//                        {
//                            _db.MyTokens.Remove(ort);
//                        }
//                    }
//                    //add new refresh token to db
//                    _db.MyTokens.Add(newRefreshToken);

//                    await _db.SaveChangesAsync();

//                    var accessToken = await CreateAccessTokenAsync(user, newRefreshToken.Value);

//                    return new TokenResponseDto()
//                    {
//                        token = accessToken.token,
//                        refresh_token = accessToken.refresh_token,
//                        status = true,
//                        //user = user
//                    };
//                }
//                else
//                {
//                    return new TokenResponseDto()
//                    {
//                        status = false,
//                        message = "کاربری با این یوزر و پس وجود ندارد"
//                    };
//                }
//            }
//        }

//        public async Task<TokenResponseDto> CreateAccessTokenAsync(User user, string refreshToken)
//        {
//            double tokenExpireTime = Convert.ToDouble(_tokenSetting.ExpireTime);

//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier,user.Id),
//                new Claim(ClaimTypes.Name,user.UserName)
//            };

//            var roles = await _userManager.GetRolesAsync(user);
//            foreach (var role in roles)
//            {
//                claims.Add(new Claim(ClaimTypes.Role, role));
//            }
//            //claims.Add(new Claim(ClaimTypes.Role, "Fake"));

//            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenSetting.Secret));
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

//            var tokenDes = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(claims),
//                Issuer = _tokenSetting.Site,
//                Audience = _tokenSetting.Audience,
//                Expires = DateTime.Now.AddMinutes(tokenExpireTime),
//                SigningCredentials = creds
//            };

//            var newAccessToken = tokenHandler.CreateToken(tokenDes);

//            var encodedAccessToken = tokenHandler.WriteToken(newAccessToken);

//            return new TokenResponseDto()
//            {
//                token = encodedAccessToken,
//                refresh_token = refreshToken
//            };
//        }

//        public MyToken CreateRefreshToken(string clientId, string userId, bool isRemember)
//        {
//            return new MyToken()
//            {
//                ClientId = clientId,
//                UserId = userId,
//                Value = Guid.NewGuid().ToString("N"),
//                ExpireTime = isRemember ? DateTime.Now.AddDays(7) : DateTime.Now.AddDays(1),
//                Ip = 
//                _http.HttpContext.Connection != null ?
//                    _http.HttpContext.Connection.RemoteIpAddress != null ?
//                    _http.HttpContext.Connection.RemoteIpAddress.ToString() :
//                    "noIp" :
//                    "noIp"
//            };

//        }

//        #endregion

//        #region tokenRefresh

//        public async Task<TokenResponseDto> RefreshAccessTokenAsync(TokenRequestDto tokenRequestDto)
//        {
//            string ip = _http.HttpContext.Connection != null
//                    ? _http.HttpContext.Connection.RemoteIpAddress != null
//                        ?
//                        _http.HttpContext.Connection.RemoteIpAddress.ToString()
//                        :
//                        "noIp"
//                    : "noIp";


//            var refreshToken = await _db.MyTokens.FirstOrDefaultAsync(p =>
//                 p.ClientId == _tokenSetting.ClientId && p.Value == tokenRequestDto.RefreshToken
//                 && p.Ip == ip
//                 );

//            if (refreshToken == null)
//            {
//                return new TokenResponseDto()
//                {
//                    status = false,
//                    message = "خطا در اعتبار سنجی خودکار"
//                };
//            }
//            if (refreshToken.ExpireTime < DateTime.Now)
//            {
//                return new TokenResponseDto()
//                {
//                    status = false,
//                    message = "خطا در اعتبار سنجی خودکار"
//                };
//            }

//            var user = await _userManager.FindByIdAsync(refreshToken.UserId);
//            if (user == null)
//            {
//                return new TokenResponseDto()
//                {
//                    status = false,
//                    message = "خطا در اعتبار سنجی خودکار"
//                };
//            }

//            var response = await CreateAccessTokenAsync(user, refreshToken.Value);

//            return new TokenResponseDto()
//            {
//                status = true,
//                token = response.token
//            };
//        }

//        #endregion



//        #region Common
//        public string FindLocalPathFromUrl(string url)
//        {
//            //var arry = url.Split("//")[1].Split('/');
//            var arry = url.Replace("https://", "").Replace("http://", "")
//                .Split('/').Skip(1).SkipLast(1);

//            return (arry.Aggregate("", (current, item) => current + (item + "\\"))).TrimEnd('\\');

//        }
//        public bool IsFile(IFormFile file)
//        {
//            if (file != null)
//            {
//                if (file.Length > 0)
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            else
//            {
//                return false;
//            }
//        }


        
//        public string RemoveHtmlXss(string html)
//        {
//            if (string.IsNullOrEmpty(html))
//                return "";

//            var _htmlSanitizer = new HtmlSanitizer();

//            return _htmlSanitizer.Sanitize(html, null);


//        }
//        public async Task<string> GetDomainIpAsync(string domain)
//        {
//            domain = domain.Replace("https://", "").Replace("http://", "").Replace("www.", "").TrimEnd('/');

//            var result = await _lookupClient.QueryAsync(domain, QueryType.A);

//            var record = result.Answers.ARecords().FirstOrDefault();
//            return record != null ? record.Address.ToString() : "";

//        }
//        #endregion




//        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
//        {
//            using (var hamc = new System.Security.Cryptography.HMACSHA512())
//            {
//                passwordSalt = hamc.Key;
//                passwordHash = hamc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//            }
//        }
//        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
//        {
//            using (var hamc = new System.Security.Cryptography.HMACSHA512(passwordSalt))
//            {
//                var cumputedHash = hamc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//                for (int i = 0; i < cumputedHash.Length; i++)
//                {
//                    if (cumputedHash[i] != passwordHash[i])
//                        return false;
//                }
//            }
//            return true;
//        }
//    }
//}
