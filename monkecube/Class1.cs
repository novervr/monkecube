using System;
using System.IO;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using GorillaNetworking;
using UnityEngine.Networking;
using Photon.Pun;

namespace KnownIssues
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Class1 : BaseUnityPlugin
    {
        private const string modGUID = "KnownIssues";
        private const string modName = "KnownIssues";
        private const string modVersion = "1.0.1";

        private const bool localIsBanned = false;

        // When the script is started do Awake() \\
        public void Awake()
        {
            Debug.Log("Known Issues mod has been sucessfully read by BepInEx!");
            // Harmony Patches \\
            var harmony = new Harmony(modGUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [HarmonyPatch(typeof(GorillaLocomotion.Player))]
        [HarmonyPatch("FixedUpdate", MethodType.Normal)]
        class MainPatch
        {
            static void Prefix(GorillaLocomotion.Player __instance)
            {
                try
                {
                    for (int i = 0; i < GorillaComputer.instance.levelScreens.Length; i++)
                    {
                        Material colorr = new Material(Shader.Find("Standard"));
                        colorr.color = Color.cyan;
                        // Find all text elements in Level/lower level/UI for Code of Conduct and the color for \\
                        GameObject.Find("Level/lower level/StaticUnlit/screen").GetComponent<Renderer>().material = colorr;
                        GameObject.Find("Level/lower level/UI/CodeOfConduct").GetComponent<Text>().text = "[<color=yellow>KNOWN ISSUES MOD</color>]";
                        GameObject.Find("Level/lower level/UI/CodeOfConduct/COC Text").GetComponent<Text>().text = "THE CURRENT BUG FOR THIS MOD CURRENTLY IS GETTING INFO FROM A GITHUB PAGE. PLEASE WAIT FOR V1.0.2 UNTIL THIS IS FIXED. THANK YOU! ALSO REPORT BUGS ON MY DISCORD!";
                    }

                    if (PhotonNetworkController.Instance.wrongVersion)
                    {
                        Debug.Log("KnownIssues has detected that the local player is not in latest version.");
                        Material colorrupdate = new Material(Shader.Find("Standard"));
                        colorrupdate.color = Color.red;
                        GameObject.Find("Level/lower level/StaticUnlit/screen").GetComponent<Renderer>().material = colorrupdate;
                        GameObject.Find("Level/lower level/UI/CodeOfConduct").GetComponent<Text>().text = "[<color=yellow>UPDATE</color>]";
                        GameObject.Find("Level/lower level/UI/CodeOfConduct/COC Text").GetComponent<Text>().text = "PLEASE UPDATE YOUR GAME. THIS MOD ONLY SHOWS THE BUGS IN NEWER UPDATES. THANK YOU.";
                    }

                    if (localIsBanned == true)
                    {
                        Debug.Log("KnownIssues has detected that the local player is not in latest version.");
                        Material colorrban = new Material(Shader.Find("Standard"));
                        colorrban.color = Color.black;
                        GameObject.Find("Level/lower level/StaticUnlit/screen").GetComponent<Renderer>().material = colorrban;
                        GameObject.Find("Level/lower level/UI/CodeOfConduct").GetComponent<Text>().text = "[<color=red>BANNED</color>]";
                        GameObject.Find("Level/lower level/UI/CodeOfConduct/COC Text").GetComponent<Text>().text = "DO NOT TRY TO BAN EVADE, BECAUSE ITS LIKELY YOU WILL GET BANNED ON YOUR BAN EVADING ACCOUNT. JUST WAIT OUT YOUR BAN!";
                    }

                    if (Application.internetReachability > NetworkReachability.NotReachable)
                    {
                        Debug.Log("KnownIssues has detected that the local player is not connected / unable to authenticate steam account.");
                        Material colorrwifi = new Material(Shader.Find("Standard"));
                        colorrwifi.color = Color.green;
                        GameObject.Find("Level/lower level/StaticUnlit/screen").GetComponent<Renderer>().material = colorrwifi;
                        GameObject.Find("Level/lower level/UI/CodeOfConduct").GetComponent<Text>().text = "[<color=white>CONNECT TO WIFI OR LAN.</color>]";
                        GameObject.Find("Level/lower level/UI/CodeOfConduct/COC Text").GetComponent<Text>().text = "PLEASE CONNECT TO YOUR WIFI / LAN IN ORDER FOR KNOWN ISSUES TO WORK, OR IF THIS IS WRONG YOU PROBABLY ARE NOT AUTHENTICATED TO YOUR STEAM / OCULUS PC APP.";
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error has been found. Writing the error onto KnownIssuesLog.txt.");
                    File.WriteAllText("KnownIssuesLog.txt", ex.ToString());
                }
            }
        }
    }
}
