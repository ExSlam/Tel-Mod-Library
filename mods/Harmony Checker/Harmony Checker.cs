using HarmonyLib;
using UnityEngine;

namespace HarmonyChecker
{
    // change button text
    [HarmonyPatch(typeof(MainMenu_Buttons_Controller), "Start")]
    public class MainMenu_Buttons_Controller_Start
    {
        public const string BUTTON_LABEL = "IMHI_INSTALLED";

        public static void Postfix(ref MainMenu_Buttons_Controller __instance)
        {
            // Safety: some UI variants rename/remove the Mods button.
            // If that happens we should not throw here, because menu-time exceptions can prevent other mod hooks from running.
            if (__instance == null || __instance.Main_Container == null)
            {
                return;
            }

            Transform modsTransform = __instance.Main_Container.transform.Find("Mods");
            if (modsTransform == null)
            {
                return;
            }

            Lang_Button modButton = modsTransform.GetComponentInChildren<Lang_Button>();
            if (modButton == null)
            {
                return;
            }

            modButton.Constant = BUTTON_LABEL;
        }
    }
}
