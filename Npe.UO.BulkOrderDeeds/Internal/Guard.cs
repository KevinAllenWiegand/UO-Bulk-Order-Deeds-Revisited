using System;
using System.Collections.Generic;
using System.Linq;

namespace Npe.UO.BulkOrderDeeds.Internal
{
    internal class Guard
    {
        public static void ArgumentNotNull(string paramName, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException($"{paramName} cannot be null.", paramName);
            }
        }

        public static void ArgumentNotEmpty(string paramName, Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{paramName} cannot be empty.", paramName);
            }
        }

        public static void ArgumentNotNullOrEmpty(string paramName, string value)
        {
            ArgumentNotNull(paramName, value);

            if (value == String.Empty)
            {
                throw new ArgumentException($"{paramName} cannot be empty.", paramName);
            }
        }

        public static void ArgumentAtLeast(string paramName, int minimumValue, int value)
        {
            if (value < minimumValue)
            {
                throw new ArgumentOutOfRangeException(paramName, $"{paramName} must be at least {minimumValue}");
            }
        }

        public static void ArgumentInRange(string paramName, int minimumValue, int maximumValue, int value)
        {
            if (value < minimumValue || value > maximumValue)
            {
                throw new ArgumentOutOfRangeException(paramName, $"{paramName} must be at least {minimumValue}, and at most {maximumValue}.");
            }
        }

        public static void ArgumentCollectionNotNullOrEmpty<T>(string paramName, IEnumerable<T> value)
        {
            ArgumentNotNull(paramName, value);

            if (!value.Any())
            {
                throw new ArgumentException($"{paramName} cannot be empty.", paramName);
            }
        }

        internal static void ArgumentNotOfValue(string paramName, int value, int[] possibleValues)
        {
            if (!possibleValues.Contains(value))
            {
                throw new ArgumentException($"{paramName} is not one of the possible values.", paramName);
            }
        }
    }
}
