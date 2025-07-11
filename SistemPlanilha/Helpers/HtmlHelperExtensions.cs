using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemPlanilha.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static IEnumerable<SelectListItem> GetEnumSelectList<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = GetFriendlyName(e.ToString())
                });
        }

        private static string GetFriendlyName(string value)
        {
            // Aqui você pode personalizar o texto que aparece no dropdown
            return value
                .Replace("Win", "Windows ")
                .Replace("Pro", "Pro")
                .Replace("Ult", "Ultimate")
                .Replace("Server", "Server")
                .Replace("Office", "Office ")
                .Replace("Plus", "Plus")
                .Replace("Office20", "Office 20"); // para ficar Office 2022, 2021, etc
        }
    }
}
