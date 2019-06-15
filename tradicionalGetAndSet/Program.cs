using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace tradicionalGetAndSet
{
    class Program
    {
        // Method to retrieve the enum description
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        enum AccountErrorsMessages
        {
            [Description("Balance is negative")]
            BalanceNegative
        }
        

        public class BalanceNegativeException : Exception
        {
            public BalanceNegativeException(string exception)
                : base(exception)
            {
            }
        }

        public class Account
        {
            private string _client { get; set; }
            private double _balance;

            public double Balance
            {
                get
                {
                    return _balance;
                }
                set
                {
                    if (this._balance < 0)
                        throw new BalanceNegativeException(GetEnumDescription(AccountErrorsMessages.BalanceNegative));
                }
             }

            public Account()
            {
                this._client = "Without name";
                this._balance = -1;
            }

            public string getClient()
            {
                return this._client;
            }

            public void setClient(string name)
            {
                this._client = name;
            }

            public double Debit(double value)
            {
                var res = this._balance - value;
                if (res < 0)
                    throw new BalanceNegativeException(GetEnumDescription(AccountErrorsMessages.BalanceNegative));
                return this._balance;
            }

            public double Credit(double value)
            {
                this._balance = this._balance + value;
                return this._balance;
            }

            public double getBalance()
            {
                return this._balance;
            }
        }


        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(GetEnumDescription(AccountErrorsMessages.BalanceNegative));
                Account ac = new Account();
                ac.setClient("Billy");

                Console.WriteLine(ac.getClient());

                ac.Credit(100.0);
                Console.WriteLine(ac.getBalance());

                ac.Debit(500.0);
                Console.WriteLine(ac.getBalance());

            }
            catch (BalanceNegativeException bne)
            {
                Console.WriteLine(bne.Message);
            }


            Console.ReadLine();
        }
    }
}
