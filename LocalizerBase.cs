using UnityEngine;

namespace Mewiof.LiteLocalization {

	public abstract class LocalizerBase : MonoBehaviour {

		public LocalizedString localizedString;

		protected abstract void UpdateValue();

		protected void Init() {
			UpdateValue();
			Localization.OnLangKeyChanged += UpdateValue;
		}
	}
}
