using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MaroonSeal
{
    public struct NoiseHasher
    {
        private uint hash;

        #region Constructor
        public NoiseHasher(uint _seed) { hash = _seed; }
        #endregion

        #region Bit Mixing
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Mix(uint value)
        {
            unchecked
            {
                hash ^= value;
                hash *= 0x9E3779B1u;
                hash = (hash << 13) | (hash >> 19);
            }
        }
        #endregion

        #region Type Adding
        public void Add(int _value) => Mix((uint)_value);
        public void Add(uint _value) => Mix(_value);
        public void Add(long _value)
        {
            Mix((uint)_value);
            Mix((uint)(_value >> 32));
        }

        public void Add(ulong _value)
        {
            Mix((uint)_value);
            Mix((uint)(_value >> 32));
        }

        public void Add(float _value) => Mix((uint)BitConverter.SingleToInt32Bits(_value));

        public void Add(double _value)
        {
            ulong bits = (ulong)BitConverter.DoubleToInt64Bits(_value);
            Add(bits);
        }

        public void Add(bool value) => Mix(value ? 1u : 0u);

        public void Add(char value) => Mix(value);

        public void Add(string value)
        {
            foreach (char c in value) { Add(c); }
        }

        public void Add<T>(T value) where T : unmanaged
        {
            Add(Marshal.SizeOf<T>());

            ReadOnlySpan<T> valueSpan = MemoryMarshal.CreateReadOnlySpan(ref value, 1);
            ReadOnlySpan<byte> bytes = MemoryMarshal.AsBytes(valueSpan);

            foreach (byte b in bytes) { Mix(b); }
        }
        #endregion

        public readonly uint Finish()
        {
            uint h = hash;

            unchecked
            {
                h ^= h >> 16;
                h *= 0x85EBCA6Bu;
                h ^= h >> 13;
                h *= 0xC2B2AE35u;
                h ^= h >> 16;
            }

            return h;
        }

        public static uint Combine(uint _seed, int _a, int _b)
        {
            NoiseHasher hasher = new(_seed);
            hasher.Add(_a);
            hasher.Add(_b);
            return hasher.Finish();
        }

        public static uint Combine<T1>(uint _seed, T1 _value1) 
            where T1 : unmanaged
        {
            NoiseHasher hasher = new(_seed);
            hasher.Add(_value1);
            return hasher.Finish();
        }

        public static uint Combine<T1, T2>(uint _seed, T1 _value1, T2 _value2) 
            where T1 : unmanaged
            where T2 : unmanaged
        {
            NoiseHasher hasher = new(_seed);
            hasher.Add(_value1);
            hasher.Add(_value2);
            return hasher.Finish();
        }

        public static uint Combine<T1, T2, T3>(uint _seed, T1 _value1, T2 _value2, T3 _value3) 
            where T1 : unmanaged 
            where T2 : unmanaged
            where T3 : unmanaged
        {
            NoiseHasher hasher = new(_seed);
            hasher.Add(_value1);
            hasher.Add(_value2);
            hasher.Add(_value3);
            return hasher.Finish();
        }
    }
}
