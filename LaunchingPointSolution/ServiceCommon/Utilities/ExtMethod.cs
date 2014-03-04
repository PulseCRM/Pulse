using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Utilities
{
    public static class ExtMethod
    {
        public static T Parse<T>(this string value)
        {
            // Get default value for type so if string
            // is empty then we can return default value.
            T result = default(T);
            if (!string.IsNullOrEmpty(value))
            {
                // we are not going to handle exception here
                // if you need SafeParse then you should create
                // another method specially for that.
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
                result = (T)tc.ConvertFrom(value);
            }
            return result;
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetFileName(this string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                return System.IO.Path.GetFileName(path);
            }
            return string.Empty;
        }
        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetFilePath(this string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                return System.IO.Path.GetFullPath(path);
            }
            return string.Empty;
        }
    }
}
