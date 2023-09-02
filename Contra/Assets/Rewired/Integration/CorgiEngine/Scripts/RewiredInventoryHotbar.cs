using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace Rewired.Integration.CorgiEngine {
    using MoreMountains.InventoryEngine;
    /// <summary>
    /// Special kind of inventory display, with a dedicated Action associated to it, to allow for shortcuts for use and equip
    /// </summary>
    public class RewiredInventoryHotbar : InventoryHotbar {

        [Header("Hotbar")]

        // Information attribute was changed to MMInformation and there is no way to detect the version of Corgi Engine with preprocessor directives, so remove all references to InformationAttribute.
        //[Information("Here you can define the Action your hotbar will listen to to activate the hotbar's action.", InformationAttribute.InformationType.Info, false)]
        [Tooltip("The key associated to the hotbar, that will trigger the action when pressed.")]
        public string HotbarAction;

        public int rewiredActionId_hotbarAction { get { return _rewiredActionId_hotbarAction; } }
        private int _rewiredActionId_hotbarAction = -1;

        protected override void Awake() {
            base.Awake();

            if(!ReInput.isReady) {
                Debug.LogError("Rewired: Rewired was not initialized. A Rewired Input Manager must be in the scene and enabled.");
                return;
            }

            // Cache action id
            _rewiredActionId_hotbarAction = RewiredCorgiEngineInputManager.GetRewiredActionId(HotbarAction, true);
        }
    }
}