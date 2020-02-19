using System;

namespace PrimeOrdinance
{
    public static class GuidExtensions
    {
        public static Guid ToGuid(this string guid) => Guid.Parse(guid); 
    }
}