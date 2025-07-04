//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(SweetsManager))]
//public class SweetsManagerEditor : Editor
//{
//    SerializedProperty mixturesProp;

//    void OnEnable()
//    {
//        mixturesProp = serializedObject.FindProperty("mixtures");
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        // mixturesのリスト表示
//        for (int i = 0; i < mixturesProp.arraySize; i++)
//        {
//            var subList = mixturesProp.GetArrayElementAtIndex(i);

//            // サブリスト（List<MakedSweetsPair>）を表示
//            EditorGUILayout.BeginVertical("box");
//            EditorGUILayout.LabelField($"List {i + 1}", EditorStyles.boldLabel);

//            // MakedSweetsPairの個々の要素を表示
//            for (int j = 0; j < subList.arraySize; j++)
//            {
//                var element = subList.GetArrayElementAtIndex(j);

//                EditorGUILayout.PropertyField(element, new GUIContent($"Pair {j + 1}"));
//            }

//            if (GUILayout.Button($"Add Item to List {i + 1}"))
//            {
//                subList.arraySize++;
//            }

//            EditorGUILayout.EndVertical();
//        }

//        // 新しいListを追加するボタン
//        if (GUILayout.Button("Add New List"))
//        {
//            mixturesProp.arraySize++;
//        }

//        serializedObject.ApplyModifiedProperties();
//    }
//}
