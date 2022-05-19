using UnityEngine;
using UnityEditor;

namespace Mewiof.LiteLocalization {

	public class LocalizationWindow : EditorWindow {

		public static string elemKey;
		private static string _langKey, _elemValue;

		[MenuItem("Window/Localization", false)]
		public static void Open() {
			_ = GetWindow<LocalizationWindow>(false, "Localization", true);
		}

		private void OnEnable() {
			_langKey = Localization.LangKey;
			elemKey = string.Empty;

			minSize = new Vector2 {
				x = 640f,
				y = 512f
			};
			maxSize = minSize;
		}

		private void OnLostFocus() {
			_langKey = Localization.LangKey;
		}

		public static void LoadElemValueIfKeyIsNotNullOrWhiteSpace() {
			if (string.IsNullOrWhiteSpace(elemKey)) {
				return;
			}

			_elemValue = Localization.GetLocalizedValue(elemKey, true);
		}

		private void OnGUI() {
			GUILayout.BeginHorizontal("box");
			_langKey = EditorGUILayout.TextField("Lang Key: ", _langKey);
			GUILayout.Space(64f);
			if (GUILayout.Button("Load", GUILayout.MaxWidth(128f))) {
				Localization.LangKey = _langKey;

				LoadElemValueIfKeyIsNotNullOrWhiteSpace();
			}
			if (GUILayout.Button("Save", GUILayout.MaxWidth(128f)) && EditorUtility.DisplayDialog("Confirmation", "Are you sure?", "yes", "no")) {
				Loader.Save();
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(8f);

			GUILayout.BeginVertical("window");
			GUILayout.BeginHorizontal("box");
			elemKey = EditorGUILayout.TextField("Element Key: ", elemKey);
			GUILayout.Space(64f);
			if (GUILayout.Button("Search", GUILayout.MaxWidth(64f))) {
				SearchWindow.Open(elemKey, null);
			}
			if (GUILayout.Button("Load", GUILayout.MaxWidth(64f))) {
				LoadElemValueIfKeyIsNotNullOrWhiteSpace();
			}
			if (GUILayout.Button("Set", GUILayout.MaxWidth(64f)) && !string.IsNullOrWhiteSpace(elemKey)) {
				Localization.dict[elemKey] = _elemValue;
				elemKey = string.Empty;
				_elemValue = string.Empty;
			}
			if (GUILayout.Button("Remove", GUILayout.MaxWidth(64f)) && EditorUtility.DisplayDialog("Confirmation", "Are you sure?", "yes", "no") &&
				!string.IsNullOrWhiteSpace(elemKey)) {
				_ = Localization.dict.Remove(elemKey);
			}
			GUILayout.EndHorizontal();
			EditorStyles.textArea.wordWrap = true;
			_elemValue = GUILayout.TextArea(_elemValue, EditorStyles.textArea, GUILayout.Height(256f));
			GUILayout.EndVertical();
		}
	}
}
