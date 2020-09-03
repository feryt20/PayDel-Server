using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Dtos.Site.Users;
using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Site.Admin.Upload.Interface;
using PayDel.Services.Site.Admin.User;
using PayDel.Services.Site.Admin.Util;

namespace PayDel.Presentation.Controllers.Site.V1.Users
{
    [Route("v1/site/users/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class GatesController : ControllerBase
    {
        private readonly IUnitOfWork<PayDelDbContext> _db;
        private readonly IMapper _mapper;
        private readonly ILogger<GatesController> _logger;
        private readonly IUploadService _uploadService;
        private readonly IWalletService _walletService;
        private readonly IWebHostEnvironment _env;
        private readonly IUtilitiess _utilities;
        public GatesController(IUnitOfWork<PayDelDbContext> dbContext, IMapper mapper,
            ILogger<GatesController> logger, IUploadService uploadService,
            IWebHostEnvironment env, IWalletService walletService, IUtilitiess utilities)
        {
            _db = dbContext;
            _mapper = mapper;
            _logger = logger;
            _uploadService = uploadService;
            _env = env;
            _walletService = walletService;
            _utilities = utilities;
        }


        [Authorize(Policy = "RequiredUserRole")]
        //[ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet("{userId}/gates")]
        public async Task<IActionResult> GetGates(string userId)
        {
            var gatesFromRepo = await _db._GateRepository
                .GetAllAsync(p => p.Wallet.UserId == userId, s => s.OrderByDescending(x => x.IsActive), "");

            var walletsFromRepo = await _db._WalletRepository
                .GetAllAsync(p => p.UserId == userId, s => s.OrderByDescending(x => x.IsMain).ThenByDescending(x => x.IsSms), "");

            var result = new GatesWalletsForReturnDto()
            {
                Gates = _mapper.Map<List<GateForReturnDto>>(gatesFromRepo),
                Wallets = _mapper.Map<List<WalletForReturnDto>>(walletsFromRepo)
            };

            return Ok(result);
        }
        [Authorize(Policy = "RequiredUserRole")]
        //[ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet("{userId}/gates/{id}", Name = "GetGate")]
        public async Task<IActionResult> GetGate(string id, string userId)
        {

            var gateFromRepo = (await _db._GateRepository
                .GetAllAsync(p => p.Id == id, null, "Wallet")).SingleOrDefault();

            if (gateFromRepo != null)
            {
                if (gateFromRepo.Wallet.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    var walletsFromRepo = await _db._WalletRepository
                        .GetAllAsync(p => p.UserId == userId, s => s.OrderByDescending(x => x.IsMain).ThenByDescending(x => x.IsSms), "");

                    GateWalletsForReturnDto s = new GateWalletsForReturnDto();
                    var result = new GateWalletsForReturnDto()
                    {
                        Gate = _mapper.Map<GateForReturnDto>(gateFromRepo),
                        Wallets = _mapper.Map<List<WalletForReturnDto>>(walletsFromRepo)
                    };

                    return Ok(result);
                }
                else
                {
                    _logger.LogError($"کاربر   {RouteData.Values["userId"]} قصد دسترسی به درگاه دیگری را دارد");

                    return BadRequest("شما اجازه دسترسی به درگاه کاربر دیگری را ندارید");
                }
            }
            else
            {
                return BadRequest("درگاهی وجود ندارد");
            }

        }

        [Authorize(Policy = "RequiredUserRole")]
        [HttpPost("{userId}/gates")]
        public async Task<IActionResult> AddGate(string userId, [FromForm] GateForCreateDto gateForCreateDto)
        {
            var gateFromRepo = await _db._GateRepository
                .GetAsync(p => p.WebsiteUrl == gateForCreateDto.WebsiteUrl && p.Wallet.UserId == userId);

            if (gateFromRepo == null)
            {
                var gateForCreate = new Gate()
                {
                    WalletId = gateForCreateDto.WalletId,
                    IsDirect = false,
                    IsActive = false,
                    Ip = await _utilities.GetDomainIpAsync(gateForCreateDto.WebsiteUrl)
                };
                if (gateForCreateDto.File != null)
                {
                    if (gateForCreateDto.File.Length > 0)
                    {
                        var uploadRes = await _uploadService.UploadFileToLocal(
                            gateForCreateDto.File,
                            Guid.NewGuid().ToString(),
                            _env.WebRootPath,
                            $"{Request.Scheme ?? ""}://{Request.Host.Value ?? ""}{Request.PathBase.Value ?? ""}",
                            "Files\\Gate"
                        );
                        if (uploadRes.Status)
                        {
                            gateForCreate.IconUrl = uploadRes.Url;
                        }
                        else
                        {
                            return BadRequest(uploadRes.Message);
                        }
                    }
                    else
                    {
                        gateForCreate.IconUrl = string.Format("{0}://{1}{2}/{3}",
                          Request.Scheme,
                          Request.Host.Value ?? "",
                          Request.PathBase.Value ?? "",
                          "wwwroot/Files/Pic/Logo/logo-gate.png");
                    }
                }
                else
                {
                    gateForCreate.IconUrl = string.Format("{0}://{1}{2}/{3}",
                        Request.Scheme,
                        Request.Host.Value ?? "",
                        Request.PathBase.Value ?? "",
                        "wwwroot/Files/Pic/Logo/logo-gate.png");
                }
                var gate = _mapper.Map(gateForCreateDto, gateForCreate);

                await _db._GateRepository.InsertAsync(gate);

                if (await _db.SaveAcync()>0)
                {
                    var gateForReturn = _mapper.Map<GateForReturnDto>(gate);

                    return CreatedAtRoute("GetGate", new { id = gate.Id, userId = userId }, gateForReturn);
                }
                else
                    return BadRequest("خطا در ثبت اطلاعات");
            }
            {
                return BadRequest("این درگاه قبلا ثبت شده است");
            }


        }
        [Authorize(Policy = "RequiredUserRole")]
        [HttpPut("{userId}/gates/{id}")]
        public async Task<IActionResult> UpdateGate(string userId, string id, [FromForm] GateForCreateDto gateForUpdateDto)
        {
            var gateFromRepo = (await _db._GateRepository
                .GetAllAsync(p => p.Id == id, null, "Wallet")).SingleOrDefault();

            if (gateFromRepo == null)
            {
                return BadRequest("درگاهی وجود ندارد");
            }

            if (gateFromRepo.Wallet.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                if (gateForUpdateDto.File != null)
                {
                    if (gateForUpdateDto.File.Length > 0)
                    {
                        var uploadRes = await _uploadService.UploadFileToLocal(
                            gateForUpdateDto.File,
                            gateFromRepo.Wallet.UserId,
                            _env.WebRootPath,
                            $"{Request.Scheme ?? ""}://{Request.Host.Value ?? ""}{Request.PathBase.Value ?? ""}",
                            "Files\\Gate"
                        );
                        if (uploadRes.Status)
                        {
                            gateFromRepo.IconUrl = uploadRes.Url;
                        }
                        else
                        {
                            return BadRequest(uploadRes.Message);
                        }
                    }
                }
                var gate = _mapper.Map(gateForUpdateDto, gateFromRepo);
                //
                gate.Ip = await _utilities.GetDomainIpAsync(gate.WebsiteUrl);

                _db._GateRepository.Update(gate);

                if (await _db.SaveAcync()>0)
                    return NoContent();
                else
                    return BadRequest("خطا در ثبت اطلاعات");
            }
            else
            {
                _logger.LogError($"کاربر   {RouteData.Values["userId"]} قصد اپدیت به درگاه دیگری را دارد");

                return BadRequest("شما اجازه اپدیت درگاه کاربر دیگری را ندارید");
            }
        }

        [Authorize(Policy = "RequiredUserRole")]
        [HttpPut("{userId}/gates/{id}/active")]
        public async Task<IActionResult> ActiveDirectGate(string userId, string id, ActiveDirectGateDto activeDirectGateDto)
        {
            var gateFromRepo = (await _db._GateRepository
                .GetAllAsync(p => p.Id == id, null, "Wallet")).SingleOrDefault();

            if (gateFromRepo == null)
            {
                return BadRequest("درگاهی وجود ندارد");
            }
            if (!gateFromRepo.IsActive)
            {
                return BadRequest("درگاه فعال نمیباشد");
            }
            if (gateFromRepo.Wallet.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                if (activeDirectGateDto.IsDirect)
                {
                    if (await _walletService.CheckInventoryAsync(20000, activeDirectGateDto.WalletId))
                    {
                        var decResult = await _walletService.DecreaseInventoryAsync(20000, activeDirectGateDto.WalletId);
                        if (decResult.status)
                        {
                            gateFromRepo.IsDirect = activeDirectGateDto.IsDirect;
                            _db._GateRepository.Update(gateFromRepo);

                            if (await _db.SaveAcync()>0)
                            {
                                return NoContent();
                            }
                            else
                            {
                                var incResult = await _walletService.IncreaseInventoryAsync(20000, activeDirectGateDto.WalletId);
                                if (incResult.status)
                                    return BadRequest("خطا در ثبت اطلاعات");
                                else
                                    return BadRequest("خطا در ثبت اطلاعات در صورت کسری موجودی با پشتیبانی در تماس باشید");
                            }
                        }
                        else
                        {
                            return BadRequest(decResult.message);
                        }
                    }
                    else
                    {
                        return BadRequest("کیف پول انتخابی موجودی کافی ندارد");
                    }
                }
                else
                {
                    gateFromRepo.IsDirect = activeDirectGateDto.IsDirect;
                    _db._GateRepository.Update(gateFromRepo);

                    if (await _db.SaveAcync()>0)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return BadRequest("خطا در ثبت اطلاعات");
                    }
                }
            }
            else
            {
                _logger.LogError($"کاربر   {RouteData.Values["userId"]} قصد اپدیت به درگاه دیگری را دارد");

                return BadRequest("شما اجازه اپدیت درگاه کاربر دیگری را ندارید");
            }
        }
    }
}
