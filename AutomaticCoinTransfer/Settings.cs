using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticCoinTransfer
{
    public class Settings
    {
        public static string AddressCheck { get; set; } = "ZMSuU2erphJBbiEyCRrotKZmAEbCxzvYjZ";
        public static string Address { get; set; } = "ZDS5fDMgP7hyoDyEsmF133VFfs5yCkiCtC";
        public static string Ip { get; set; } = "127.0.0.1:5555";
        public static string Url { get; set; } = $"http://{Ip}";
        public static string User { get; set; } = $"1";
        public static string Password { get; set; } = $"1";
        public static double Coins { get; set; } = 50;
        public static int Seconds { get; set; } = 5;
    }
}
