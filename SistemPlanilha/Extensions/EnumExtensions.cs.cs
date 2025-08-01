﻿using System;
using System.ComponentModel;
using System.Reflection;

namespace SistemPlanilha.Extensions  // use o namespace do seu projeto
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}
