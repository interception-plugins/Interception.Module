using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

using UnityEngine;

namespace interception.serialization.types {
    [Serializable]
    [XmlType("dictionary")]
    public class xml_dictionary<T1, T2> {
        public List<xml_pair<T1, T2>> pairs;

        public xml_dictionary() {
            pairs = new List<xml_pair<T1, T2>>();
        }

        public Dictionary<T1, T2> deserialize() {
            var result = new Dictionary<T1, T2>();
            var len = pairs.Count;
            for (int i = 0; i < len; i++)
                result.Add(pairs[i].key, pairs[i].value);
            return result;
        }

        public static xml_dictionary<T1, T2> serialize(Dictionary<T1, T2> dict) {
            var result = new xml_dictionary<T1, T2>();
            var len = dict.Count;
            for (int i = 0; i < len; i++)
                result.pairs.Add(new xml_pair<T1, T2>(dict.ElementAt(i).Key, dict.ElementAt(i).Value));
            return result;
        }

        public static implicit operator Dictionary<T1, T2>(xml_dictionary<T1, T2> dict) {
            return dict.deserialize();
        }
        
        public static implicit operator xml_dictionary<T1, T2>(Dictionary<T1, T2> dict) {
            return serialize(dict);
        }
    }
}
