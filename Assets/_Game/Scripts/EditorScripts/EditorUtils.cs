using UnityEditor;
using UnityEngine;

namespace EditorScripts
{
    public class EditorUtils
    {
#if UNITY_EDITOR
        [MenuItem("Custom/Clear All PlayerPrefs")]
        public static void ClearAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("All PlayerPrefs have been cleared.");
        }
#endif
    }
}