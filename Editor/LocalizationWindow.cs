using UnityEngine;
using UnityEditor;

namespace Mewiof.LiteLocalization {

	public class LocalizationWindow : EditorWindow {

		public static string elemKey;
		private static bool _autoSave;
		private static string _langKey, _elemValue;

		private static readonly GUILayoutOption _maxWidth64 = GUILayout.MaxWidth(64f);
		private static readonly GUILayoutOption _maxWidth128 = GUILayout.MaxWidth(128f);

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

			_autoSave = EditorPrefs.GetBool(nameof(_autoSave));
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
			bool newAutoSaveValue = GUILayout.Toggle(_autoSave, "Auto-Save");
			if (newAutoSaveValue != _autoSave) {
				_autoSave = newAutoSaveValue;
				EditorPrefs.SetBool(nameof(_autoSave), _autoSave);
			}
			if (GUILayout.Button("Load", _maxWidth128)) {
				Localization.LangKey = _langKey;

				LoadElemValueIfKeyIsNotNullOrWhiteSpace();
			}
			if (GUILayout.Button("Save", _maxWidth128) && EditorUtility.DisplayDialog("Confirmation", "Are you sure?", "yes", "no")) {
				Loader.Save();
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(8f);

			GUILayout.BeginVertical("window");
			GUILayout.BeginHorizontal("box");
			elemKey = EditorGUILayout.TextField("Element Key: ", elemKey);
			GUILayout.Space(64f);
			if (GUILayout.Button("Search", _maxWidth64)) {
				SearchWindow.Open(elemKey, null);
			}
			if (GUILayout.Button("Load", _maxWidth64)) {
				LoadElemValueIfKeyIsNotNullOrWhiteSpace();
			}
			if (GUILayout.Button("Set", _maxWidth64) && !string.IsNullOrWhiteSpace(elemKey)) {
				Localization.dict[elemKey] = _elemValue;
				elemKey = string.Empty;
				_elemValue = string.Empty;
				if (_autoSave) {
					Loader.Save();
				}
			}
			if (GUILayout.Button("Remove", _maxWidth64) && EditorUtility.DisplayDialog("Confirmation", "Are you sure?", "yes", "no") &&
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
