using IllusionPlugin;
using UnityEngine.SceneManagement;
using CustomUI.BeatSaber;
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

        private StatsScreenTweaks statsScreenTweaks;
        private RippleEffectModifier rippleEffectModifier;

        private BoolViewController failCounterToggle;
        private BoolViewController disableMenuShockwave;

        private static bool debug = true;

        public enum LogLevel
        {
            DebugOnly = 0,
            Info = 1,
            Error = 2
        }

        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManagerOnActiveSceneChanged(Scene sceneOld, Scene sceneNew)
        {
            if (sceneNew.name != "Menu")
            {
                return;
            }

            Plugin.Log("Menu Tweaks Started!", LogLevel.DebugOnly);
            var submenu = SettingsUI.CreateSubMenu("Menu Tweaks");
            failCounterToggle = submenu.AddBool("Fail Counter");
            failCounterToggle.GetValue += delegate { return ModPrefs.GetBool("MenuTweaks", "FailCounterVisible", false, true); };
            failCounterToggle.SetValue += delegate (bool value) { ModPrefs.SetBool("MenuTweaks", "FailCounterVisible", value); };

            disableMenuShockwave = submenu.AddBool("Click Shockwave");
            disableMenuShockwave.GetValue += delegate { return ModPrefs.GetBool("MenuTweaks", "ClickShockwaveEnabled", false, true); };
            disableMenuShockwave.SetValue += delegate (bool value) { ModPrefs.SetBool("MenuTweaks", "ClickShockwaveEnabled", value); };

            // Create the MenuTweaks GameObject
            statsScreenTweaks = new GameObject("StatsScreenTweaks").AddComponent<StatsScreenTweaks>();
            statsScreenTweaks.enabled = true;

            rippleEffectModifier = new GameObject("RippleEffectModifier").AddComponent<RippleEffectModifier>();
            rippleEffectModifier.enabled = true;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnUpdate()
        {

        }

        public void OnFixedUpdate()
        {
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public static void Log(string input, Plugin.LogLevel logLvl)
        {
            if (logLvl >= LogLevel.Info || debug) Console.WriteLine("[MenuTweaks]: " + input);
        }
    }
}
