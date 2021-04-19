using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutomaticCoinTransfer
{
    public class SendCoinsJob : IJob
    {
        static ClientZHCash clientZHCash = new ClientZHCash(new HttpClient());

        public async Task Execute(IJobExecutionContext context)
        {
            var list = clientZHCash.GetZhCashListAddressGroupPings();
            var address = list.FirstOrDefault(e => e.Address == Settings.AddressCheck);

            Console.WriteLine($"Адрес: {address.Address}" +
                $"\n       количество монет: {address.Amount} ");

            if (address.Amount >= Settings.Coins)
            {
                clientZHCash.SendToAddressAsync(address.Address, Settings.Address, address.Amount);
                Console.WriteLine($"Переведено с адреса {address.Address} на адрес {Settings.Address}" +
                    $"\n      количество монет {address.Amount} ");
            }
        }
    }
}
