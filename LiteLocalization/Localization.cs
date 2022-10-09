using System.Collections.Generic;
using UnityEngine;

namespace Mewiof.LiteLocalization {

	public static class Localization {

		public const string DirectoryName = "Localization";
		public const string FileNamePrefix = "local";
		public const string DefaultLangKey = "en";

		public static Dictionary<string, string> dict;

#if UNITY_EDITOR
		private static bool _initialized;
#endif

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
#if UNITY_EDITOR
			_initialized = true;
#endif
			if (_langKey == null) {
				LangKey = DefaultLangKey;
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
					Debug.LogWarning($"[{typeof(Localization).Name}] Failed to find item with the key \"{key}\"");
				}
				return string.Empty;
			}
			return result;
		}
	}
}
