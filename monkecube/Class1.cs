using System;
using HarmonyLib;
using BepInEx;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.XR;

namespace monkecube
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Class1 : BaseUnityPlugin
    {
        private const string modGUID = "Monke Cube";
        private const string modName = "Monke Cube";
        private const string modVersion = "1.0.0.0";
        // On Awake() start the script.
        public void Awake()
        {
            var harmony = new Harmony(modGUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        // harmony patches for Assembly-CSharp.dll
        [HarmonyPatch(typeof(GorillaLocomotion.Player))]
        [HarmonyPatch("FixedUpdate", MethodType.Normal)]
        class MainPatch
        {
            static bool gripDown;
            static bool triggerDown;

            static GameObject cubeObject;
            static void Prefix(GorillaLocomotion.Player __instance)
            {
                try
                {
                    List<InputDevice> list = new List<InputDevice>();
                    InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, list);
                    list[0].TryGetFeatureValue(CommonUsages.gripButton, out gripDown);
                    list[0].TryGetFeatureValue(CommonUsages.triggerButton, out triggerDown);
                    if (gripDown && triggerDown)
                    {
                        DrawCube(__instance);
                    }
                    else
                    {
                        GameObject.Destroy(cubeObject);
                    }
                }
                catch
                {

                }
            }
            static void DrawCube(GorillaLocomotion.Player player)
            {
                if (cubeObject)
                    GameObject.Destroy(cubeObject);
                cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubeObject.transform.parent = player.leftControllerTransform;
                cubeObject.transform.position = player.leftControllerTransform.position;
                cubeObject.transform.rotation = player.leftControllerTransform.rotation;
                cubeObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                cubeObject.GetComponent<Renderer>().material.color = Color.cyan;

                GameObject.Destroy(cubeObject.GetComponent<Rigidbody>());
                GameObject.Destroy(cubeObject.GetComponent<Collider>());
            }
        }
    }
}
