using TMPro;
using UnityEditor;

namespace Editor
{
    public class ReplaceFont : ScriptableWizard
    {
        [MenuItem("Winter's Party Games/Replace Font")]
        public static void CreateWizard()
        {
            DisplayWizard<ReplaceFont>("Replace Font", "Replace");
        }

        public TMP_FontAsset font;

        public void OnWizardCreate()
        {
            var tmps = FindObjectsOfType<TMP_Text>();
            foreach (var tmp in tmps)
            {
                tmp.font = font;
            }
        }
    }
}