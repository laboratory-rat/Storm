using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Editor.Strom
{
    [CustomEditor(typeof(ElementsAdd))]
    public class ElementsAdd : EditorWindow
    {
        [MenuItem("Edit/Elements add %_e")]
        private static void Init()
        {
            var window = (ElementsAdd)EditorWindow.GetWindow(typeof(ElementsAdd));
            window.maxSize = new Vector2(200, 100);
        }

        private void Awake()
        {
            SimplePanel6 = Resources.Load<GameObject>("Prefabs/Game/Platforms2/Simple6");
            DeathArea = Resources.Load<GameObject>("Prefabs/Shock");
            Start = Resources.Load<GameObject>("Prefabs/Game/Start");
            CheckPoint = Resources.Load<GameObject>("Prefabs/Game/CheckPoint");
            Clear = Resources.Load<GameObject>("Prefabs/Game/Clear");
            Finish = Resources.Load<GameObject>("Prefabs/Game/Finish");
            SimpleTrigger = Resources.Load<GameObject>("Prefabs/Game/Trigger/Trigger");
        }

        [Header("Objects")]
        private GameObject SimplePanel6;

        private GameObject DeathArea;
        private GameObject DeathPlatform;

        [Header("CheckPoints")]
        private GameObject Start;

        private GameObject CheckPoint;
        private GameObject Clear;
        private GameObject Finish;

        [Header("Triggers")]
        private GameObject SimpleTrigger;

        public void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.TextArea("Panels");

            if (GUILayout.Button("Simple_6"))
            {
                var g = Instantiate(SimplePanel6, Point().position, Point().rotation) as GameObject;
                Selection.activeGameObject = g;
            }
            if (GUILayout.Button("Death area"))
            {
                var g = Instantiate(DeathArea, Point().position, Point().rotation) as GameObject;
                Selection.activeGameObject = g;
            }

            //if (GUILayout.Button("Death platform"))
            //{
            //    var g = Instantiate(DeathPlatform, Point().position, Point().rotation) as GameObject;
            //    Selection.activeGameObject = g;
            //}

            GUILayout.TextArea("Check points");

            if (GUILayout.Button("Start"))
            {
                var g = Instantiate(Start, Point().position, Point().rotation) as GameObject;
                Selection.activeGameObject = g;
            }

            if (GUILayout.Button("Check point"))
            {
                var g = Instantiate(CheckPoint, Point().position, Point().rotation) as GameObject;
                Selection.activeGameObject = g;
            }
            if (GUILayout.Button("Clear"))
            {
                var g = Instantiate(Clear, Point().position, Point().rotation) as GameObject;
                Selection.activeGameObject = g;
            }

            if (GUILayout.Button("Finish"))
            {
                var g = Instantiate(Finish, Point().position, Point().rotation) as GameObject;
                Selection.activeGameObject = g;
            }

            GUILayout.TextArea("Triggers");

            if (GUILayout.Button("Trigger"))
            {
                var g = Instantiate(SimpleTrigger, Point().position, Point().rotation) as GameObject;
                Selection.activeGameObject = g;
            }

            GUILayout.EndVertical();
        }

        private Transform Point()
        {
            return Selection.activeGameObject.transform;
        }
    }
}