﻿namespace Semver.Utility
{
#if NETSTANDARD1_1
    internal static class ListExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyCollection<T> AsReadOnly<T>(this List<T> list)
            => new ReadOnlyCollection<T>(list);
    }
#endif
}
