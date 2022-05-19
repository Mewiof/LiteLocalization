using UnityEngine;
using TMPro;

namespace Mewiof.Localization {

	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TextMeshProUGUILocalizer : MonoBehaviour {

		public LocalizedString localizedString;

		private TextMeshProUGUI _text;

		private void UpdateText() {
			_text.text = localizedString.Value;
		}

		private void Start() {
			_text = GetComponent<TextMeshProUGUI>();
			UpdateText();
			Localization.OnLangKeyChanged += UpdateText;
		}

#if UNITY_EDITOR
		private void OnValidate() {
			if (_text == null) {
				_text = GetComponent<TextMeshProUGUI>();
			}

			if (localizedString.key != null) {
				_text.text = Localization.GetLocalizedValue(localizedString.key, true);
			}
		}
#endif
	}
}
