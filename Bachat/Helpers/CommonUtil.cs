using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachat.Helpers
{
    class CommonUtil
    {
        public static string getMonthName(int month)
        {
            if (month == 1)
                return ("Jan");
            else if (month == 2)
                return ("Feb");
            else if (month == 3)
                return ("Mar");
            else if (month == 4)
                return ("Apr");
            else if (month == 5)
                return ("May");
            else if (month == 6)
                return ("Jun");
            else if (month == 7)
                return ("Jul");
            else if (month == 8)
                return ("Aug");
            else if (month == 9)
                return ("Sep");
            else if (month == 10)
                return ("Oct");
            else if (month == 11)
                return ("Nov");
            else if (month == 12)
                return ("Dec");
            else
                return ("");
        }

        public static string getDecimalValue(int num)
        {
            if (num == 1)
                return ("ONE");
            else if (num == 2)
                return ("TWO");
            else if (num == 3)
                return ("THREE");
            else if (num == 4)
                return ("FOUR");
            else if (num == 5)
                return ("FIVE");
            else if (num == 6)
                return ("SIX");
            else if (num == 7)
                return ("SEVEN");
            else if (num == 8)
                return ("EIGHT");
            else if (num == 9)
                return ("NINE");
            else if (num == 10)
                return ("TEN");
            else if (num == 11)
                return ("ELEVEN");
            else if (num == 12)
                return ("TWELVE");
            else if (num == 13)
                return ("THIRTEEN");
            else if (num == 14)
                return ("FOURTEEN");
            else if (num == 15)
                return ("FIFTEEN");
            else if (num == 16)
                return ("SIXTEEN");
            else if (num == 17)
                return ("SEVENTEEN");
            else if (num == 18)
                return ("EIGHTEEN");
            else if (num == 19)
                return ("NINETEEN");
            else
                return ("");
        }

        public static string getWord(int num)
        {
            if (num == 2)
                return ("TWENTY");
            else if (num == 3)
                return ("THIRTY");
            else if (num == 4)
                return ("FORTY");
            else if (num == 5)
                return ("FIFTY");
            else if (num == 6)
                return ("SIXTY");
            else if (num == 7)
                return ("SEVENTY");
            else if (num == 8)
                return ("EIGHTTY");
            else if (num == 9)
                return ("NINETY");
            else
                return ("");
        }
    }
}
