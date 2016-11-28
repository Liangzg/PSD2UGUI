using UnityEngine;
using UnityEditor;

namespace subjectnerdagreement.psdexport
{
	[CustomEditor(typeof(PsdSetting))]
	public class PsdSettingEditor : Editor
	{
		protected PsdSetting m_PsdSetting;

		public void OnEnable()
		{
			m_PsdSetting = (PsdSetting)target;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Psd Path"))
			{
				m_PsdSetting.PsdPath = EditorUtility.SaveFolderPanel("Default psd file path", m_PsdSetting.PsdPath, string.Empty);
			}
			m_PsdSetting.PsdPath = GUILayout.TextArea(m_PsdSetting.PsdPath);

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();

			SerializedProperty defaultImportPath = serializedObject.FindProperty("m_DefaultImportPath");
			if (GUILayout.Button("Default Import Path"))
			{
				var path = EditorUtility.SaveFolderPanel("Default import path", m_PsdSetting.DefaultImportPath, string.Empty);
				if (path.StartsWith(Application.dataPath))
				{
					defaultImportPath.stringValue = path.Substring(Application.dataPath.Length + 1);
				}
			}
			defaultImportPath.stringValue = GUILayout.TextArea(defaultImportPath.stringValue);
			EditorGUILayout.EndHorizontal();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
