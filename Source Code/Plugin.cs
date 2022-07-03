using BepInEx;
using System;
using UnityEngine;
using Utilla;
using System.IO;
using System.Reflection;
using Random = UnityEngine.Random;

namespace GorillaCosmeticRandomizer
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        GameObject standObject;

        void Awake()
        {
            instance = this;
            Events.GameInitialized += OnGameInitialized;
        }

        public void Generate()
        {
            Transform allTheHats = standObject.transform.Find("headmodel");
            int oneHat = Random.Range(0, allTheHats.childCount);
            string thisHatName = "";
            for (int i = 0; i < allTheHats.childCount; i++)
            {
                bool enableThisHat = i == oneHat;
                allTheHats.GetChild(i).gameObject.SetActive(enableThisHat);
                if (enableThisHat)
                {
                    thisHatName = allTheHats.GetChild(i).name;
                }
            }

            standObject.transform.Find("Canvas/cosmetic").GetComponent<UnityEngine.UI.Text>().text = "COSMETIC: " + thisHatName.ToUpper() ;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GorillaCosmeticRandomizer.standobject");
            AssetBundle assetBundle = AssetBundle.LoadFromStream(manifestResourceStream);
            GameObject obj = assetBundle.LoadAsset<GameObject>("StandObject");
            GameObject tabletObject = Instantiate(obj);
            tabletObject.transform.position = new Vector3(-66.828f, 16.084f, -116.13f);
            tabletObject.transform.rotation = Quaternion.Euler(0f, 28.499f, 0f);
            tabletObject.transform.localScale = Vector3.one;
            tabletObject.transform.SetParent(null, false);

            tabletObject.transform.Find("Cube").gameObject.AddComponent<CosmeticButton>();

            standObject = tabletObject;

            standObject.transform.Find("Canvas/cosmetic").GetComponent<UnityEngine.UI.Text>().text = "COSMETIC: " + "UNKNOWN";

            var colliders = standObject.transform.GetComponentsInChildren<MeshCollider>();

            if (colliders != null)
            {
                foreach (var collider in colliders)
                {
                    collider.enabled = false;
                }
            }
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            var colliders = standObject.transform.GetComponentsInChildren<MeshCollider>();

            if (colliders != null)
            {
                foreach (var collider in colliders)
                {
                    collider.enabled = true;
                }
            }
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            var colliders = standObject.transform.GetComponentsInChildren<MeshCollider>();

            if (colliders != null)
            {
                foreach (var collider in colliders)
                {
                    collider.enabled = false;
                }
            }
        }
    }
}
