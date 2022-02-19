using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace UnityEngine.UI {

    [CustomEditor(typeof(ScrollRectEtra), true)]
    [CanEditMultipleObjects]
    public class ScrollRectEtraEditor : ScrollRectEditor {
        SerializedProperty m_UpthrowGo;

        protected override void OnEnable() {
            m_UpthrowGo = serializedObject.FindProperty("upthrowGo");
            base.OnEnable();

        }

        public override void OnInspectorGUI() {
            EditorGUILayout.PropertyField(m_UpthrowGo);
            base.OnInspectorGUI();
        }


    }




}


