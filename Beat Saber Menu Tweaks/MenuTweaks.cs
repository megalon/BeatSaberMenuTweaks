using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;

namespace Beat_Saber_Menu_Tweaks
{
    class MenuTweaks : MonoBehaviour
    {

        private PlayerStatisticsViewController stats;
        private bool hideFailCounter = false;
        private TextMeshProUGUI _failedLevelsCountText;
        private string failedLevelsCountReplacementText = "???";

        private void Awake()
        {
            Plugin.Log("Menu Tweaks GameObject created!");
            StartCoroutine(WaitForLoad());
        }

        private IEnumerator WaitForLoad()
        {
            bool loaded = false;
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
            Plugin.Log("init is true!");
            if (!hideFailCounter)
            {
                _failedLevelsCountText = ReflectionUtil.GetPrivateField<TextMeshProUGUI>(stats, "_failedLevelsCountText");
                _failedLevelsCountText.text = "???";
                Plugin.Log("_failedLevelsCountText.text: " + _failedLevelsCountText.text);
                hideFailCounter = true;
                Plugin.Log("Disabled fail text?");
            }
        }

        public void Update()
        {
            Plugin.Log("Update called!");
            //if (hideFailCounter)
            {
                if (_failedLevelsCountText == null)
                {
                    if (stats == null)
                    {
                        stats = Resources.FindObjectsOfTypeAll<PlayerStatisticsViewController>().FirstOrDefault();
                        return;
                    }
                    _failedLevelsCountText = ReflectionUtil.GetPrivateField<TextMeshProUGUI>(stats, "_failedLevelsCountText");
                    return;
                }
                _failedLevelsCountText.text = failedLevelsCountReplacementText;
                _failedLevelsCountText.ForceMeshUpdate(true);
            }
        }

        public void OnFixedUpdate()
        {
        }
    }
}