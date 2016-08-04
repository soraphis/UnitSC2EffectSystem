using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Soraphis.Lib;
using UnityEditor;
using UnityEngine;

namespace Assets.Soraphis.EffectSystem.Editor {
        [CustomPropertyDrawer(typeof(RuntimeEffect), true)]
        public class RuntimeEffectDrawer : PropertyDrawer {

            private Effect m_Effect;
            private readonly List<Type> m_EffectDataTypes = new List<Type>();

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
                const int margin = 25;
                if(property.FindPropertyRelative("Effect").objectReferenceValue == null) return EditorGUIUtility.singleLineHeight + margin;

                RuntimeEffect RFX;
                try {
                    RFX = property.FindPropertyRelative("Effect").GetParentReference() as RuntimeEffect;
                } catch (InvalidOperationException) {
                    return EditorGUIUtility.singleLineHeight * 2 + margin;
                }
                if(RFX == null) return EditorGUIUtility.singleLineHeight + margin; // should never happen because line 1
                if(RFX.EffectData == null) return EditorGUIUtility.singleLineHeight*2 + margin;

                var fields = RFX.EffectData.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                return EditorGUIUtility.singleLineHeight*fields.Length + margin;
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
                // base.OnGUI(position, property, label);
                var rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("Effect"));
                RuntimeEffect RFX;
                try {
                    RFX = property.FindPropertyRelative("Effect").GetParentReference() as RuntimeEffect;
                } catch (InvalidOperationException) {
                    return;
                }
                if (property.FindPropertyRelative("Effect").objectReferenceValue == null) {
                    RFX.EffectData = null;
                    return;
                }
                if(RFX.Effect == null) return;

                var dataAttrib = RFX.Effect.GetType().GetCustomAttributes(false).First(attr => attr is EffectDataAttribute) as EffectDataAttribute;
                if (dataAttrib == null) {
                    Debug.LogError("The Effect needs a EffectDataAttribute!");
                    return;
                }
                if (RFX.EffectData != null) {
                    // ReSharper disable once UseMethodIsInstanceOfType
                    if (! dataAttrib.type.IsAssignableFrom(RFX.EffectData.GetType())) {
                        RFX.EffectData = null; // b'cause type mismatch
                    }
                }
                rect.y += EditorGUIUtility.singleLineHeight;
                if (RFX.EffectData == null) {
                    Assembly alleffects = typeof(IEffectData).Assembly;
                    if (RFX.Effect != m_Effect) {
                        m_Effect = RFX.Effect;

                        m_EffectDataTypes.Clear();
                        foreach (var type in alleffects.GetTypes()) {
                            if(! dataAttrib.type.IsAssignableFrom(type)) continue;
                            if(type.IsAbstract) continue;
                            m_EffectDataTypes.Add(type);
                        }
                    }
                    if (m_EffectDataTypes.Count < 1) {
                        EditorGUI.LabelField(rect, "No Data Class found ... ");
                    } else if (m_EffectDataTypes.Count == 1) {
                        RFX.EffectData = Activator.CreateInstance(m_EffectDataTypes[0]) as IEffectData;
                        return;
                    } else {
                        var ss = new string[m_EffectDataTypes.Count+1];
                        ss[0] = "Choose DataClass";
                        for (int i = 0; i < m_EffectDataTypes.Count; i++) {
                            ss[i + 1] = m_EffectDataTypes[i].Name;
                        }
                        EditorGUI.BeginChangeCheck();
                        var option = EditorGUI.Popup(rect, 0, ss);
                        if (EditorGUI.EndChangeCheck()) {
                            if (option > 0 && option <= m_EffectDataTypes.Count) {
                                try {
                                    RFX.EffectData =
                                        Activator.CreateInstance(m_EffectDataTypes[option - 1]) as IEffectData;
                                } catch (ArgumentOutOfRangeException) {
                                    Debug.LogError("there are " + m_EffectDataTypes.Count + " types, you tried #"+option);
                                    return;
                                }
                            }
                        }

                    }
                } else {
                    var type = RFX.EffectData.GetType();
                    var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var field in fields) {
                        EditorGUI.BeginChangeCheck();
                        object x = null;

                        if (field.FieldType == typeof(int)) {
                            x = EditorGUI.IntField(rect, field.Name, (int) field.GetValue(RFX.EffectData));
                        }else if (field.FieldType.IsEnum) {
                            x = EditorGUI.EnumPopup(rect, field.Name, field.GetValue(RFX.EffectData) as Enum);
                        }else if (field.FieldType == typeof(string)) {
                            x = EditorGUI.TextField(rect, field.Name, (string) field.GetValue(RFX.EffectData));
                        }else if (field.FieldType == typeof(float)) {
                            x = EditorGUI.FloatField(rect, field.Name, (float) field.GetValue(RFX.EffectData));
                        } else {
                            EditorGUI.LabelField(rect, "some unidentifyable object");
                        }

                        if (EditorGUI.EndChangeCheck()) {
                                field.SetValue(RFX.EffectData, x);
                        }

                        rect.y += EditorGUIUtility.singleLineHeight;
                    }


                }
                
            }

            /*public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

                
            }*/
        }
}
