using gnosis_safe_signer_report.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace gnosis_safe_signer_report
{
    class Program
    {
        private static IConfiguration _config;

        static void Main(string[] args)
        {
            if (args.Count() < 1)
            {
                Console.WriteLine("Gnosis Safe Transactions Report Generator v0.1");
                Console.WriteLine("This application generates a report showing the number of transactions signed by the owners of a gnosis safe address.");
                Console.WriteLine("");
                Console.WriteLine("Usage: gnosis-safe-signer-report [arguments]");
                Console.WriteLine("");
                Console.WriteLine("month    Month filter");
                Console.WriteLine("year     Year filter");
                Console.WriteLine("* both filters are required");
                Console.WriteLine("");
                Console.WriteLine("Example:");
                Console.WriteLine("gnosis-safe-signer-report 4 2022");
                Console.WriteLine("Generates a report from april of 2022.");
            }
            else
            {
                MainAsync(args).GetAwaiter().GetResult();
            }
        }

        private static async Task MainAsync(string[] args)
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            if (int.TryParse(args[0], out int month) && month <= 12 && month > 0)
            {
                if (int.TryParse(args[1], out int year) && year > 0)
                {
                    try
                    {
                        await GenerateTransactionReport(month, year);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid year");
                }
            }
            else
            {
                Console.WriteLine("Invalid month");
            }

            Console.WriteLine("The end");
        }

        public static async Task GenerateTransactionReport(int month, int year)
        {
            Settings settings = _config.GetRequiredSection("Settings").Get<Settings>();

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(settings.ApiURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                DateTime start = new DateTime(year, month, 1, 0, 0, 0);
                DateTime end = start.AddMonths(1).AddTicks(-1);

                var result = await client.GetAsync($"safes/{settings.SafeAddress}/multisig-transactions/?executed=true&limit=100&execution_date__gte={start.ToString("yyyy-MM-ddTHH:mm:ss")}&execution_date__lte={end.ToString("yyyy-MM-ddTHH:mm:ss")}");

                if (result.IsSuccessStatusCode)
                {
                    string content = await result.Content.ReadAsStringAsync();

                    SafeData safeData = JsonSerializer.Deserialize<SafeData>(content);

                    List<Confirmation> confirmations = new();

                    //Grabs all confirmations
                    safeData.results.ForEach(t => confirmations.AddRange(t.confirmations));

                    var signedTransactionsReport = confirmations.GroupBy(c => c.owner).Select(c => new { Signer = c.Key, TransactionsSigned = c.Count() });

                    string transactionsFromPeriod = string.Join(", ", safeData.results.Select(s => s.nonce));

                    Console.WriteLine($"Transactions found in period (nonce): {transactionsFromPeriod}");

                    foreach (var signer in signedTransactionsReport)
                    {
                        Console.WriteLine($"Signer: {signer.Signer} TransactionsSigned: {signer.TransactionsSigned}");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to retrieve transaction data, try again.");
                    Console.ReadLine();
                    return;
                }
            }
        }
    }
}
