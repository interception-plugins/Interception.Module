// based on https://www.math.sci.hiroshima-u.ac.jp/m-mat/MT/VERSIONS/C-LANG/980409/mt19937-2.c
// and https://www.math.sci.hiroshima-u.ac.jp/m-mat/MT/VERSIONS/C-LANG/mt19937-64.c
using System;

namespace interception.random {
    public class mt19937 : random_number_generator {
        const int N = 624;
        const int M = 397;
        const uint MATRIX_A = 0x9908b0df;
        const uint UPPER_MASK = 0x80000000;
        const uint LOWER_MASK = 0x7fffffff;
        const uint TEMPERING_MASK_B = 0x9d2c5680;
        const uint TEMPERING_MASK_C = 0xefc60000;
        public const ulong DEFAULT_SEED = 5489; // https://cplusplus.com/reference/random/mt19937/

        static ulong TEMPERING_SHIFT_U(ulong y) => (y >> 11);
        static ulong TEMPERING_SHIFT_S(ulong y) => (y << 7);
        static ulong TEMPERING_SHIFT_T(ulong y) => (y << 15);
        static ulong TEMPERING_SHIFT_L(ulong y) => (y >> 18);

        ulong[] mt = new ulong[N];
        int mti = N + 1;

        public void seed(ulong _seed) {
            mt[0] = _seed;
            for (mti = 1; mti < N; mti++) {
                mt[mti] = (1812433253 * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + (ulong)mti) & 0xffffffff;
            }
        }

        //void generate_seed(ulong _seed) {
        //    mt[0] = _seed & 0xffffffff;
        //    for (mti = 1; mti < N; mti++)
        //        mt[mti] = (69069 * mt[mti - 1]) & 0xffffffff;
        //}

        public mt19937() : this(DEFAULT_SEED) { }
        public mt19937(ulong _seed) {
            seed(_seed);
        }

        // todo
        // public mt19937(ulong[] sequence)
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
            y ^= TEMPERING_SHIFT_U(y);
            y ^= TEMPERING_SHIFT_S(y) & TEMPERING_MASK_B;
            y ^= TEMPERING_SHIFT_T(y) & TEMPERING_MASK_C;
            y ^= TEMPERING_SHIFT_L(y);

            return ((double)y * FACT);
        }
    }
}
