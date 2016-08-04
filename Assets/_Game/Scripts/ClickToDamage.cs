using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Soraphis.EffectSystem;

public class ClickToDamage : MonoBehaviour, ISerializationCallbackReceiver {
    private EffectHandlerComponent EHC;

    [SerializeField] public RuntimeEffect EffectToBeApplied;

	// Use this for initialization
	void Awake () { EHC = this.GetComponent<EffectHandlerComponent>(); }
	
    void OnMouseDown() {
        if(EffectToBeApplied == null || EffectToBeApplied.EffectData == null) return;


        EHC.Apply(EffectToBeApplied);
    }



    // -----------------
#region EffectDataSerialization
    private BinaryFormatter serializer = new BinaryFormatter();
    [SerializeField] [HideInInspector] private string Sstr; // string[] for saving an array

    public void OnBeforeSerialize() {
        // serialize
        if(EffectToBeApplied == null) return;
        Sstr = SerializeEffect(EffectToBeApplied);
        /* to save an array 
        Sstr = new string[EffectToBeApplied.Count];
        for (var i = 0; i < EffectToBeApplied.Count; i++) {
            Sstr[i] = SerializeEffect(EffectToBeApplied[i]);
        }
        */
    }
    public void OnAfterDeserialize() { 
        //deserialize 
        if(EffectToBeApplied == null || Sstr == null) return;
        EffectToBeApplied.EffectData = DeSerializeEffect(EffectToBeApplied, Sstr);

        /* Array:
        for (var i = 0; i < Effects.Count; i++) {
            var str = i >= Sstr.Length ? "" : Sstr[i];
            Effects[i].EffectData = DeSerializeEffect(Effects[i], str);
        }
        */
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

#endregion

}
