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

        [Header("Custom Mouse Cursor")]
        [Tooltip("Add custom MouseCursor (Texture2D) here. Make sure Texture Type is \"Cursor\"")]
        public Texture2D Cursor;
    }

    //todo, editor stuff
    [System.Serializable]
    public class WandSettingsButtons
    {
        public CaveInput Back = CaveInput.Return;
        public CaveInput Joystick = CaveInput.Space;
        public CaveInput TopLeft = CaveInput.Control;
        public CaveInput TopRight = CaveInput.Shift;
        public CaveInput BottomLeft = CaveInput.Number_0;
        public CaveInput BottomRight = CaveInput.Number_1;
    }

    public enum CaveInput
    {
        A = WindowsInput.VirtualKeyCode.VK_A,
        B = WindowsInput.VirtualKeyCode.VK_B,
        C = WindowsInput.VirtualKeyCode.VK_C,
        D = WindowsInput.VirtualKeyCode.VK_D,
        E = WindowsInput.VirtualKeyCode.VK_E,
        F = WindowsInput.VirtualKeyCode.VK_F,
        G = WindowsInput.VirtualKeyCode.VK_G,
        H = WindowsInput.VirtualKeyCode.VK_H,
        I = WindowsInput.VirtualKeyCode.VK_I,
        J = WindowsInput.VirtualKeyCode.VK_J,
        K = WindowsInput.VirtualKeyCode.VK_K,
        L = WindowsInput.VirtualKeyCode.VK_L,
        M = WindowsInput.VirtualKeyCode.VK_M,
        N = WindowsInput.VirtualKeyCode.VK_N,
        O = WindowsInput.VirtualKeyCode.VK_O,
        P = WindowsInput.VirtualKeyCode.VK_P,
        Q = WindowsInput.VirtualKeyCode.VK_Q,
        R = WindowsInput.VirtualKeyCode.VK_R,
        S = WindowsInput.VirtualKeyCode.VK_S,
        T = WindowsInput.VirtualKeyCode.VK_T,
        U = WindowsInput.VirtualKeyCode.VK_U,
        V = WindowsInput.VirtualKeyCode.VK_V,
        W = WindowsInput.VirtualKeyCode.VK_W,
        X = WindowsInput.VirtualKeyCode.VK_X,
        Y = WindowsInput.VirtualKeyCode.VK_Y,
        Z = WindowsInput.VirtualKeyCode.VK_Z,
        Number_0 = WindowsInput.VirtualKeyCode.VK_0,
        Number_1 = WindowsInput.VirtualKeyCode.VK_1,
        Number_2 = WindowsInput.VirtualKeyCode.VK_2,
        Number_3 = WindowsInput.VirtualKeyCode.VK_3,
        Number_4 = WindowsInput.VirtualKeyCode.VK_4,
        Number_5 = WindowsInput.VirtualKeyCode.VK_5,
        Number_6 = WindowsInput.VirtualKeyCode.VK_6,
        Number_7 = WindowsInput.VirtualKeyCode.VK_7,
        Number_8 = WindowsInput.VirtualKeyCode.VK_8,
        Number_9 = WindowsInput.VirtualKeyCode.VK_9,
        Escape = WindowsInput.VirtualKeyCode.ESCAPE,
        Shift = WindowsInput.VirtualKeyCode.SHIFT,
        Control = WindowsInput.VirtualKeyCode.CONTROL,
        Space = WindowsInput.VirtualKeyCode.SPACE,
        Return = WindowsInput.VirtualKeyCode.RETURN,
        Back = WindowsInput.VirtualKeyCode.BACK,
        Delete = WindowsInput.VirtualKeyCode.DELETE,
        Left = WindowsInput.VirtualKeyCode.LEFT,
        Right = WindowsInput.VirtualKeyCode.RIGHT,
        Up = WindowsInput.VirtualKeyCode.UP,
        Down = WindowsInput.VirtualKeyCode.DOWN,
        Add = WindowsInput.VirtualKeyCode.ADD,
        Subtract = WindowsInput.VirtualKeyCode.SUBTRACT
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