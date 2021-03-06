using IllusionPlugin;
using UnityEngine;
using System.Linq;
using System.Collections;
using TMPro;

namespace Beat_Saber_Menu_Tweaks
{
    public class StatsScreenTweaks : MonoBehaviour
    {
        private PlayerStatisticsViewController stats;
        private bool showFailCounter = true;
        private bool loaded = false;
        private TextMeshProUGUI _failedLevelsCountText;
        private string failedLevelsCountReplacementText = "HIDDEN";

        private void Awake()
        {
            Plugin.Log("Menu Tweaks GameObject created!", Plugin.LogLevel.DebugOnly);
            StartCoroutine(WaitForLoad());
        }

        private IEnumerator WaitForLoad()
        {
            while (!loaded)
            {
                stats = Resources.FindObjectsOfTypeAll<PlayerStatisticsViewController>().FirstOrDefault();

                if (stats == null)
                    yield return new WaitForSeconds(0.01f);
                else
                {
                    Plugin.Log("Found PlayerStatisticsViewController!", Plugin.LogLevel.DebugOnly);
                    loaded = true;
                }
            }
            Init();
        }

        private void Init()
        {
            Plugin.Log("Initialized!", Plugin.LogLevel.DebugOnly);
            failedLevelsCountReplacementText = ModPrefs.GetString("MenuTweaks", "FailedCounterReplacementText", "HIDDEN", false);
            ModPrefs.SetString("MenuTweaks", "FailedLevelsReplacementText", failedLevelsCountReplacementText);

            showFailCounter = ModPrefs.GetBool("MenuTweaks", "FailCounterVisible", true, true);
        }

        public void Update()
        {
            if (!showFailCounter && loaded)
            {
                if (_failedLevelsCountText == null)
                {
                    if (stats == null)
                    {
                        stats = Resources.FindObjectsOfTypeAll<PlayerStatisticsViewController>().FirstOrDefault();
                    }
                    _failedLevelsCountText = ReflectionUtil.GetPrivateField<TextMeshProUGUI>(stats, "_failedLevelsCountText");
                }
                if (_failedLevelsCountText.text != failedLevelsCountReplacementText)
                {
                    Plugin.Log("Replacing the _failedLevelsCountText with: " + failedLevelsCountReplacementText, Plugin.LogLevel.Info);
                    _failedLevelsCountText.text = failedLevelsCountReplacementText;
                    _failedLevelsCountText.ForceMeshUpdate(true);
                }
            }
        }
    }
}