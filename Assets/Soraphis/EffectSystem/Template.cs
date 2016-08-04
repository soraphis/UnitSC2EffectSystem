//
// replace all [YOUREFFECTNAME] with your actual effect name (including brackets)
// 


/*
using Assets.Soraphis.EffectSystem;
using UnityEngine;

[System.Serializable]
public struct [YOUREFFECTNAME]Data : IEffectData {

}

[CreateAssetMenu(menuName = "Effects/"+nameof([YOUREFFECTNAME]))]
[EffectData(typeof([YOUREFFECTNAME]Data))]
public class [YOUREFFECTNAME] : Effect {
    // ignore me:
    public override void ApplyEffect<T>(EffectHandlerComponent unit, T data) {
        if (data is [YOUREFFECTNAME]Data) this.ApplyEffect(unit, ([YOUREFFECTNAME]Data) (object) data);
    }

    private void ApplyEffect(EffectHandlerComponent unit, [YOUREFFECTNAME]Data data) {
        // do stuff here:

        data.Finished = true;
    }

}
*/
