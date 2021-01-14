using System;
using System.Collections.Generic;

namespace TikzFix.Utils
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="col"></param>
        /// <param name="ClearCallback">Callback that will be executed before clear</param>
        public static void Clear<T>(this ICollection<T> col, Action<ICollection<T>> ClearCallback)
        {
            ClearCallback(col);
            col.Clear();
        }
    }
}
