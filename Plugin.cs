using System;
using System.IO;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using Utilla;
using Newtonsoft.Json;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;

namespace DaytimeAndNightTimeChanger
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static GameObject DaytimeAndNightTimeChanger;
        public static bool inModded = false;
        public static Plugin instance;
        public int OGTime;

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            DaytimeAndNightTimeChanger.SetActive(true);
            inModded = true;
            DaytimeAndNightTimeChanger.transform.position = new Vector3(-68.6002f, 11.3735f, -84.0571f);
            DaytimeAndNightTimeChanger.transform.rotation = Quaternion.Euler(0f, 250.0225f, 359.6218f);
            DaytimeAndNightTimeChanger.transform.localScale = new Vector3(.6f, .6f, .6f);
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            DaytimeAndNightTimeChanger.transform.position = new Vector3(0, 0, 0);
            DaytimeAndNightTimeChanger.SetActive(false);
            inModded = false;
            RestoreOGTime();
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            instance = this;
            LoadAssets();
            DaytimeAndNightTimeChanger = Instantiate(DaytimeAndNightTimeChanger);
            DaytimeAndNightTimeChanger.transform.position = new Vector3(0, 0, 0);
            DaytimeAndNightTimeChanger.SetActive(true);
            LoadButtons();
            GetOGTime();
        }

        void LoadButtons()
        {
            GameObject day = GameObject.Find("day");
            GameObject night = GameObject.Find("night");
            GameObject moon = GameObject.Find("Moon");
            GameObject sun = GameObject.Find("sun");
            GameObject sunset = GameObject.Find("sunset");
            GameObject Sun4Set = GameObject.Find("Sun4Set");
            day.AddComponent<SunButton>();
            night.AddComponent<MoonButton>();
            day.layer = 18;
            night.layer = 18;
            sun.AddComponent<SunButton>();
            moon.AddComponent<MoonButton>();
            sun.layer = 18;
            moon.layer = 18;
            sunset.AddComponent<SunsetButton>();
            Sun4Set.AddComponent<SunsetButton>();
            sunset.layer = 18;
            Sun4Set.layer = 18;
        }

        public static void LoadAssets()
        {
            AssetBundle bundle = LoadAssetBundle("DaytimeAndNightTimeChanger.daytimeandnightime");
            DaytimeAndNightTimeChanger = bundle.LoadAsset<GameObject>("NIghtAndDay");
        }

        public static AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }

        void L(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }

        void Update()
        {
            if (!PhotonNetwork.InRoom)
            {
                DaytimeAndNightTimeChanger.SetActive(true);
                DaytimeAndNightTimeChanger.transform.position = new Vector3(-68.6002f, 11.3735f,-84.0571f);
                DaytimeAndNightTimeChanger.transform.rotation = Quaternion.Euler(0f, 250.0225f, 359.6218f);
                DaytimeAndNightTimeChanger.transform.localScale = new Vector3(.6f, .6f, .6f);
            }
            else if (!inModded)
            {
                DaytimeAndNightTimeChanger.transform.position = new Vector3(0, 0, 0);
                RestoreOGTime();
            }
        }

        public void ChangeToDay()
        {
            foreach (BetterDayNightManager t in GameObject.FindObjectsOfType<BetterDayNightManager>())
            {
                t.currentWeatherIndex = 3;
                t.SetTimeOfDay(4);
            }
        }

        public void ChangeToNight()
        {
            foreach (BetterDayNightManager t in GameObject.FindObjectsOfType<BetterDayNightManager>())
            {
                t.currentWeatherIndex = 3;
                t.SetTimeOfDay(8);
            }
        }
        
        public void ChangeToSunset()
        {
            foreach (BetterDayNightManager t in GameObject.FindObjectsOfType<BetterDayNightManager>())
            {
                t.currentWeatherIndex = 3;
                t.SetTimeOfDay(6);
            }
        }

        private void GetOGTime()
        {
            BetterDayNightManager dayNightManager = GameObject.FindObjectOfType<BetterDayNightManager>();
            if (dayNightManager != null)
            {
                OGTime = dayNightManager.currentTimeIndex;
            }
        }

        private void RestoreOGTime()
        {
            foreach (BetterDayNightManager t in GameObject.FindObjectsOfType<BetterDayNightManager>())
            {
                t.SetTimeOfDay(OGTime);
            }
        }
    }
}
