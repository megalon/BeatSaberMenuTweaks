using IllusionPlugin;
using UnityEngine.SceneManagement;
using CustomUI.Settings;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using TMPro;

namespace Beat_Saber_Menu_Tweaks
{
    public class Plugin : IPlugin
    {
        public string Name => "Beat Saber Menu Tweaks";
        public string Version => "0.0.1";
        private static bool init = false;
        private static bool inMenu = false;
        public static bool hideFailCounter = false;
        private BoolViewController hideFailsToggle;
        private PlayerStatisticsViewController stats;
        private TextMeshProUGUI _failedLevelsCountText;
        private string failedLevelsCountReplacementText = "???";

        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name != "Menu")
            {
                inMenu = false;
                return;
            }

            inMenu = true;

            if (!init)
            {
                init = true;
                Plugin.Log("Menu Tweaks Started!");
                var submenu = SettingsUI.CreateSubMenu("Menu Tweaks");
                hideFailsToggle = submenu.AddBool("Hide Fail Counter");
                hideFailsToggle.GetValue += delegate { return ModPrefs.GetBool("MenuTweaks", "HideFailCounter", false, true); };
                hideFailsToggle.SetValue += delegate (bool value) { ModPrefs.SetBool("MenuTweaks", "HideFailCounter", value); hideFailCounter = value; };

                Plugin.Log("Menu Tweaks GameObject created!");
                SharedCoroutineStarter.instance.StartCoroutine(WaitForLoad());
            }
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
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

        public void Init()
        {
            init = true;
            Plugin.Log("init!");
            _failedLevelsCountText = ReflectionUtil.GetPrivateField<TextMeshProUGUI>(stats, "_failedLevelsCountText");
        }

        public void OnUpdate()
        {

        }

        public void OnFixedUpdate()
        {
        }
    }
}
