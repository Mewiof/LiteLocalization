using System.Collections.Generic;
using UnityEngine;

namespace Mewiof.LiteLocalization {

	public static class Localization {

		public const string EnglishLangKey = "en";
		public const string RussianLangKey = "ru";

		public const string DirectoryName = "Localization";
		public const string FileNamePrefix = "local";

		public static Dictionary<string, string> dict;
		private static bool _initialized;

		public static event System.Action OnLangKeyChanged;

		private static string _langKey;
		public static string LangKey {
			get {
#if UNITY_EDITOR
				if (!_initialized) {
					Init();
				}
#endif

				return _langKey;
			}
			set {
				_langKey = value;

#if UNITY_EDITOR
				if (!_initialized) {
					Init();
				}
#endif

				Loader.LoadFile(_langKey);
				dict = Loader.GetDict();

				OnLangKeyChanged?.Invoke();
			}
		}

#if !UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
#endif
		private static void Init() {
			_initialized = true;
			if (_langKey == null) {
				LangKey = Application.systemLanguage == SystemLanguage.Russian ? RussianLangKey : EnglishLangKey;
			}
		}

		public static string GetLocalizedValue(string key, bool ignoreWarning = false) {
#if UNITY_EDITOR
			if (!_initialized) {
				Init();
			}
#endif

			if (!dict.TryGetValue(key, out string result)) {
				if (!ignoreWarning) {
					Debug.LogWarning($"Failed to find item with the key \"{key}\"");
				}
				return string.Empty;
			}
			return result;
		}
	}
}
