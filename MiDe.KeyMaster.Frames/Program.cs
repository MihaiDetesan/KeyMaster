using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Common.Internal;
using NLog;
using NLog.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace MiDe.KeyMaster.Frames
{
    internal static class Program
    {
        static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");
        static Logger logger = LogManager.Setup()
            .LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config.json", optional: true, reloadOnChange: true).Build();

                var services = new ServiceCollection().AddLogging(logging => logging.AddNLog("NLog.config"));
                var serviceProvider = services.BuildServiceProvider();
                var logger = serviceProvider.GetRequiredService<ILoggerFactory>();

                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                var abstractLogger = logger.CreateLogger("logger");

                var port = int.Parse(configuration["Port"]);
                var ip = configuration["Ip"];

                var validAdreeses = GetLocalIPAddress();
                if (!validAdreeses.Contains(ip))
                {
                    throw new ApplicationException($"{ip} is not a valid IP address");
                }

                var ethernetListenr = new EthernetNotificationListener(ip, port, abstractLogger);
                ethernetListenr.StartListenerAsync();

                Application.Run(new Form1(configuration["DbPath"], ethernetListenr, abstractLogger));
                mutex.ReleaseMutex();
            }
            else
            {
                NativeMethods.PostMessage(
                (IntPtr)NativeMethods.HWND_BROADCAST,
                NativeMethods.WM_SHOWME,
                IntPtr.Zero,
                IntPtr.Zero);
            }
        }

        public static IList<string> GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var validAdresses = new List<string>();

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    validAdresses.Add(ip.ToString());
                }
            }

            return validAdresses;
        }
    }

    internal class NativeMethods
    {
        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
    }
}