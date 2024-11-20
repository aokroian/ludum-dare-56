using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

namespace Localization
{
    public class LocalizationTest : MonoBehaviour
    {
        // [SerializeField] private LocalizedString localizedString;
        [SerializeField] private TableEntryReference tableEntryReference;
        
        [SerializeField] private LocalizeStringEvent stringLocalizer;
        [SerializeField] private Button[] buttons;
        [SerializeField] private VariablesGroupAsset globalVariables;
        [SerializeField] private LocalizedString zeroString;
        [SerializeField] private LocalizedString nonZeroString;
        

        private void Start()
        {
            Test();
            Test2();
            stringLocalizer.StringReference = ((IntVariable)globalVariables["matches"]).Value == 0 ? zeroString : nonZeroString;
            
            for (var i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                var i1 = i;
                button.onClick.AddListener(() =>
                {
                    stringLocalizer.StringReference = i1 == 0 ? zeroString : nonZeroString;
                    ((IntVariable)globalVariables["matches"]).Value = i1;
                    // stringLocalizer.StringReference.Arguments = new object[] { i1 + 1 };
                    stringLocalizer.RefreshString();
                    // stringLocalizer.StringReference = 
                });
            }
        }

        private async void Test()
        {
            var locale = await LocalizationSettings.SelectedLocaleAsync.Task;
            var text = await LocalizationSettings.StringDatabase.GetLocalizedStringAsync(tableEntryReference, locale).Task;
            Debug.LogWarning("Translation: " + text);
        }

        private void Test2()
        {
            Strings.Localize("TestStringKey", (result) => Debug.LogWarning("Translation: " + result));
        }
    }
}