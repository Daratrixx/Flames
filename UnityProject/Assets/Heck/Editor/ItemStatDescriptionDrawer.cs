using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ItemStatDescription))]
public class ItemStatDescriptionDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var statName = property.FindPropertyRelative("statName");
        var statValue = property.FindPropertyRelative("statValue");

        float width = position.width;
        width = width / 2;


        statName.stringValue = EditorGUI.TextField(
            new Rect(position.xMin, position.y, width, position.height),
                            statName.stringValue);

        statValue.intValue = EditorGUI.IntField(
            new Rect(position.xMax - width, position.y, width, position.height),
                            statValue.intValue);
    }
}