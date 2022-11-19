﻿using System.Runtime.CompilerServices;
using System.Threading;

namespace MetaFac.Threading
{
    public sealed class InterlockedCounter
    {
        private readonly InterlockedCounter? _parent;

        private long _count = 0;
        public long Count => _count;

        public InterlockedCounter(InterlockedCounter? parent)
        {
            _parent = parent;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(long value)
        {
            Interlocked.Add(ref _count, value);
            var parent = _parent;
            while (!(parent is null))
            {
                Interlocked.Add(ref parent._count, value);
                parent = parent._parent;
            }
        }
    }
}