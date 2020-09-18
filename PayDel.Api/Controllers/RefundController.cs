using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Parbad;
using PayDel.Common.Helpers;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Dtos;
using PayDel.Data.Dtos.Api;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Site.Admin.Util;

namespace PayDel.Api.Controllers
{
    [ApiVersion("1")]
    [Route("v{v:apiVersion}")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [AllowAnonymous]
    public class RefundController : ControllerBase
    {
        private readonly IUnitOfWork<PayDelDbContext> _db;
        private readonly IUnitOfWork<FinDbContext> _dbFinancial;
        private readonly IMapper _mapper;
        private readonly ILogger<RefundController> _logger;
        private readonly IUtilitiess _utilities;
        private readonly IOnlinePayment _onlinePayment;
        private GateApiReturn<string> errorModel;

        public RefundController(IUnitOfWork<PayDelDbContext> dbContext,
            IUnitOfWork<FinDbContext> dbFinancial,
            IMapper mapper, ILogger<RefundController> logger,
            IUtilitiess utilities, IOnlinePayment onlinePayment)
        {
            _db = dbContext;
            _dbFinancial = dbFinancial;
            _mapper = mapper;
            _logger = logger;
            _utilities = utilities;
            _onlinePayment = onlinePayment;
            errorModel = new GateApiReturn<string>
            {
                Status = false,
                Result = null
            };
        }
        [HttpPost("refund")]
        [ProducesResponseType(typeof(GateApiReturn<RefundResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GateApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefundSend(RefundRequestDto refundRequestDto)
        {

            var model = new GateApiReturn<RefundResponseDto>
            {
                Status = true
            };
            //Error
            var factorFromRepo = await _dbFinancial.FactorRepository.GetByIdAsync(refundRequestDto.Token);
            if (factorFromRepo == null)
            {
                errorModel.Messages.Clear();
                errorModel.Messages = new string[] { "تراکنشی با این مشخصات یافت نشد" };
                return BadRequest(errorModel);
            }
            if (factorFromRepo.DateModified.AddMinutes(20) < DateTime.Now)
            {
                errorModel.Messages.Clear();
                errorModel.Messages = new string[] { "زمان استرداد تراکنش شما گذشته است" };
                return BadRequest(errorModel);
            }
            if (factorFromRepo.IsAlreadyVerified)
            {
                errorModel.Messages.Clear();
                errorModel.Messages = new string[] { "این تراکنش قبلا بررسی شده است" };
                return BadRequest(errorModel);
            }
            var gateFromRepo = (await _db._GateRepository.GetAllAsync(p => p.Id == refundRequestDto.Api, null, "Wallet")).SingleOrDefault();
            if (gateFromRepo == null)
            {
                errorModel.Messages.Clear();
                errorModel.Messages = new string[] { "Api درگاه معتبر نمیباشد" };
                return BadRequest(errorModel);
            }
            //var userDocuments = await _db.DocumentRepository
            //    .GetManyAsync(p => p.Approve == 1 && p.UserId == gateFromRepo.Wallet.UserId, null, "");
            //if (!userDocuments.Any())
            //{
            //    errorModel.Messages.Clear();
            //    errorModel.Messages = new string[] { "مدارک کاربر صاحب درگاه تکمیل نمیباشد" };
            //    return BadRequest(errorModel);
            //}
            if (!gateFromRepo.IsActive)
            {
                errorModel.Messages.Clear();
                errorModel.Messages = new string[] { "این درگاه غیر فعال میباشد . درصورت نیاز با پشتیبانی در تماس باید" };
                return BadRequest(errorModel);
            }
            if (gateFromRepo.IsIp)
            {
                var currentIp = HttpContext.Connection.RemoteIpAddress.ToString(); //::1
                var gateWebsiteIp = await _utilities.GetDomainIpAsync(gateFromRepo.WebsiteUrl);
                if (currentIp != gateWebsiteIp)
                {
                    errorModel.Messages.Clear();
                    errorModel.Messages = new string[] { "آی پی وبسایت درخواست دهنده پرداخت معبتر نمیباشد" };
                    return BadRequest(errorModel);
                }
            }


            //Refund
            var trackingNumber = Convert.ToInt64(factorFromRepo.RefBank);
            var refundResult = await _onlinePayment.RefundCompletelyAsync(trackingNumber);
            if (refundResult.IsSucceed)
            {
                factorFromRepo.Status = false;
                factorFromRepo.IsAlreadyVerified = true;
                factorFromRepo.DateModified = DateTime.Now;
                factorFromRepo.Message = "تراکنش انجام شده و مبلغ استرداد شده است";
                _dbFinancial.FactorRepository.Update(factorFromRepo);
                await _dbFinancial.SaveAcync();

                model.Messages.Clear();
                model.Messages = new string[] { "مبلغ تراکنش استرداد شد" };
                model.Result = new RefundResponseDto
                {
                    Amount = factorFromRepo.EndPrice,
                    FactorNumber = factorFromRepo.FactorNumber,
                    RefBank = "MPC-" + factorFromRepo.RefBank,
                    Mobile = factorFromRepo.Mobile,
                    Email = factorFromRepo.Email,
                    Description = factorFromRepo.Description,
                    CardNumber = factorFromRepo.ValidCardNumber
                };
                return Ok(model);
            }
            else
            {
                factorFromRepo.IsAlreadyVerified = true;
                factorFromRepo.DateModified = DateTime.Now;
                factorFromRepo.Message = refundResult.Message;
                _dbFinancial.FactorRepository.Update(factorFromRepo);
                await _dbFinancial.SaveAcync();


                errorModel.Messages.Clear();
                errorModel.Messages = new string[] { refundResult.Message };
                return BadRequest(errorModel);
            }
        }
    }
}
