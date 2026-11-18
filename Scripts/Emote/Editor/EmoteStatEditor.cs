# if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(EmoteStat))]
public class EmoteStatEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty typeProperty = property.FindPropertyRelative("Type");
        SerializedProperty amountProperty = property.FindPropertyRelative("Amount");
        SerializedProperty multiplierProperty = property.FindPropertyRelative("Multiplier");

        Rect typeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect valueRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);

        // Detect change
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(typeRect, typeProperty);
        if (EditorGUI.EndChangeCheck())
        {
            if (typeProperty.enumValueIndex == (int)StatType.Multiplier)
            {
                amountProperty.floatValue = 0f; // Reset Amount when Multiplier selected.
            }
            else
            {
                multiplierProperty.floatValue = 1f; // Reset Multiplier for non-Multiplier types.
            }
        }

        if (typeProperty.enumValueIndex == (int)StatType.Multiplier)
        {
            EditorGUI.PropertyField(valueRect, multiplierProperty);
        }
        else
        {
            EditorGUI.PropertyField(valueRect, amountProperty);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Height for 2 lines (type + value)
        return EditorGUIUtility.singleLineHeight * 2 + 4;
    }
}
#endif