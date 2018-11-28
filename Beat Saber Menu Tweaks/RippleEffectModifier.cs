using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IllusionPlugin;
using UnityEngine;
using System.Collections;


namespace Beat_Saber_Menu_Tweaks
{
    class RippleEffectModifier : MonoBehaviour
    {
        private MenuShockwave menuShockwave;
        private bool clickShockwaveOn = true;
        private bool loaded = false;

        private void Awake()
        {
            Plugin.Log("RippleEffectModifier GameObject created!", Plugin.LogLevel.DebugOnly);
            StartCoroutine(WaitForLoad());
        }
        
        private IEnumerator WaitForLoad()
        {
            while (!loaded)
            {
                menuShockwave = Resources.FindObjectsOfTypeAll<MenuShockwave>().FirstOrDefault();

                if (menuShockwave == null)
                    yield return new WaitForSeconds(0.01f);
                else
                {
                    Plugin.Log("Found MenuShockwave!", Plugin.LogLevel.DebugOnly);
                    loaded = true;
                }
            }
            Init();
        }

        private void Init()
        {
            Plugin.Log("RippleEffectModifier Initialized!", Plugin.LogLevel.DebugOnly);

            clickShockwaveOn = ModPrefs.GetBool("MenuTweaks", "ClickShockwaveEnabled", true, true);
        }

        public void Update()
        {
            if (menuShockwave == null && loaded)
            {
                menuShockwave = Resources.FindObjectsOfTypeAll<MenuShockwave>().FirstOrDefault();
            }
            else if (menuShockwave.enabled != clickShockwaveOn)
            {
                menuShockwave.enabled = clickShockwaveOn;
            }
        }
    }
}