using System;
using UnityEngine;

namespace Assets.Soraphis.EffectSystem {
    [System.Serializable]
    public class RuntimeEffect : ICloneable{
        public Effect Effect;
        public IEffectData EffectData;

        public void Update(EffectHandlerComponent unit) {
            if (Effect == null || EffectData == null) return;
            Effect.ApplyEffect(unit, EffectData);
        }

        public bool Finished => EffectData?.Finished ?? false;

        public object Clone() {
            var result = new RuntimeEffect();
            result.Effect = this.Effect;
            result.EffectData = (IEffectData)EffectData.Clone();
            return result;
        }
    }


    public class EffectDataAttribute : Attribute {
        public readonly Type type;
        public EffectDataAttribute(Type t) { this.type = t; }
    }

    [System.Serializable]
    public abstract class IEffectData : ICloneable{
        public virtual bool Finished { get; set; }
        public abstract object Clone();
    }

    [System.Serializable]
    public abstract class Effect : ScriptableObject {

        public abstract void ApplyEffect<T>(EffectHandlerComponent unit, T data) where T : IEffectData;
    }




}
