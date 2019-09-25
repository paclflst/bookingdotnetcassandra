using System;
using System.Reflection;

namespace BookingService.Model.Helper_Code.Mapping
{
    public static class AssemblyHelper
    {
        public static Type[] GetTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes();
        }
    }
}
