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
            DaytimeAndNightTimeChanger.transform.position = new Vector3(-67.4002f, 11.3408f, -80.6326f);
            DaytimeAndNightTimeChanger.transform.rotation = Quaternion.Euler(359.4726f, 331.1904f, 0f);
            DaytimeAndNightTimeChanger.transform.localScale = new Vector3(0.9f, 1f, 1f);
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            DaytimeAndNightTimeChanger.transform.position = new Vector3(0, 0, 0);
            DaytimeAndNightTimeChanger.SetActive(true);
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
            GameObject day = GameObject.Find("day");
            GameObject night = GameObject.Find("night");
            GameObject moon = GameObject.Find("Moon");
            GameObject sun = GameObject.Find("sun");
            day.AddComponent<SunButton>();
            night.AddComponent<MoonButton>();
            day.layer = 18;
            night.layer = 18;
            sun.AddComponent<SunButton>();
            moon.AddComponent<MoonButton>();
            sun.layer = 18;
            moon.layer = 18;
            GetOGTime();
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
                DaytimeAndNightTimeChanger.transform.position = new Vector3(-67.4002f, 11.3408f, -80.6326f);
                DaytimeAndNightTimeChanger.transform.rotation = Quaternion.Euler(359.4726f, 331.1904f, 0f);
                DaytimeAndNightTimeChanger.transform.localScale = new Vector3(0.9f, 1f, 1f);
            }
            else if (!inModded)
            {
                DaytimeAndNightTimeChanger.transform.position = new Vector3(0, 0, 0);
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
        
        public void GetOGTime()
        {
            BetterDayNightManager dayNightManager = GameObject.FindObjectOfType<BetterDayNightManager>();
            if (dayNightManager != null)
            {
                OGTime = dayNightManager.currentTimeIndex;
            }
        }
        
        public void RestoreOGTime()
        {
            foreach (BetterDayNightManager t in GameObject.FindObjectsOfType<BetterDayNightManager>())
            {
                t.SetTimeOfDay(OGTime);
            }
        }
    }
}
