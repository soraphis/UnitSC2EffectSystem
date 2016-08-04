using UnityEngine;

namespace Assets.Soraphis.EffectSystem.Example {
    
    [System.Serializable]
    public enum DamageType {
        Heavy,
        Light,
        Whatever
    }

    [System.Serializable]
    public class DamageOverTimeEffectData : DamageEffectData {
        public float Time;

        public override object Clone() {
            return new DamageOverTimeEffectData() {
                Amount = this.Amount,
                Time = this.Time,
                Type = this.Type
            };
        }
    }

    [System.Serializable]
    public class DamageEffectData : IEffectData {
        public int Amount;
        public DamageType Type;

        public override object Clone() {
            return new DamageEffectData() {
                Amount = this.Amount,
                Type = this.Type
            };

        }
    }

    [CreateAssetMenu(menuName = "Effects/Example"+nameof(DamageEffect))]
    [EffectData(typeof(DamageEffectData))]
    public class DamageEffect : Effect {

        //! THIS LOOPHOLE HERE IS SADLY NECESSARY TO OVERCOME THE UNITY'S EDITOR
        //! PROBLEM WITH EXPOSING GENERICS OR INTERFACES IN THE INSPECTOR
        public override void ApplyEffect<T>(EffectHandlerComponent unit, T data) {
            if (data is DamageOverTimeEffectData) this.ApplyEffect(unit, (DamageOverTimeEffectData)(object)data);
            else if (data is DamageEffectData) this.ApplyEffect(unit, (DamageEffectData)(object)data);
        }

        //! DO THE ACTUALL APPLYEFFECT METHOD HERE:
        private void ApplyEffect(EffectHandlerComponent unit, DamageEffectData data) {
         // something like:
         // unit.GetComponent<HealthComponent>().ApplyDamage(data.Amount, data.Type)
            Debug.Log("Do Damage: " + data.Amount + " of type " + data.Type.ToString());
            data.Finished = true;
        }

        private void ApplyEffect(EffectHandlerComponent unit, DamageOverTimeEffectData data) {
            Debug.Log("Do Damage: " + data.Amount*Time.deltaTime + " of type " + data.Type.ToString());
            data.Time -= Time.deltaTime;

            data.Finished = data.Time <= 0;
        }



    }
    
}
