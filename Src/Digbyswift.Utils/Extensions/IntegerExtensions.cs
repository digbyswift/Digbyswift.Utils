using System;

namespace Digbyswift.Utils.Extensions
{
    public static class IntegerExtensions
    {
        #region Methods: Enums

        /// <summary>
        /// Returns parsed short as enum value of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this short value)
        {
            return ToEnum<T>((int)value);
        }

        /// <summary>
        /// Returns parsed byte as enum value of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this byte value)
        {
            return ToEnum<T>((int)value);
        }

        /// <summary>
        /// Returns parsed int as enum value of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this int value)
        {
            if (!Enum.IsDefined(typeof(T), value)) throw new ArgumentOutOfRangeException("value");
            return (T)Enum.ToObject(typeof(T), value);
        }
        
        #endregion
        
    }
}
