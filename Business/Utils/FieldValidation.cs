using Microsoft.AspNetCore.Mvc;
using System;
using static DataAccess.Enum.EnumConst;

namespace API.Utils
{
    public class FieldValidation : ControllerBase
    {
        public static bool RoleValidation(int role)
        {
            return role >= 1 && role <= Enum.GetNames(typeof(RoleEnum)).Length ? true : false;
        }

        public static string DateOfBirthValidation(DateTime dateOfBirth)
        {
            if (dateOfBirth >= DateTime.Now.AddYears(-100) && dateOfBirth <= DateTime.Now.AddYears(-18))
            {
                return null;
            }
            return $"Date Of Birth: {dateOfBirth.ToString("yyyy-MM-dd")} must greater than {DateTime.Now.AddYears(-100).ToString("yyyy-MM-dd")}" +
                $" and lower than {DateTime.Now.AddYears(-18).ToString("yyyy-MM-dd")}";
        }

        public static bool PriceValidation(double price)
        {
            return price >= 0;
        }
    }
}
