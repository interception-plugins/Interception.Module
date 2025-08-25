// based on https://www.math.sci.hiroshima-u.ac.jp/m-mat/MT/VERSIONS/C-LANG/980409/mt19937-2.c
using System;

namespace interception.random {
    public class mt19937_64 : random_number_generator {
        const int N = 312;
        const int M = 156;
        const ulong MATRIX_A = 0xb5026f5aa96619e9;
        const ulong UPPER_MASK = 0xffffffff80000000UL;
        const ulong LOWER_MASK = 0x7fffffffUL;
        const ulong TEMPERING_MASK_D = 0x5555555555555555;
        const ulong TEMPERING_MASK_B = 0x71d67fffeda60000;
        const ulong TEMPERING_MASK_C = 0xfff7eee000000000;
        public const ulong DEFAULT_SEED = 5489; // https://cplusplus.com/reference/random/mt19937_64/

        static ulong TEMPERING_SHIFT_U(ulong y) => (y >> 29);
        static ulong TEMPERING_SHIFT_S(ulong y) => (y << 17);
        static ulong TEMPERING_SHIFT_T(ulong y) => (y << 37);
        static ulong TEMPERING_SHIFT_L(ulong y) => (y >> 43);

        ulong[] mt = new ulong[N];
        int mti = N + 1;

        public void seed(ulong _seed) {
            mt[0] = _seed;
            for (mti = 1; mti < N; mti++) {
                mt[mti] = 6364136223846793005 * (mt[mti - 1] ^ (mt[mti - 1] >> 62)) + (ulong)mti;
            }
        }

        public mt19937_64() : this(DEFAULT_SEED) { }
        public mt19937_64(ulong _seed) {
            seed(_seed);
        }

        // todo
        // public mt19937_64(ulong[] sequence)
        // public void seed(ulong[] sequence)

        protected override double generate() {
            ulong y;
            ulong[] mag01 = new ulong[2] { 0x0, MATRIX_A };

            if (mti >= N) {
                int kk;

                for (kk = 0; kk < N - M; kk++) {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1];
                }
                for (; kk < N - 1; kk++) {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1];
                }
                y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
                mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1];

                mti = 0;
            }

            y = mt[mti++];
            y ^= TEMPERING_SHIFT_U(y) & TEMPERING_MASK_D;
            y ^= TEMPERING_SHIFT_S(y) & TEMPERING_MASK_B;
            y ^= TEMPERING_SHIFT_T(y) & TEMPERING_MASK_C;
            y ^= TEMPERING_SHIFT_L(y);

            return (y >> 11) * (1.0 / 9007199254740992.0);
        }
    }
}
