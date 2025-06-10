using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace interception.database.types {
    [Serializable]
    public class json_vector3 {
        public float x;
        public float y;
        public float z;

        public json_vec3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Vector3(json_vec3 v) {
            return new Vector3(v.x, v.y, v.z);
        }

        public static implicit operator json_vec3(Vector3 v) {
            return new json_vec3(v.x, v.y, v.z);
        }
    }
}
