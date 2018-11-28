using IllusionPlugin;
using UnityEngine;
using System.Linq;
using System.Collections;

namespace Beat_Saber_Menu_Tweaks
{
    public class FireworksTweaks : MonoBehaviour
    {
        private ResultsViewController resultsViewController;
        private FireworksController fireworksController;
        private bool isEnabled = true;
        private bool loaded = false;
        public bool leftGameCoreScene = false;

        private void Awake()
        {
            Plugin.Log("FireworksTweaks GameObject created!", Plugin.LogLevel.DebugOnly);
            StartCoroutine(WaitForLoad());
        }

        private IEnumerator WaitForLoad()
        {
            while (!loaded)
            {
                resultsViewController = Resources.FindObjectsOfTypeAll<ResultsViewController>().FirstOrDefault();
                fireworksController = ReflectionUtil.GetPrivateField<FireworksController>(resultsViewController, "_fireworksController");
                
                if (resultsViewController == null) {
                    Plugin.Log("resultsViewController is null!", Plugin.LogLevel.DebugOnly);
                    yield return new WaitForSeconds(0.01f);
                }
                else if (fireworksController == null)
                {
                    Plugin.Log("fireworksController is null!", Plugin.LogLevel.DebugOnly);
                    yield return new WaitForSeconds(0.01f);
                }
                else
                {
                    Plugin.Log("Found FireworksController!", Plugin.LogLevel.DebugOnly);
                    loaded = true;
                }
            }
            Init();
        }

        private void Init()
        {
            Plugin.Log("Initialized!", Plugin.LogLevel.DebugOnly);

            isEnabled = ModPrefs.GetBool("MenuTweaks", "FireworksEnabled", true, true);
        }

        public void Update()
        {
            //Plugin.Log("resultsViewController.enabled: " + resultsViewController.enabled, Plugin.LogLevel.DebugOnly);
            if (fireworksController == null && loaded)
            {
                if (resultsViewController == null)
                    resultsViewController = Resources.FindObjectsOfTypeAll<ResultsViewController>().FirstOrDefault();
                else
                    fireworksController = Resources.FindObjectsOfTypeAll<FireworksController>().FirstOrDefault();
            }
            else if (resultsViewController.enabled && leftGameCoreScene && fireworksController.enabled)
            {
                if (!isEnabled)
                {
                    fireworksController.enabled = false;
                    // Delete any fireworks that were generated before it could be disabled
                    var fireworkItemController = Resources.FindObjectsOfTypeAll<FireworkItemController>().FirstOrDefault();
                    GameObject.Destroy(fireworkItemController);
                }
            }
        }
    }
}
