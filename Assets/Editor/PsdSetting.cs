#if UNITY_EDITOR
using UnityEditor;
#endif
#if !SLUA_STANDALONE
using UnityEngine;
#endif

namespace subjectnerdagreement.psdexport
{
	public class PsdSetting : ScriptableObject
	{
		private const string DEFAULT_IMPORT_PATH = "UI";

		public string PsdPath = "";
		public string DefaultImportPath
		{
			get
			{
				if (string.IsNullOrEmpty(m_DefaultImportPath))
					return DEFAULT_IMPORT_PATH;
				return m_DefaultImportPath;
			}
		}
		[SerializeField]
		protected string m_DefaultImportPath;

		private static PsdSetting _instance = null;
		public static PsdSetting Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = Resources.Load<PsdSetting>("psdsetting");

#if UNITY_EDITOR
					if (_instance == null)
					{
						_instance = PsdSetting.CreateInstance<PsdSetting>();
						AssetDatabase.CreateAsset(_instance, "Assets/PSD/Resources/psdsetting.asset");
					}
#endif

				}

				return _instance;
			}
		}

#if UNITY_EDITOR && !SLUA_STANDALONE
		[MenuItem("PSD/Setting")]
		public static void Open()
		{
			Selection.activeObject = Instance;
		}
#endif

	}
}
