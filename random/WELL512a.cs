// based on https://web.archive.org/web/20240118022029/http://www.iro.umontreal.ca/~panneton/well/WELL512a.c
using System;

namespace interception.random {
    public class WELL512a : random_number_generator {
        const int W = 32;
        const int R = 16;
        const int P = 0;
        const int M1 = 13;
        const int M2 = 9;
        const int M3 = 5;

        static uint MAT0POS(int t, uint v) => (v ^ (v >> t));
        static uint MAT0NEG(int t, uint v) => (v ^ (v << (-(t))));
        static uint MAT3NEG(int t, uint v) => (v << (-(t)));
        static uint MAT4NEG(int t, uint b, uint v) => (v ^ ((v << (-(t))) & b));

        uint V0, VM1, VM2, VM3, VRm1, VRm2, newV0, newV1, newVRm1;
        uint state_i = 0;
        uint[] STATE = new uint[R];
        uint z0, z1, z2;

        public void seed(uint _seed) {
            STATE[0] = _seed;
            for (state_i = 1; state_i < R; state_i++) {
                STATE[state_i] = 1812433253 * (STATE[state_i - 1] ^ (STATE[state_i - 1] >> 30)) + (uint)state_i;
            }
            state_i = 0;
        }

        public WELL512a() : this((uint)DateTime.UtcNow.Ticks) { }
        public WELL512a(uint _seed) {
            seed(_seed);
            V0 = STATE[state_i];
            VM1 = STATE[(state_i + M1) & 0x0000000fU];
            VM2 = STATE[(state_i + M2) & 0x0000000fU];
            VM3 = STATE[(state_i + M3) & 0x0000000fU];
            VRm1 = STATE[(state_i + 15) & 0x0000000fU];
            VRm2 = STATE[(state_i + 14) & 0x0000000fU];
            newV0 = STATE[(state_i + 15) & 0x0000000fU];
            newV1 = STATE[state_i];
            newVRm1 = STATE[(state_i + 14) & 0x0000000fU];
        }

        // todo
        // public WELL512a(uint[] sequence)
        // public seed(uint[] sequence)

        protected override double generate() {
            z0 = VRm1;
            z1 = MAT0NEG(-16, V0) ^ MAT0NEG(-15, VM1);
            z2 = MAT0POS(11, VM2);
            newV1 = z1 ^ z2;
            newV0 = MAT0NEG(-2, z0) ^ MAT0NEG(-18, z1) ^ MAT3NEG(-28, z2) ^ MAT4NEG(-5, 0xda442d24U, newV1);
            state_i = (state_i + 15) & 0x0000000fU;
            return ((double)STATE[state_i]) * FACT;
        }
    }
}
