using UnityEngine;
using UnityEditor;

namespace Mewiof.LiteLocalization {

	[CustomPropertyDrawer(typeof(LocalizedString))]
	public class LocalizedStringDrawer : PropertyDrawer {

		private Rect _valueRect;
		private Rect _foldoutRect;
		private bool _dropdown;
		private SerializedProperty _key;
		private string _newKey;
		private System.DateTime _now;
		private System.DateTime _lastValueUpdateTimestamp;
		private string _value;
		private GUIStyle _style;
		private float _height;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			if (_dropdown) {
				return _height + 32f;
			}

			return 16f;
		}

		public void UpdateKey(string key) {
			_newKey = key;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			_ = EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			position.width -= 48f;
			position.height = 18f;

			_valueRect = new(position);
			_valueRect.x -= 32f;
			_valueRect.width += 32f;

			_foldoutRect = new(position);
			_foldoutRect.width = 18f;
			_foldoutRect.x -= 4f;

			_dropdown = EditorGUI.Foldout(_foldoutRect, _dropdown, string.Empty);

			_key = property.FindPropertyRelative("key");
			if (_newKey != null) {
				_key.stringValue = _newKey;
				_newKey = null;
			}
			_key.stringValue = EditorGUI.TextField(position, _key.stringValue);

			position.x += position.width + 4f;
			position.width = 20f;
			position.height = 18f;

			if (GUI.Button(position, "S")) {
				SearchWindow.Open(null, this);
			}

			position.x += position.width + 4f;

			if (GUI.Button(position, "E")) {
				EditWindow.Open(_key.stringValue, Localization.GetLocalizedValue(_key.stringValue, true));
			}

			if (_dropdown) {
				_now = System.DateTime.Now;
				if (_value == null || (_now - _lastValueUpdateTimestamp).TotalSeconds >= 2.0) {
					_value = Localization.GetLocalizedValue(_key.stringValue, true);
					_lastValueUpdateTimestamp = _now;
				}
				_style = GUI.skin.box;
				_height = _style.CalcHeight(new GUIContent(_value), _valueRect.width);

				_valueRect.height = _height;
				_valueRect.y += 32f;
				EditorGUI.LabelField(_valueRect, _value, EditorStyles.wordWrappedLabel);
			}

			EditorGUI.EndProperty();
		}
	}
}
