using PayDel.Common.ErrorsAndMessages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Services.Site.Admin.User
{
    public interface IWalletService
    {
        Task<bool> CheckInventoryAsync(int cost, string walletId);
        Task<ReturnMessage> DecreaseInventoryAsync(int cost, string walletId, bool isFactor = false);
        Task<ReturnMessage> IncreaseInventoryAsync(int cost, string walletId, bool isFactor = false);
        Task<ReturnMessage> EntryIncreaseInventoryAsync(int cost, string walletId);
        Task<ReturnMessage> EntryDecreaseInventoryAsync(int cost, string walletId);

        Task<ReturnMessage> EntryIncreaseOnExitMoneyAsync(int cost, string walletId);
        Task<ReturnMessage> EntryDecreaseOnExitMoneyAsync(int cost, string walletId);
    }
}
