using UnityEngine;
using TMPro;

namespace Mewiof.LiteLocalization {

	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TextMeshProUGUILocalizer : LocalizerBase {

		private TextMeshProUGUI _text;

		protected override void UpdateValue() {
			_text.text = localizedString.Value;
		}

		private void SetComponent() {
			_text = GetComponent<TextMeshProUGUI>();
		}

		private void Start() {
			SetComponent();
			Init();
		}

#if UNITY_EDITOR
		private void OnValidate() {
			if (_text == null) {
				SetComponent();
			}

			if (localizedString.key != null) {
				_text.text = Localization.GetLocalizedValue(localizedString.key, true);
			}
		}
#endif
	}
}
