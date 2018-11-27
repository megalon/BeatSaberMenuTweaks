using IllusionPlugin;
using UnityEngine;
using System.Linq;
using System.Collections;
using TMPro;

namespace Beat_Saber_Menu_Tweaks
{
    public class MenuTweaks : MonoBehaviour
    {
        private PlayerStatisticsViewController stats;
        private bool hideFailCounter = false;
        private bool loaded = false;
        private TextMeshProUGUI _failedLevelsCountText;
        private string failedLevelsCountReplacementText = "???";

        private void Awake()
        {
            Plugin.Log("Menu Tweaks GameObject created!");
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
                    Plugin.Log("Found PlayerStatisticsViewController!");
                    loaded = true;
                }
            }
            Init();
        }

        private void Init()
        {
            Plugin.Log("Init called for MenuTweaks!");
            hideFailCounter = ModPrefs.GetBool("MenuTweaks", "HideFailCounter", false, true);
        }

        public void Update()
        {
            Plugin.Log("CheckingText! hideFailCounter:" + hideFailCounter + " loaded:" + loaded);
            if (hideFailCounter && loaded)
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
                    Plugin.Log("Replacing the _failedLevelsCountText with: " + failedLevelsCountReplacementText);
                    _failedLevelsCountText.text = failedLevelsCountReplacementText;
                    _failedLevelsCountText.ForceMeshUpdate(true);
                } else
                {
                    Plugin.Log("_failedLevelsCountText should be: " + failedLevelsCountReplacementText);
                }
            }
        }
    }
}