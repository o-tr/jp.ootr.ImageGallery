using System;
using jp.ootr.common;
using jp.ootr.ImageDeviceController.Editor;
using jp.ootr.ImageSlide.Editor;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace jp.ootr.ImageGallery.Editor
{
    [CustomEditor(typeof(ImageGallery))]
    public class ImageGalleryEditor : CommonDeviceEditor
    {
        private SerializedProperty _restoreMode;
        private SerializedProperty _restoreTimeout;
        private SerializedProperty _seekInterval;
        private SerializedProperty _syncMode;
        private SerializedProperty _targetScreen;
        private SerializedProperty _registeredTypes;
        private SerializedProperty _registeredUrls;
        private SerializedProperty _registeredFileNames;
        private SerializedProperty _registeredVRCUrls;

        public override void OnEnable()
        {
            base.OnEnable();
            _targetScreen = serializedObject.FindProperty("targetScreen");
            _seekInterval = serializedObject.FindProperty("seekInterval");
            _restoreTimeout = serializedObject.FindProperty("restoreTimeout");
            _restoreMode = serializedObject.FindProperty("restoreMode");
            _syncMode = serializedObject.FindProperty("syncMode");
            _registeredTypes = serializedObject.FindProperty("registeredTypes");
            _registeredUrls = serializedObject.FindProperty("registeredUrls");
            _registeredFileNames = serializedObject.FindProperty("registeredFileNames");
            _registeredVRCUrls = serializedObject.FindProperty("vrcUrls");
        }

        protected override void ShowContent()
        {
            EditorGUILayout.PropertyField(_targetScreen, new GUIContent("Target Screen"), true);
            EditorGUILayout.PropertyField(_seekInterval, new GUIContent("Seek Interval"), true);
            EditorGUILayout.PropertyField(_restoreMode, new GUIContent("Restore Mode"), true);
            if (_restoreMode.enumValueIndex == (int)RestoreMode.Timeout)
                EditorGUILayout.PropertyField(_restoreTimeout, new GUIContent("Restore Timeout"), true);
            EditorGUILayout.PropertyField(_syncMode, new GUIContent("Sync Mode"), true);
            
            BuildURLs((ImageGallery)target);

            serializedObject.ApplyModifiedProperties();
        }

        protected override void ShowScriptName()
        {
            EditorGUILayout.LabelField("ImageGallery", EditorStyle.UiTitle);
        }
        
        

        private void BuildURLs(ImageGallery script)
        {
            EditorGUILayout.LabelField("URLs", EditorStyles.boldLabel);
            var originalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50;
            
            var size = script.registeredUrls.Length;
            var flag = false;
            
            for (var i = 0; i < size; i++)
            {
                EditorGUILayout.BeginHorizontal();
                flag = BuildSourceItem(script, ref i) || flag;
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Add Element"))
            {
                _registeredTypes.arraySize++;
                _registeredUrls.arraySize++;
                _registeredFileNames.arraySize++;
                
                flag = true;
            }

            if (flag)
            {
                var urls = script.registeredUrls.Unique();
                _registeredVRCUrls.arraySize = urls.Length;
                
                for (int i = 0; i < urls.Length; i++)
                {
                    _registeredVRCUrls.GetArrayElementAtIndex(i).FindPropertyRelative("url").stringValue = urls[i];
                }
                
                serializedObject.ApplyModifiedProperties();
            }
            EditorGUIUtility.labelWidth = originalLabelWidth;
            EditorGUILayout.EndHorizontal();
        }
        
        private bool BuildSourceItem(ImageGallery script, ref int index)
        {
            if (index >= _registeredTypes.arraySize || index < 0)
            {
                return false;
            }
            EditorGUI.BeginChangeCheck();
            var type = _registeredTypes.GetArrayElementAtIndex(index).intValue = (int)(SourceType)EditorGUILayout.EnumPopup("Type", script.registeredTypes[index], GUILayout.Width(125));
            _registeredUrls.GetArrayElementAtIndex(index).stringValue = EditorGUILayout.TextField("URL", script.registeredUrls[index]);
            if ((SourceType)type == SourceType.TextZipImage)
            {
                var fileName = _registeredFileNames.GetArrayElementAtIndex(index).stringValue;
                _registeredFileNames.GetArrayElementAtIndex(index).stringValue = EditorGUILayout.TextField("File Name", fileName);
            }
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                _registeredTypes.DeleteArrayElementAtIndex(index);
                _registeredUrls.DeleteArrayElementAtIndex(index);
                _registeredFileNames.DeleteArrayElementAtIndex(index);
                index--;
                return true;
            }
            return EditorGUI.EndChangeCheck();
        }
    }
}
