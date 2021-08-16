using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob) //03/10/2000
        {  //explanation in CSharpLearning Console Project
            var today = DateTime.Today; //13/08/2021
            var age = today.Year - dob.Year;  //2021 - 2000 = 21age
            if (dob.Date > today.AddYears(-age)) age--;   //if(03/10/2000 > 13/08/2000) //upcoming
            return age;
        }
    }
}
