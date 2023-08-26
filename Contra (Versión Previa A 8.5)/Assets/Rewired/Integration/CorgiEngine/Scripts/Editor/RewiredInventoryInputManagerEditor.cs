using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Rewired.Integration.CorgiEngine.Editor {

    [CustomEditor(typeof(RewiredInventoryInputManager))]
    public class RewiredInventoryInputManagerEditor : UnityEditor.Editor {

        public override void OnInspectorGUI() {

            serializedObject.Update();

            // Draw each field
            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);
            do {
                // Exclude certain fields that are defined in the base class which are unused by the Rewired version.
                // This is done because so the base class can be inherited from to avoid code duplication, but the
                // key binding fields do not apply to the Rewired version and are replaced with Action fields.
                if(IsExcludedField(property)) continue; // skip excluded fields
                
                EditorGUILayout.PropertyField(property); // draw the field

            } while(property.NextVisible(false));

            serializedObject.ApplyModifiedProperties();
        }

        private static bool IsExcludedField(SerializedProperty property) {
            return property.propertyType == SerializedPropertyType.String && property.name.EndsWith("Key"); // all the key binding fields end with Key
        }
    }
}