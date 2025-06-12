using System;
using System.Xml.Serialization;

using UnityEngine;

namespace interception.serialization.types {
    [Serializable]
    [XmlType("key_value_pair")]
    public class xml_pair<T1, T2> {
        public T1 key;
        public T2 value;

        public xml_pair() {
            this.key = default(T1);
            this.value = default(T2);
        }

        public xml_pair(T1 key, T2 value) {
            this.key = key;
            this.value = value;
        }
    }
}
