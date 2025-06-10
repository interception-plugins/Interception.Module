using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace interception.database.types {
    [Serializable]
    public class json_quaternion {
        public float x;
        public float y;
        public float z;
        public float w;

        public json_quaternion() {

        }

        public json_quaternion(float x, float y, float z, float w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static implicit operator Quaternion(json_quaternion q) {
            return new Quaternion(q.x, q.y, q.z, q.w);
        }

        public static implicit operator json_quaternion(Quaternion q) {
            return new json_quaternion(q.x, q.y, q.z, q.w);
        }
    }
}
