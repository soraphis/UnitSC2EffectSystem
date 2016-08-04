using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Soraphis.EffectSystem {

    [System.Serializable]
    public class EffectHandlerComponent : MonoBehaviour, ISerializationCallbackReceiver {
        
        [SerializeField] private List<RuntimeEffect> Effects = new List<RuntimeEffect>();
        // [SerializeField] private RuntimeEffect[] Effects;

        private BinaryFormatter serializer = new BinaryFormatter();
        
        [SerializeField] [HideInInspector] private string[] Sstr;

        void Update() {
            for (var i = 0; i < Effects.Count; i++) {
                Effects[i].Update(this);
            }
            Effects.RemoveAll(fx => fx.Finished);
        }

        public void Apply(RuntimeEffect r) {
            Effects.Add((RuntimeEffect)r.Clone());
        }

        public void OnBeforeSerialize() {
            // serialize
            if(Effects == null) return;

            Sstr = new string[Effects.Count];
            for (var i = 0; i < Effects.Count; i++) {
                Sstr[i] = SerializeEffect(Effects[i]);
            }
        }
        public void OnAfterDeserialize() { 
            //deserialize 
            if(Effects == null || Sstr == null) return;

            for (var i = 0; i < Effects.Count; i++) {
                var str = i >= Sstr.Length ? "" : Sstr[i];
                Effects[i].EffectData = DeSerializeEffect(Effects[i], str);
            }

        }

        private string SerializeEffect(RuntimeEffect effect) {
            if(effect == null) return "";
            if(effect.EffectData == null) return "";

            using (var stream = new MemoryStream()) {
                serializer.Serialize(stream, effect.EffectData);
                stream.Flush();
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        private IEffectData DeSerializeEffect(RuntimeEffect effect, string str) {
            if(effect == null) return null;
            if(string.IsNullOrEmpty(str)) return null;

            byte[] bytes = Convert.FromBase64String(str);
            using (var stream = new MemoryStream(bytes)) {
                return (IEffectData) serializer.Deserialize(stream);
            }
        }
    }
}
