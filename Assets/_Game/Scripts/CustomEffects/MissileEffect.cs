using Assets.Soraphis.EffectSystem;
using UnityEngine;

namespace Assets._Game.Scripts.CustomEffects {

    [System.Serializable]
    public class MissileEffectData : IEffectData {
        public float MissileSpeed;

        public override object Clone() {
            return new MissileEffectData() {
                MissileSpeed =  this.MissileSpeed
            };
        }
    }

    [CreateAssetMenu(menuName = "Effects/"+nameof(MissileEffect))]
    [EffectData(typeof(MissileEffectData))]
    public class MissileEffect : Effect {
        // ignore me:
        public override void ApplyEffect<T>(EffectHandlerComponent unit, T data) {
            if (data is MissileEffectData) this.ApplyEffect(unit, data);
        }

        private void ApplyEffect(EffectHandlerComponent unit, MissileEffectData data) {
            // do stuff here:

            // data.Finished = true;
        }

    }

}
