﻿using IllusionPlugin;
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
        private BoolViewController hideFailsToggle;
        private MenuTweaks menuTweaks;

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

            Plugin.Log("Menu Tweaks Started!");
            var submenu = SettingsUI.CreateSubMenu("Menu Tweaks");
            hideFailsToggle = submenu.AddBool("Hide Fail Counter");
            hideFailsToggle.GetValue += delegate { return ModPrefs.GetBool("MenuTweaks", "HideFailCounter", false, true); };
            hideFailsToggle.SetValue += delegate (bool value) { ModPrefs.SetBool("MenuTweaks", "HideFailCounter", value); };

            Plugin.Log("Menu Tweaks GameObject created!");
            menuTweaks = new GameObject("MenuTweaks").AddComponent<MenuTweaks>();
            menuTweaks.enabled = true;
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

        public static void Log(string input)
        {
            Console.WriteLine("[MenuTweaks]: " + input);
        }
    }
}
