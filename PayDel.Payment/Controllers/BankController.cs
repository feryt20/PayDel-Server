using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Parbad;
using PayDel.Common.Helpers;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Dtos;
using PayDel.Data.Dtos.Api;
using PayDel.Repo.Infrastructures;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PayDel.Payment.Controllers
{

    public class BankController : Controller
    {
        private readonly IUnitOfWork<PayDelDbContext> _db;
        private readonly IUnitOfWork<FinDbContext> _dbFinancial;
        private readonly IOnlinePayment _onlinePayment;
        private GateApiReturn<BankPayDto> model;

        public BankController(IUnitOfWork<PayDelDbContext> dbContext,
            IUnitOfWork<FinDbContext> dbFinancial,
             IOnlinePayment onlinePayment)
        {
            _db = dbContext;
            _dbFinancial = dbFinancial;
            _onlinePayment = onlinePayment;
            model = new GateApiReturn<BankPayDto>
            {
                Result = new BankPayDto()
            };
        }
        [HttpGet]
        public async Task<IActionResult> Pay(string token)
        {

            var factorFromRepo = await _dbFinancial.FactorRepository.GetByIdAsync(token);

            if (factorFromRepo == null)
            {
                model.Status = false;
                model.Messages.Clear();
                model.Messages = new string[] { "token-error" };
                model.Result = null;
                return View(model);
            }
            //
            model.Result.Factor = factorFromRepo;
            model.Result.Gate = await _db._GateRepository.GetByIdAsync(factorFromRepo.GateId);
            //
            if (factorFromRepo.DateCreated.AddMinutes(10) < DateTime.Now)
            {
                model.Status = false;
                model.Messages.Clear();
                model.Messages = new string[] { "زمان تکمیل عملیات پرداخت تمام شده است" };
                return View(model);
            }
            if (factorFromRepo.Status)
            {
                model.Status = false;
                model.Messages.Clear();
                model.Messages = new string[] { "پرداخت قبلا به صورت موفق انجام شده است" };
                return View(model);
            }

            if (model.Result.Gate.IsDirect)
            {
                var callbackUrl = Url.Action("Verify", "Bank", null, Request.Scheme);
                var result = await _onlinePayment.RequestAsync(invoice =>
                {
                    decimal amount = Convert.ToDecimal(factorFromRepo.EndPrice);
                    invoice
                    .UseAutoIncrementTrackingNumber()
                    .SetAmount(amount)
                    .SetCallbackUrl(callbackUrl)
                    //.UseParbadVirtual();
                    .UseZarinPal("پرداخت از سایت مادپی","info@madpay724.ir", "09361234567");
                });
                if (result.IsSucceed)
                {
                    factorFromRepo.RefBank = result.TrackingNumber.ToString();
                    factorFromRepo.DateModified = DateTime.Now;
                    factorFromRepo.Bank = result.GatewayName.ToBank();

                    _dbFinancial.FactorRepository.Update(factorFromRepo);

                    if (await _dbFinancial.SaveAcync()>0)
                    {
                        await result.GatewayTransporter.TransportAsync();
                        return new EmptyResult();
                    }
                    else
                    {
                        model.Status = false;
                        model.Messages.Clear();
                        model.Messages = new string[] { "خطا در ثبت " };
                        return View(model);
                    }
                }
                else
                {
                    model.Status = false;
                    model.Messages.Clear();
                    model.Messages = new string[] { result.Message };

                    return View(model);
                }
            }
            else
            {
                model.Status = true;
                model.Messages.Clear();
                return View(model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> MadPay(string token)
        {

            var factorFromRepo = await _dbFinancial.FactorRepository.GetByIdAsync(token);

            if (factorFromRepo == null)
            {
                model.Status = false;
                model.Messages.Clear();
                model.Messages = new string[] { "token-error" };
                model.Result = null;
                return View("Pay", model);
            }
            //
            model.Result.Factor = factorFromRepo;
            model.Result.Gate = await _db._GateRepository.GetByIdAsync(factorFromRepo.GateId);
            //
            if (factorFromRepo.DateCreated.AddMinutes(10) < DateTime.Now)
            {
                model.Status = false;
                model.Messages.Clear();
                model.Messages = new string[] { "زمان تکمیل عملیات پرداخت تمام شده است" };
                return View("Pay", model);
            }
            if (factorFromRepo.Status)
            {
                model.Status = false;
                model.Messages.Clear();
                model.Messages = new string[] { "پرداخت قبلا به صورت موفق انجام شده است" };
                return View("Pay", model);
            }

            //if (!model.Result.Gate.IsDirect)
            //{
            //    model.Status = true;
            //    model.Messages.Clear();
            //    return View("Pay", model);
            //}
            decimal amount = Convert.ToDecimal(factorFromRepo.EndPrice);
            var callbackUrl = Url.Action("verify", "bank", null, Request.Scheme);
            var result = await _onlinePayment.RequestAsync(invoice =>
            {
                invoice
                .UseAutoIncrementTrackingNumber()
                .SetAmount(amount)
                .SetCallbackUrl(callbackUrl)
                .UseParbadVirtual();
                //.UseZarinPal("پرداخت از سایت مادپی");
            });
            if (result.IsSucceed)
            {
                factorFromRepo.RefBank = result.TrackingNumber.ToString();
                factorFromRepo.DateModified = DateTime.Now;
                factorFromRepo.Bank = result.GatewayName.ToBank();
                _dbFinancial.FactorRepository.Update(factorFromRepo);

                if (await _dbFinancial.SaveAcync()>0)
                {
                    await result.GatewayTransporter.TransportAsync();
                    return new EmptyResult();
                }
                else
                {
                    model.Status = false;
                    model.Messages.Clear();
                    model.Messages = new string[] { "خطا در ثبت " };
                    return View("Pay", model);
                }
            }
            else
            {
                model.Status = false;
                model.Messages.Clear();
                model.Messages = new string[] { result.Message };
                return View("Pay", model);
            }

        }
        public async Task<IActionResult> Verify(string token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var factor = await _dbFinancial.FactorRepository.GetByIdAsync(token);
                factor.IsAlreadyVerified = false;
                factor.DateModified = DateTime.Now;
                factor.Message = "کاربر پرداخت را کنسل کرده است";

                _dbFinancial.FactorRepository.Update(factor);
                await _dbFinancial.SaveAcync();
                return Redirect(factor.RedirectUrl + "?token=" + factor.Id);
            }

            var invoice = await _onlinePayment.FetchAsync();

            var factorFromRepo = (await _dbFinancial.FactorRepository
                .GetAllAsync(p => p.RefBank == invoice.TrackingNumber.ToString(), null, "")).SingleOrDefault();

            if (invoice.Status == PaymentFetchResultStatus.AlreadyProcessed)
            {
                factorFromRepo.IsAlreadyVerified = true;
                factorFromRepo.DateModified = DateTime.Now;
                factorFromRepo.Message = "این تراکنش قبلا برررسی شده است";
                factorFromRepo.GatewayName = invoice.GatewayName;

                _dbFinancial.FactorRepository.Update(factorFromRepo);
                await _dbFinancial.SaveAcync();

            }
            else
            {
                factorFromRepo.IsAlreadyVerified = invoice.IsAlreadyVerified;
                factorFromRepo.DateModified = DateTime.Now;
                factorFromRepo.Message = "پرداخت با موفقیت انجام شد اما توسط کاربر تایید نشده است";
                factorFromRepo.GatewayName = invoice.GatewayName;

                _dbFinancial.FactorRepository.Update(factorFromRepo);
                await _dbFinancial.SaveAcync();
            }

            return Redirect(factorFromRepo.RedirectUrl + "?token=" + factorFromRepo.Id);
        }
    }
}