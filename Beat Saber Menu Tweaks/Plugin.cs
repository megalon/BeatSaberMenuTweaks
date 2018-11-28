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
        public string Name => "Menu Tweaks";
        public string Version => "1.1.0";

        private StatsScreenTweaks statsScreenTweaks;
        private RippleEffectModifier rippleEffectModifier;
        private FireworksTweaks fireworksTweaks;

        private BoolViewController failCounterToggle;
        private BoolViewController menuShockwaveToggle;
        private BoolViewController fireworksToggle;

        private static bool debug = false;

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
            Plugin.Log("sceneNew Name: " + sceneNew.name, LogLevel.DebugOnly);
            if (sceneNew.name != "Menu") return;

            // Create the tweaks GameObjects
            statsScreenTweaks = new GameObject("StatsScreenTweaks").AddComponent<StatsScreenTweaks>();
            statsScreenTweaks.enabled = true;

            rippleEffectModifier = new GameObject("RippleEffectModifier").AddComponent<RippleEffectModifier>();
            rippleEffectModifier.enabled = true;

            fireworksTweaks = new GameObject("FireworksTweaks").AddComponent<FireworksTweaks>();
            fireworksTweaks.enabled = true;
            if (sceneOld.name == "GameCore")
            {
                Log("Left GameCore scene", LogLevel.DebugOnly);
                fireworksTweaks.leftGameCoreScene = true;
            }
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name != "Menu") return;

            Plugin.Log("Menu Tweaks Started!", LogLevel.DebugOnly);

            // Create the settings menu
            var submenu = SettingsUI.CreateSubMenu("Menu Tweaks");

            // Add the Fail Counter toggle
            failCounterToggle = submenu.AddBool("Fail Counter");
            failCounterToggle.GetValue += delegate { return ModPrefs.GetBool("MenuTweaks", "FailCounterVisible", true, true); };
            failCounterToggle.SetValue += delegate (bool value) { ModPrefs.SetBool("MenuTweaks", "FailCounterVisible", value); };
            failCounterToggle.EnabledText = "VISIBLE";
            failCounterToggle.DisabledText = "HIDDEN";

            // Add the Click Shockwave toggle
            menuShockwaveToggle = submenu.AddBool("Click Shockwave");
            menuShockwaveToggle.GetValue += delegate { return ModPrefs.GetBool("MenuTweaks", "ClickShockwaveEnabled", true, true); };
            menuShockwaveToggle.SetValue += delegate (bool value) { ModPrefs.SetBool("MenuTweaks", "ClickShockwaveEnabled", value); };

            // Add the Fireworks toggle
            fireworksToggle = submenu.AddBool("Fireworks");
            fireworksToggle.GetValue += delegate { return ModPrefs.GetBool("MenuTweaks", "FireworksEnabled", true, true); };
            fireworksToggle.SetValue += delegate (bool value) { ModPrefs.SetBool("MenuTweaks", "FireworksEnabled", value); };
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
