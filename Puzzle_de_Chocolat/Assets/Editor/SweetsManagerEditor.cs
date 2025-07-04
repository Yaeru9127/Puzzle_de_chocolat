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

//        // mixtures�̃��X�g�\��
//        for (int i = 0; i < mixturesProp.arraySize; i++)
//        {
//            var subList = mixturesProp.GetArrayElementAtIndex(i);

//            // �T�u���X�g�iList<MakedSweetsPair>�j��\��
//            EditorGUILayout.BeginVertical("box");
//            EditorGUILayout.LabelField($"List {i + 1}", EditorStyles.boldLabel);

//            // MakedSweetsPair�̌X�̗v�f��\��
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

//        // �V����List��ǉ�����{�^��
//        if (GUILayout.Button("Add New List"))
//        {
//            mixturesProp.arraySize++;
//        }

//        serializedObject.ApplyModifiedProperties();
//    }
//}
