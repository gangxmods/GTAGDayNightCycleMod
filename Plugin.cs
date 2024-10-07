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
        public static AudioClip keyboardClickClip;
        public static GameObject day;
        public static GameObject sun;
        public static GameObject moon;
        public static GameObject night;
        public static GameObject sunset;
        public static GameObject Sun4Set;
        public int OGTime;

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            inModded = true;
            DaytimeAndNightTimeChanger.SetActive(true);
            DaytimeAndNightTimeChanger.transform.position = new Vector3(-65.5877f, 11.3921f, -84.4808f);
            DaytimeAndNightTimeChanger.transform.rotation = Quaternion.Euler(0f, 176.5996f, 0f);
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
            day = GameObject.Find("day");
            night = GameObject.Find("night");
            moon = GameObject.Find("Moon");
            sun = GameObject.Find("sun");
            sunset = GameObject.Find("sunset");
            Sun4Set = GameObject.Find("Sun4Set");
            day.AddComponent<SunButton>();
            day.AddComponent<AudioSource>();
            night.AddComponent<MoonButton>();
            night.AddComponent<AudioSource>();
            day.layer = 18;
            night.layer = 18;
            sun.AddComponent<SunButton>();
            sun.AddComponent <AudioSource>();
            moon.AddComponent<MoonButton>();
            sun.layer = 18;
            moon.layer = 18;
            sunset.AddComponent<SunsetButton>();
            sunset.AddComponent<AudioSource>();
            Sun4Set.AddComponent<SunsetButton>();
            sunset.layer = 18;
            Sun4Set.layer = 18;
        }

        public static async void LoadAssets()
        {
            AssetBundle bundle = LoadAssetBundle("DaytimeAndNightTimeChanger.daytimeandnightime");
            DaytimeAndNightTimeChanger = bundle.LoadAsset<GameObject>("NIghtAndDay");
            GameObject T = bundle.LoadAsset<GameObject>("NIghtAndDay");
            keyboardClickClip = bundle.LoadAsset<AudioClip>("keyboardclick.mp3");
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
                DaytimeAndNightTimeChanger.transform.position = new Vector3(-65.5877f, 11.3921f, -84.4808f);
                DaytimeAndNightTimeChanger.transform.rotation = Quaternion.Euler(0f, 176.5996f, 0f);
                DaytimeAndNightTimeChanger.transform.localScale = new Vector3(.6f, .6f, .6f);
            }
            else if (!inModded)
            {
               // DaytimeAndNightTimeChanger.transform.position = new Vector3(0, 0, 0);
                RestoreOGTime();
            }
        }

        public void ChangeToDay()
        {
            day.GetComponent<AudioSource>().clip = keyboardClickClip;
            day.GetComponent<AudioSource>().Play();
            foreach (BetterDayNightManager t in GameObject.FindObjectsOfType<BetterDayNightManager>())
            {
                t.currentWeatherIndex = 3;
                t.SetTimeOfDay(4);
            }
        }

        public void ChangeToNight()
        {
            night.GetComponent<AudioSource>().clip = keyboardClickClip;
            night.GetComponent<AudioSource>().Play();
            foreach (BetterDayNightManager t in GameObject.FindObjectsOfType<BetterDayNightManager>())
            {
                t.currentWeatherIndex = 3;
                t.SetTimeOfDay(8);
            }
        }
        
        public void ChangeToSunset()
        {
            sunset.GetComponent<AudioSource>().clip = keyboardClickClip;
            sunset.GetComponent<AudioSource>().Play();
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
