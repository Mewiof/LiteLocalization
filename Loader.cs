using System.Collections.Generic;
using UnityEngine;

namespace Mewiof.LiteLocalization {

	[System.Serializable]
	public struct LanguageElement {
		public string k;
		public string v;
	}

	[System.Serializable]
	public struct Language {
		public LanguageElement[] eArr;
	}

	public static class Loader {

		private static string _path;
		private static string _text;

		public static void LoadFile(string langKey) {
			_path = Localization.DirectoryName + '/' + Localization.FileNamePrefix + '_' + langKey;

#if UNITY_EDITOR
			string tempFilePath = "Assets/Resources/";
			string tempDirectoryPath = tempFilePath + Localization.DirectoryName;
			if (!System.IO.Directory.Exists(tempDirectoryPath)) {
				System.IO.Directory.CreateDirectory(tempDirectoryPath);
			}
			tempFilePath += _path + ".json";
			if (!System.IO.File.Exists(tempFilePath)) {
				System.IO.File.WriteAllText(tempFilePath, JsonUtility.ToJson(new Language { eArr = new LanguageElement[0] }, false), System.Text.Encoding.UTF8);
				UnityEditor.AssetDatabase.Refresh();
			}
#endif

			_text = Resources.Load<TextAsset>(_path).text;
		}

		public static Dictionary<string, string> GetDict() {
			Dictionary<string, string> result = new();

			if (_text == null) {
				Debug.LogError($"\"{nameof(_text)}\" is null");
				return result;
			}

			LanguageElement[] elementArr;
			try {
				elementArr = JsonUtility.FromJson<Language>(_text).eArr;
			}
			catch {
				Debug.LogError($"\"{nameof(_text)}\" is corrupted");
				return result;
			}
			LanguageElement element;

			for (int i = 0; i < elementArr.Length; i++) {
				element = elementArr[i];
				result[element.k] = element.v;
			}

			return result;
		}

#if UNITY_EDITOR
		public static void Save() {
			LanguageElement[] elementArr = new LanguageElement[Localization.dict.Count];

			int i = 0;
			foreach (KeyValuePair<string, string> keyValuePair in Localization.dict) {
				elementArr[i] = new LanguageElement {
					k = keyValuePair.Key,
					v = keyValuePair.Value
				};
				i++;
			}

			string tempFilePath = "Assets/Resources/";
			string tempDirectoryPath = tempFilePath + Localization.DirectoryName;
			if (!System.IO.Directory.Exists(tempDirectoryPath)) {
				System.IO.Directory.CreateDirectory(tempDirectoryPath);
			}
			tempFilePath += _path + ".json";
			System.IO.File.WriteAllText(tempFilePath, JsonUtility.ToJson(new Language { eArr = elementArr }, false), System.Text.Encoding.UTF8);
			UnityEditor.AssetDatabase.Refresh();
		}
#endif
	}
}
