using UnityEngine;
using UnityEditor;

namespace Mewiof.LiteLocalization {

	public class SearchWindow : EditorWindow {

		public const int RESULT_LIMIT = 25;

		private static SearchWindow _instance;
		private static LocalizedStringDrawer _drawer;

		private static string _value;
		private static Vector2 _scroll;

		private static readonly System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> _searchResultList = new();
		private static int _i;
		private static System.Collections.Generic.KeyValuePair<string, string> _item;

		public static void Open(string elemKey, LocalizedStringDrawer drawer) {
			if (drawer != null) {
				EditWindow.CloseIfInstanceIsNotNull();
			}

			_instance = CreateInstance<SearchWindow>();
			_drawer = drawer;

			_value = !string.IsNullOrWhiteSpace(elemKey) ? elemKey.ToLower() : string.Empty;
			_scroll = Vector2.zero;

			Search();

			Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
			_instance.ShowAsDropDown(new(mousePos.x - 512f, mousePos.y + 16f, 16f, 16f), new Vector2(512f, 256f));
		}

		private static void Search() {
			_searchResultList.Clear();
			int i = 0;
			foreach (System.Collections.Generic.KeyValuePair<string, string> item in Localization.dict) {
				if (i >= RESULT_LIMIT) {
					break;
				}
				if (item.Key.ToLower().Contains(_value) || item.Value.ToLower().Contains(_value)) {
					_searchResultList.Add(item);
					i++;
				}
			}
		}

		private void OnGUI() {
			_ = EditorGUILayout.BeginHorizontal("box");
			_value = EditorGUILayout.TextField(_value);
			GUILayout.Space(64f);
			if (GUILayout.Button("Search")) {
				Search();
			}
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(8f);

			if (_searchResultList.Count == 0) {
				return;
			}

			_ = EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Key");
			GUILayout.Space(-64f);
			EditorGUILayout.LabelField("Value");
			GUILayout.Space(-64f);
			EditorGUILayout.LabelField("Result limit: " + RESULT_LIMIT);
			EditorGUILayout.EndHorizontal();

			_ = EditorGUILayout.BeginVertical();
			_scroll = EditorGUILayout.BeginScrollView(_scroll);
			for (_i = 0; _i < _searchResultList.Count; _i++) {
				_item = _searchResultList[_i];
				_ = EditorGUILayout.BeginHorizontal("box");
				EditorGUILayout.LabelField(_item.Key);
				GUILayout.Space(-64f);
				EditorGUILayout.LabelField(_item.Value, EditorStyles.wordWrappedLabel);
				if (GUILayout.Button("S", GUILayout.Width(32f), GUILayout.Height(18f))) {
					if (_drawer == null) {
						LocalizationWindow.elemKey = _item.Key;
						Close();
						LocalizationWindow.LoadElemValueIfKeyIsNotNullOrWhiteSpace();
					}
					else {
						_drawer.UpdateKey(_item.Key);
						Close();
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}
	}
}
