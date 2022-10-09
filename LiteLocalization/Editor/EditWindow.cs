using UnityEngine;
using UnityEditor;

namespace Mewiof.LiteLocalization {

	public class EditWindow : EditorWindow {

		private static EditWindow _instance;

		private string _key, _value;

		public static void CloseIfInstanceIsNotNull() {
			if (_instance != null) {
				_instance.Close();
			}
		}

		public static void Open(string key, string value) {
			CloseIfInstanceIsNotNull();

			_instance = CreateInstance<EditWindow>();
			_instance.titleContent = new GUIContent("Language Element");
			_instance._key = key;
			_instance._value = value;
			_instance.ShowUtility();
		}

		private void OnGUI() {
			EditorGUILayout.LabelField("Lang Key:\t" + Localization.LangKey);
			EditorGUILayout.LabelField("Key:\t" + _key);

			_ = EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Value: ", GUILayout.MaxWidth(64f));
			EditorStyles.textArea.wordWrap = true;
			_value = EditorGUILayout.TextArea(_value, EditorStyles.textArea, GUILayout.Height(128f));
			EditorGUILayout.EndHorizontal();

			if (GUILayout.Button("Set")) {
				Localization.dict[_key] = _value;

				Loader.Save();
			}

			minSize = new Vector2 {
				x = 512f,
				y = 256f
			};
			maxSize = minSize;
		}
	}
}
