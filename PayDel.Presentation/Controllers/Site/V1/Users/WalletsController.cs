using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Dtos.Site.Users;
using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Site.Admin.User;

namespace PayDel.Presentation.Controllers.Site.V1.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IUnitOfWork<PayDelDbContext> _db;
        private readonly IMapper _mapper;
        private readonly ILogger<WalletsController> _logger;
        private readonly IWalletService _walletService;
        //private readonly IUnitOfWork<Financial_MadPayDbContext> _dbFinancial;
        //private ApiReturn<string> errorModel;

        public WalletsController(IUnitOfWork<PayDelDbContext> dbContext, IMapper mapper,
            ILogger<WalletsController> logger, IWalletService walletService
            //,IUnitOfWork<Financial_MadPayDbContext> dbFinancial
            )
        {
            _db = dbContext;
            _mapper = mapper;
            _logger = logger;
            _walletService = walletService;
            //_dbFinancial = dbFinancial;
            //errorModel = new ApiReturn<string>
            //{
            //    Status = false,
            //    Result = null
            //};
        }


        [Authorize(Policy = "RequiredUserRole")]
        ////[ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet("{userId}/wallets")]
        public async Task<IActionResult> GetWallets(string userId)
        {
            var walletsFromRepo = await _db._WalletRepository
                .GetAllAsync(p => p.UserId == userId, s => s.OrderByDescending(x => x.IsMain).ThenByDescending(x => x.IsSms), "");

            var wallets = _mapper.Map<List<WalletForReturnDto>>(walletsFromRepo);

            return Ok(wallets);
        }

        [Authorize(Policy = "RequiredUserRole")]
        //[ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet("{userId}/wallets/{id}", Name = "GetWallet")]
        public async Task<IActionResult> GetWallet(string id, string userId)
        {
            var walletFromRepo = await _db._WalletRepository.GetByIdAsync(id);
            if (walletFromRepo != null)
            {
                if (walletFromRepo.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    var wallet = _mapper.Map<WalletForReturnDto>(walletFromRepo);

                    return Ok(wallet);
                }
                else
                {
                    _logger.LogError($"کاربر   {RouteData.Values["userId"]} قصد دسترسی به کیف پول دیگری را دارد");

                    return BadRequest("شما اجازه دسترسی به کیف پول کاربر دیگری را ندارید");
                }
            }
            else
            {
                return BadRequest("کیف پولی وجود ندارد");
            }

        }

        [Authorize(Policy = "RequiredUserRole")]
        [HttpPost("{userId}/wallets")]
        public async Task<IActionResult> AddWallet(string userId, WalletForCreateDto walletForCreateDto)
        {
            var walletFromRepo = await _db._WalletRepository
                .GetAsync(p => p.Name == walletForCreateDto.WalletName && p.UserId == userId);
            var walletCount = await _db._WalletRepository.WalletCountAsync(userId);

            if (walletFromRepo == null)
            {
                if (walletCount <= 10)
                {
                    var code = await _db._WalletRepository.GetLastWalletCodeAsync() + 1;

                    while (await _db._WalletRepository.WalletCodeExistAsync(code))
                    {
                        code += 1;
                    }

                    if (await _walletService.CheckInventoryAsync(1000, walletForCreateDto.WalletId))
                    {
                        var decResult = await _walletService.DecreaseInventoryAsync(1000, walletForCreateDto.WalletId);
                        if (decResult.status)
                        {
                            var wallet = new Wallet()
                            {
                                UserId = userId,
                                IsBlock = false,
                                Code = code,
                                Name = walletForCreateDto.WalletName,
                                IsMain = false,
                                IsSms = false,
                                Inventory = 0,
                                InterMoney = 0,
                                ExitMoney = 0,
                                OnExitMoney = 0
                            };

                            await _db._WalletRepository.InsertAsync(wallet);

                            if (await _db.SaveAcync()>0)
                            {
                                var walletForReturn = _mapper.Map<WalletForReturnDto>(wallet);

                                return CreatedAtRoute("GetWallet", new { id = wallet.Id, userId = userId },
                                    walletForReturn);
                            }
                            else
                            {
                                var incResult = await _walletService.IncreaseInventoryAsync(1000, walletForCreateDto.WalletId);
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
                {
                    return BadRequest("شما اجازه وارد کردن بیش از 10 کیف پول را ندارید");
                }
            }
            {
                return BadRequest("این کیف پول قبلا ثبت شده است");
            }


        }


        //[Authorize(Policy = "RequiredUserRole")]
        ////[ServiceFilter(typeof(UserCheckIdFilter))]
        //[HttpPost(SiteV1Routes.Wallet.GetBankGate)]
        //public async Task<IActionResult> GetBankGate(string userId, string walletId, GetBankGateDto getBankGateDto)
        //{
        //    var model = new ApiReturn<string>
        //    {
        //        Status = true
        //    };

        //    var walletFromRepo = await _db._WalletRepository.GetByIdAsync(walletId);
        //    if (walletFromRepo != null)
        //    {
        //        if (walletFromRepo.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
        //        {
        //            var factorToCreate = new Factor()
        //            {
        //                UserId = userId,
        //                GateId = "0",
        //                EnterMoneyWalletId = walletId,
        //                UserName = "",
        //                Mobile = "",
        //                Email = "",
        //                FactorNumber = "",
        //                Description = "افزایش موجودی کیف پول",
        //                ValidCardNumber = "",
        //                RedirectUrl = "",
        //                Status = false,
        //                Kind = (int)FactorTypeEnums.IncInventory,
        //                Bank = (int)BankEnums.ZarinPal,
        //                GiftCode = "",
        //                IsGifted = false,
        //                Price = getBankGateDto.Price,
        //                EndPrice = getBankGateDto.Price,
        //                RefBank = "پرداختی انجام نشده است",
        //                IsAlreadyVerified = false,
        //                GatewayName = "non",
        //                Message = "خطای نامشخص"
        //            };

        //            await _dbFinancial.FactorRepository.InsertAsync(factorToCreate);

        //            if (await _dbFinancial.SaveAsync())
        //            {
        //                model.Message = "بدون خطا";
        //                model.Result = $"{Request.Scheme ?? ""}://{Request.Host.Value ?? ""}{Request.PathBase.Value ?? ""}" +
        //                        "/bank/pay/" + factorToCreate.Id;
        //                return Ok(model);
        //            }
        //            else
        //            {
        //                errorModel.Message = "خطا در ثبت فاکتور";
        //                return BadRequest(errorModel);
        //            }
        //        }
        //        else
        //        {
        //            _logger.LogError($"کاربر   {RouteData.Values["userId"]} قصد دسترسی به کیف پول دیگری را دارد");

        //            return BadRequest("شما اجازه دسترسی به کیف پول کاربر دیگری را ندارید");
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest("کیف پولی وجود ندارد");
        //    }

        //}

    }
}
