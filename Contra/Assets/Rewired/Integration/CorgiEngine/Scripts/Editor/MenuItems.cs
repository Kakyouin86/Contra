#if UNITY_2020 || UNITY_2021 || UNITY_2022 || UNITY_2023 || UNITY_2024 || UNITY_2025
#define UNITY_2020_PLUS
#endif

#if UNITY_2019 || UNITY_2020_PLUS
#define UNITY_2019_PLUS
#endif

#if UNITY_2018 || UNITY_2019_PLUS
#define UNITY_2018_PLUS
#endif

#if UNITY_2018_3_OR_NEWER || UNITY_2019_PLUS
#define UNITY_2018_3_PLUS
#endif

namespace Rewired.Integration.CorgiEngine.Editor {
    using UnityEngine;
    using UnityEditor;

    public static class MenuItems {

        private const string assetGuid_rewiredInputManager_1Player = "4db44ec8df022d246a2e41d329ae065a";
        private const string assetGuid_rewiredInputManager_4Player = "8d658c54dddae9d4c9f0f7e37e5be869";

        private const string assetGuid_rewiredCorgiEngineInputManager = "84cb1d069d7fb31409b668397ef0addf";
        private const int scriptExecutionOrder = -100;

        [MenuItem(Rewired.Consts.menuRoot + "/Integration/Corgi Engine/Setup/Run Setup")]
        private static void Setup() {

            string scriptName = typeof(RewiredCorgiEngineInputManager).Name;

            string path = AssetDatabase.GUIDToAssetPath(assetGuid_rewiredCorgiEngineInputManager);
            MonoScript ms = (MonoScript)AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript));
            if(ms == null) {
                Debug.LogError(scriptName + " script not found at expected GUID! Please delete and reinstall the integration pack.");
                return;
            }

            int order = MonoImporter.GetExecutionOrder(ms);
            if(order <= scriptExecutionOrder) {
                Debug.Log("Script execution order is already set to " + order);
                return;
            }

            try {
                MonoImporter.SetExecutionOrder(ms, scriptExecutionOrder);
                Debug.Log(scriptName + " script execution order set to " + scriptExecutionOrder);
            } catch {
                Debug.LogError("Failed to set script execution order on " + scriptName + ". Script execution order must be set manually or input will not function correctly. See the " + scriptName + ".cs file for information.");
            }
        }

        [MenuItem(Rewired.Consts.menuRoot + "/Integration/Corgi Engine/Create/Rewired Input Manager (1-Player)")]
        [MenuItem(Rewired.Consts.menuRoot + "/Create/Integration/Corgi Engine/Rewired Input Manager (1-Player)")]
        [MenuItem("GameObject/Create Other/Rewired/Integration/Corgi Engine/Rewired Input Manager (1-Player)")]
        public static void CreateInputManager1Player() {
            if(!InstantiatePrefabAtGuid(assetGuid_rewiredInputManager_1Player, "Rewired Input Manager (1-Player)", true)) {
                Debug.LogError("Unable to locate prefab file. Please reinstall the Corgi Engine integration pack.");
            }
        }

        [MenuItem(Rewired.Consts.menuRoot + "/Integration/Corgi Engine/Create/Rewired Input Manager (4-Player)")]
        [MenuItem(Rewired.Consts.menuRoot + "/Create/Integration/Corgi Engine/Rewired Input Manager (4-Player)")]
        [MenuItem("GameObject/Create Other/Rewired/Integration/Corgi Engine/Rewired Input Manager (4-Player)")]
        public static void CreateInputManager4Player() {
            if(!InstantiatePrefabAtGuid(assetGuid_rewiredInputManager_4Player, "Rewired Input Manager (4-Player)", true)) {
                Debug.LogError("Unable to locate prefab file. Please reinstall the Corgi Engine integration pack.");
            }
        }

        private static bool InstantiatePrefabAtGuid(string guid, string name, bool breakPrefabInstance) {
            GameObject prefab = LoadAssetAtGuid<GameObject>(guid);
            if(prefab == null) return false;

            GameObject instance = GameObject.Instantiate(prefab);
            if(instance == null) return false;

            if(!string.IsNullOrEmpty(name)) {
                instance.name = name;
            } else {
                // Strip (Clone) off the end of the name
                if(instance.name.EndsWith("(Clone)")) {
                    instance.name = instance.name.Substring(0, instance.name.Length - 7);
                }
            }

            if(breakPrefabInstance) {
#if UNITY_2018_3_PLUS
                try {
                    PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                } catch {
                    // Unity will throw an ArgumentException: UnpackPrefabInstance must be called wit ha Prefab instance.
                    // This is likely due to the fact that the prefab was created in Unity 5.6.3f1 and they did not account
                    // for running this function on the old prefab structure. This error cannot be "fixed" and simply has to
                    // be silenced in this catch statement.
                }
#else
                PrefabUtility.DisconnectPrefabInstance(instance);
#endif
            }
            Undo.RegisterCreatedObjectUndo(instance, "Create " + prefab.name);
            return true;
        }

        private static T LoadAssetAtGuid<T>(string guid) where T : Object {
            return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
        }
    }
}