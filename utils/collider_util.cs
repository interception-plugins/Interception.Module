using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace interception.utils {
    public static class collider_util {
        public static Vector3[] get_collider_vertex_positions(GameObject obj) {
			var collider = obj.GetComponent<Collider>();
			if (collider == null)
				throw new Exception($"cannot get any collider from {obj.name}");
			var vertices = new Vector3[8];
            var thisMatrix = obj.transform.localToWorldMatrix;
            var storedRotation = obj.transform.rotation;
            obj.transform.rotation = Quaternion.identity;
			var extents = collider.bounds.extents;
			vertices[0] = thisMatrix.MultiplyPoint3x4(extents);
			vertices[1] = thisMatrix.MultiplyPoint3x4(new Vector3(-extents.x, extents.y, extents.z));
			vertices[2] = thisMatrix.MultiplyPoint3x4(new Vector3(extents.x, extents.y, -extents.z));
			vertices[3] = thisMatrix.MultiplyPoint3x4(new Vector3(-extents.x, extents.y, -extents.z));
			vertices[4] = thisMatrix.MultiplyPoint3x4(new Vector3(extents.x, -extents.y, extents.z));
			vertices[5] = thisMatrix.MultiplyPoint3x4(new Vector3(-extents.x, -extents.y, extents.z));
			vertices[6] = thisMatrix.MultiplyPoint3x4(new Vector3(extents.x, -extents.y, -extents.z));
			vertices[7] = thisMatrix.MultiplyPoint3x4(-extents);

			obj.transform.rotation = storedRotation;
			return vertices;
		}
    }
}
