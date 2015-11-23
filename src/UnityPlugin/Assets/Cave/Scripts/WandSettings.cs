using UnityEngine;
using System.Collections;


namespace Cave
{
    [System.Serializable]
    public class WandSettings : BasicSettings
    {
        [Header("Position")]
        public bool TrackPosition = true;
        [Range(0.0f, 1.0f)]
        public float MovementSensivity;
        public BlockPositionAxis PositionAxisConstraints;
        public MovementSmoothing PositionMovementConstraints;

        [Header("Rotation")]
        public bool TrackRotation = true;
        public BlockRotationAxisWand RotationAxisConstraints;
        public MovementSmoothing RotationMovementConstraints;

        [Header("Inputs")]
        public bool allowJoystick = true;
        public WandSettingsButtons ButtonMapping;

        [Header("VRPN")]
        public string WorldVizObject = "PPT0";
        public string WorldVizObjectButtons = "PPT_WAND1";
        public int Channel = 0;
        public int Port = 8945;

        //public enum MyCustomClassEnum
        //{
        //    OptionOne,
        //    OptionTwo,
        //    OptionThree,
        //};
        //public string stringField;
        //public MyCustomClassEnum enumField;
        //public int intField;
    }

    [System.Serializable]
    public class WandSettingsButtons
    {
        public string Back = "Enter";
        public string Joystick = "Space";
        public string TopLeft = "E";
        public string TopRight = "R";
        public string BottomLeft = "T";
        public string BottomRight = "Z";
    }

    //[CustomPropertyDrawer(typeof(WandSettings))]
    //public class MyCustomClassDrawer : PropertyDrawer {

    //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //    {
    //        EditorGUI.BeginProperty(position, label, property);

    //        // Get the serialized properties for our custom class
    //        SerializedProperty stringFieldProp = property.FindPropertyRelative("stringField");
    //        SerializedProperty enumFieldProp = property.FindPropertyRelative("enumField");
    //        SerializedProperty intFieldProp = property.FindPropertyRelative("intField");

    //        // Set up Rects for the controls
    //        var stringRect = new Rect(position.x + 90, position.y, 70, position.height);
    //        var enumRect = new Rect(position.x + 165, position.y, 80, position.height);
    //        var intRect = new Rect(position.x + 250, position.y, 25, position.height);

    //        EditorGUI.PrefixLabel(position, new GUIContent("Custom Class"));
    //        EditorGUI.PropertyField(stringRect, stringFieldProp, GUIContent.none);
    //        EditorGUI.PropertyField(enumRect, enumFieldProp, GUIContent.none);
    //        EditorGUI.PropertyField(intRect, intFieldProp, GUIContent.none);

    //        EditorGUI.EndProperty();
    //    }
    //}
}