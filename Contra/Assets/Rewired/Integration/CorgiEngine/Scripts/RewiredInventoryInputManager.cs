using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Rewired.Integration.CorgiEngine {
    using MoreMountains.InventoryEngine;

    public class RewiredInventoryInputManager : InventoryInputManager {

        [Header("Rewired Settings")]
        [Tooltip("The id of the Rewired Player that will be used to control this inventory.")]
        public int RewiredPlayerId = 0;

        [Header("Action Mapping")]
        // Information attribute was changed to MMInformation and there is no way to detect the version of Corgi Engine with preprocessor directives, so remove all references to InformationAttribute.
        //[Information("Here you need to set the various Action bindings. There are some by default but feel free to change them.", InformationAttribute.Type.Info, false)]
        [Tooltip("The Action used to open/close the inventory.")]
        public string ToggleInventoryAction = "InventoryToggle";
        [Tooltip("The Action used to move an item.")]
        public string MoveAction = "InventoryMove";
        [Tooltip("The Action used to equip an item.")]
        public string EquipAction = "InventoryEquip";
        [Tooltip("The Action used to use an item.")]
        public string UseAction = "InventoryUse";
        [Tooltip("The Action used to equip or use an item.")]
        public string EquipOrUseAction = "InventoryEquipOrUse";
        [Tooltip("The Action used to drop an item.")]
        public string DropAction = "InventoryDrop";
        [Tooltip("The Action used to go to the next inventory.")]
        public string NextAction = "InventoryNext";
        [Tooltip("The Action used to go to the previous inventory.")]
        public string PreviousAction = "InventoryPrev";

        private int _rewiredActionId_toggleInventory = -1;
        private int _rewiredActionId_move = -1;
        private int _rewiredActionId_equip = -1;
        private int _rewiredActionId_use = -1;
        private int _rewiredActionId_equipOrUse = -1;
        private int _rewiredActionId_drop = -1;
        private int _rewiredActionId_next = -1;
        private int _rewiredActionId_previous = -1;

        private Rewired.Player _rewiredPlayer;
        private bool _initialized;

        #region InventoryOpen Hack

        /// <summary>
        /// This is an ugly workaround for a breaking change in Corgi Engine.
        /// InventoryInputManager.InventoryOpen was changed to InventoryInputManager.InventoryIsOpen
        /// at some point. There is no way possible to branch code based on a change like this.
        /// Instead of forcing users to upgrade Corgi Engine in their project when Rewired is updated,
        /// and instead of having to support multiple branches of the Corgi Engine integration,
        /// I have chosen to use reflection to get the value of this variable. This is ugly, but
        /// there is no other way to handle this breaking change.
        /// </summary>
        [System.NonSerialized]
        private bool _inventoryOpenFieldChecked;
        [System.NonSerialized]
        private System.Reflection.FieldInfo _inventoryOpenFieldInfo;
        private bool isInventoryOpen {
            get {
                if(!_inventoryOpenFieldChecked) {
                    // Check which field exists
                    try {
                        _inventoryOpenFieldInfo = typeof(InventoryInputManager).GetField("InventoryOpen");
                    } catch {
                    }
                    if (_inventoryOpenFieldInfo == null) {
                        try {
                            _inventoryOpenFieldInfo = typeof(InventoryInputManager).GetField("InventoryIsOpen");
                        } catch {
                        }
                    }
                    if (_inventoryOpenFieldInfo == null) {
                        UnityEngine.Debug.Log("Error. InventoryOpen field format could not be determined. This must be an unsupported version of Corgi Engine and there has been another breaking change that is not supported.");
                    }
                    _inventoryOpenFieldChecked = true;
                }
                return _inventoryOpenFieldInfo != null ? (bool)_inventoryOpenFieldInfo.GetValue(this) : false;
            }
        }

        #endregion

        protected override void Start() {
            base.Start();
            InitializeRewired();
        }

        protected override void HandleInventoryInput() {
            if(!_initialized) return;
            // if we don't have a current inventory display, we do nothing and exit
            if(_currentInventoryDisplay == null) {
                return;
            }

            // if the user presses the 'toggle inventory' key
            if(_rewiredActionId_toggleInventory >= 0 && _rewiredPlayer.GetButtonDown(_rewiredActionId_toggleInventory)) {
                // if the inventory is not open
                if(!isInventoryOpen) {
                    OpenInventory();
                }
                // if it's open
                else {
                    CloseInventory();
                }
            }

            // if we've only authorized input when open, and if the inventory is currently closed, we do nothing and exit
            if(InputOnlyWhenOpen && !isInventoryOpen) {
                return;
            }

            // previous inventory panel
            if(_rewiredActionId_previous >= 0 &&_rewiredPlayer.GetButtonDown(_rewiredActionId_previous)) {
                if(_currentInventoryDisplay.GoToInventory(-1) != null) {
                    _currentInventoryDisplay = _currentInventoryDisplay.GoToInventory(-1);
                }
            }

            // next inventory panel
            if(_rewiredActionId_next >= 0 && _rewiredPlayer.GetButtonDown(_rewiredActionId_next)) {
                if(_currentInventoryDisplay.GoToInventory(1) != null) {
                    _currentInventoryDisplay = _currentInventoryDisplay.GoToInventory(1);
                }
            }

            // move
            if(_rewiredActionId_move >= 0 &&_rewiredPlayer.GetButtonDown(_rewiredActionId_move)) {
                if(CurrentlySelectedInventorySlot != null) {
                    CurrentlySelectedInventorySlot.Move();
                }
            }

            // equip or use
            if(_rewiredActionId_equipOrUse >= 0 && _rewiredPlayer.GetButtonDown(_rewiredActionId_equipOrUse)) {
                EquipOrUse();
            }

            // equip
            if(_rewiredActionId_equip >= 0 && _rewiredPlayer.GetButtonDown(_rewiredActionId_equip)) {
                if(CurrentlySelectedInventorySlot != null) {
                    CurrentlySelectedInventorySlot.Equip();
                }
            }

            // use
            if(_rewiredActionId_use >= 0 &&_rewiredPlayer.GetButtonDown(_rewiredActionId_use)) {
                if(CurrentlySelectedInventorySlot != null) {
                    CurrentlySelectedInventorySlot.Use();
                }
            }

            // drop
            if(_rewiredActionId_drop >= 0 && _rewiredPlayer.GetButtonDown(_rewiredActionId_drop)) {
                if(CurrentlySelectedInventorySlot != null) {
                    CurrentlySelectedInventorySlot.Drop();
                }
            }
        }

        protected override void HandleHotbarsInput() {
            if(!_initialized) return;
            if(!isInventoryOpen) {
                foreach(RewiredInventoryHotbar hotbar in _targetInventoryHotbars) {
                    if(hotbar != null) {
                        if(hotbar.rewiredActionId_hotbarAction >= 0 && _rewiredPlayer.GetButtonDown(hotbar.rewiredActionId_hotbarAction)) {
                            hotbar.Action();
                        }
                    }
                }
            }
        }

        private void InitializeRewired() {
            if(!ReInput.isReady) {
                Debug.LogError("Rewired: Rewired was not initialized. Setup could not be performed. A Rewired Input Manager must be in the scene and enabled. Falling back to default input handler.");
                return;
            }

            // Get the Rewired Id based on the PlayerID string
            _rewiredPlayer = ReInput.players.GetPlayer(RewiredPlayerId);
            if(_rewiredPlayer == null) {
                Debug.LogError("Rewired: No Rewired Player was found for id " + RewiredPlayerId + ".");
                return;
            }

            // Cache action ids
            _rewiredActionId_toggleInventory = RewiredCorgiEngineInputManager.GetRewiredActionId(ToggleInventoryAction, true);
            _rewiredActionId_move = RewiredCorgiEngineInputManager.GetRewiredActionId(MoveAction, true);
            _rewiredActionId_equip = RewiredCorgiEngineInputManager.GetRewiredActionId(EquipAction, true);
            _rewiredActionId_use = RewiredCorgiEngineInputManager.GetRewiredActionId(UseAction, true);
            _rewiredActionId_equipOrUse = RewiredCorgiEngineInputManager.GetRewiredActionId(EquipOrUseAction, true);
            _rewiredActionId_drop = RewiredCorgiEngineInputManager.GetRewiredActionId(DropAction, true);
            _rewiredActionId_next = RewiredCorgiEngineInputManager.GetRewiredActionId(NextAction, true);
            _rewiredActionId_previous = RewiredCorgiEngineInputManager.GetRewiredActionId(PreviousAction, true);

            _initialized = true;
        }
    }
}