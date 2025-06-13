using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interception.utils {
    public static class math_util {
        public static int clamp(int val, int min, int max) {
            return val > max ? max : val < min ? min : val;
        }

        public static uint clamp(uint val, uint min, uint max) {
            return val > max ? max : val < min ? min : val;
        }
    }
}
