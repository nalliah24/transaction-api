using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace transaction_api.Models
{
    public static class Constants
    {
        public enum TransactionStatus { New, Submitted, Pending };
        public enum TransactionType { DR, CR, OOP }


        public static string CategoryAccm = "ACCM";
        public static string CategoryTrvl = "TRVL";
        public static string CategoryFood = "FOOD";
    }
}
