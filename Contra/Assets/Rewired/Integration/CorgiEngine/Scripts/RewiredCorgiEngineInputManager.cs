namespace Rewired.Integration.CorgiEngine {
    using UnityEngine;
    using MoreMountains.CorgiEngine;
    using MoreMountains.Tools;

    /// <summary>
	/// This persistent singleton handles the inputs and sends commands to the player.
	/// IMPORTANT : this script's Execution Order MUST be -100.
	/// You can define a script's execution order by clicking on the script's file and then clicking on the Execution Order button at the bottom right of the script's inspector.
	/// See https://docs.unity3d.com/Manual/class-ScriptExecution.html for more details
	/// </summary>
    [AddComponentMenu("Corgi Engine/Managers/Rewired Input Manager")]
    public class RewiredCorgiEngineInputManager : InputManager {

        private const string rewiredSystemPauseActionName = "SystemPause";
        private bool _initialized;
        private Rewired.Player _rewiredPlayer;
        private int _rewiredActionId_horizontal;
        private int _rewiredActionId_vertical;
        private int _rewiredActionId_secondaryHorizontal;
        private int _rewiredActionId_secondaryVertical;
        private int _rewiredActionId_shoot;
        private int[] _rewiredButtonIds;
        private int _rewiredSystemPauseButtonId;

        /// <summary>
        /// On Start we look for what mode to use, and initialize our axis and buttons
        /// </summary>
        protected override void Start() {
            base.Start();

            if(!ReInput.isReady) {
                Debug.LogError("Rewired: Rewired was not initialized. Setup could not be performed. A Rewired Input Manager must be in the scene and enabled. Falling back to default input handler.");
                return;
            }

            // Get the Rewired Id based on the PlayerID string
            _rewiredPlayer = ReInput.players.GetPlayer(PlayerID);
            if(_rewiredPlayer == null) {
                Debug.LogError("Rewired: No Rewired Player was found for the PlayerID string \"" + PlayerID + "\". Falling back to default input handler.");
                return;
            }

            _initialized = true;
        }

        /// <summary>
        /// Initializes the buttons. If you want to add more buttons, make sure to register them in the InintializeButtons method in the base InputManager class.
        /// </summary>
        protected override void InitializeButtons() {
            base.InitializeButtons();
            if(!ReInput.isReady) return;

            // Cache the Rewired Action Id integers instead of using strings for speed
            _rewiredButtonIds = new int[ButtonList.Count];
            for(int i = 0; i < _rewiredButtonIds.Length; i++) _rewiredButtonIds[i] = -1; // init to invalid
            for(int i = 0; i < _rewiredButtonIds.Length; i++) {
                string actionName = StripPlayerIdFromActionName(ButtonList[i].ButtonID); // strip PlayerId from the ButtonId (PlayerID_ActionName) to get the action name.
                if(string.IsNullOrEmpty(actionName)) continue;
                _rewiredButtonIds[i] = ReInput.mapping.GetActionId(actionName);
                // Find the Shoot action so we can reuse it instead of ShootAxis
                if(actionName.Equals("Shoot", System.StringComparison.OrdinalIgnoreCase)) {
                    _rewiredActionId_shoot = _rewiredButtonIds[i];
                }
            }
            _rewiredSystemPauseButtonId = ReInput.mapping.GetActionId(rewiredSystemPauseActionName);
        }

        /// <summary>
		/// Initializes the axis ids.
		/// </summary>
		protected override void InitializeAxis() {
            base.InitializeAxis();
            if(!ReInput.isReady) return;

            // Cache the Rewired Action Id integers instead of using strings for speed
            _rewiredActionId_horizontal = ReInput.mapping.GetActionId(StripPlayerIdFromActionName(_axisHorizontal));
            _rewiredActionId_vertical = ReInput.mapping.GetActionId(StripPlayerIdFromActionName(_axisVertical));
            _rewiredActionId_secondaryHorizontal = ReInput.mapping.GetActionId(StripPlayerIdFromActionName(_axisSecondaryHorizontal));
            _rewiredActionId_secondaryVertical = ReInput.mapping.GetActionId(StripPlayerIdFromActionName(_axisSecondaryVertical));
        }

        /// <summary>
	    /// At update, we check the various commands and update our values and states accordingly.
	    /// </summary>
	    protected override void Update() {
            if(!_initialized) {
                base.Update();
                return;
            }
            SetMovement();
            SetSecondaryMovement();
            SetShootAxis();
            GetInputButtons();
        }

        /// <summary>
        /// Watches for input changes and updates our buttons states accordingly
        /// </summary>
        protected override void GetInputButtons() {
            if(!_initialized) {
                base.GetInputButtons();
                return;
            }
            for(int i = 0; i < _rewiredButtonIds.Length; i++) {
                if(_rewiredPlayer.GetButton(_rewiredButtonIds[i])) {
                    ButtonList[i].TriggerButtonPressed();
                }
                if(_rewiredPlayer.GetButtonDown(_rewiredButtonIds[i])) {
                    ButtonList[i].TriggerButtonDown();
                }
                if(_rewiredPlayer.GetButtonUp(_rewiredButtonIds[i])) {
                    ButtonList[i].TriggerButtonUp();
                }
            }

            // Special handling for ShootAxis which no longer exists because Rewired doesn't need special handling for these.
            if(_rewiredActionId_shoot >= 0) {
                ShootAxis = GetButtonState(_rewiredPlayer, _rewiredActionId_shoot);
            }

            // Special handling for System Pause
            // Allow the System Player to trigger Pause on all players so the key
            // only has to be mapped to one fixed key and the assignment can be protected.
            Rewired.Player systemPlayer = ReInput.players.GetSystemPlayer();
            if(systemPlayer.GetButtonDown(_rewiredSystemPauseButtonId)) {
                PauseButton.TriggerButtonDown();
            }
            if(systemPlayer.GetButtonUp(_rewiredSystemPauseButtonId)) {
                PauseButton.TriggerButtonUp();
            }
        }

        /// <summary>
		/// Called at LateUpdate(), this method processes the button states of all registered buttons
		/// </summary>
		public override void ProcessButtonStates() {
            base.ProcessButtonStates();
            if(!_initialized) return;

            // Update the ShootAxis state which is separate from other buttons
            if(ShootAxis == MMInput.ButtonStates.ButtonDown) {
                ShootAxis = MMInput.ButtonStates.ButtonPressed;
            }
            if(ShootAxis == MMInput.ButtonStates.ButtonUp) {
                ShootAxis = MMInput.ButtonStates.Off;
            }
        }

        /// <summary>
        /// Called every frame, gets primary movement values from Rewired Player
        /// </summary>
        public override void SetMovement() //Leo Monge: You need this code so the joystick detects absolute values.
        {
            if (!_initialized)
            {
                base.SetMovement();
                return;
            }
            if (SmoothMovement)
            {
                _primaryMovement.x = _rewiredPlayer.GetAxis(_rewiredActionId_horizontal);
                _primaryMovement.y = _rewiredPlayer.GetAxis(_rewiredActionId_vertical);

                // Normalize _primaryMovement
                _primaryMovement.x = Mathf.Abs(_primaryMovement.x) > 0.1f ? Mathf.Sign(_primaryMovement.x) : 0f;
                _primaryMovement.y = Mathf.Abs(_primaryMovement.y) > 0.1f ? Mathf.Sign(_primaryMovement.y) : 0f;
            }
            else
            {
                _primaryMovement.x = _rewiredPlayer.GetAxisRaw(_rewiredActionId_horizontal);
                _primaryMovement.y = _rewiredPlayer.GetAxisRaw(_rewiredActionId_vertical);

                // Normalize _primaryMovement
                _primaryMovement.x = _primaryMovement.x != 0f ? Mathf.Sign(_primaryMovement.x) : 0f;
                _primaryMovement.y = _primaryMovement.y != 0f ? Mathf.Sign(_primaryMovement.y) : 0f;
            }
        }

        /// <summary>
        /// Called every frame, gets secondary movement values from Rewired player
        /// </summary>
        public override void SetSecondaryMovement() {
            if(!_initialized) {
                base.SetSecondaryMovement();
                return;
            }
            if(SmoothMovement) {
                _secondaryMovement.x = _rewiredPlayer.GetAxis(_rewiredActionId_secondaryHorizontal);
                _secondaryMovement.y = _rewiredPlayer.GetAxis(_rewiredActionId_secondaryVertical);
            } else {
                _secondaryMovement.x = _rewiredPlayer.GetAxisRaw(_rewiredActionId_secondaryHorizontal);
                _secondaryMovement.y = _rewiredPlayer.GetAxisRaw(_rewiredActionId_secondaryVertical);
            }
        }

        /// <summary>
        /// Called every frame, gets shoot axis values from Rewired Player
        /// </summary>
        protected override void SetShootAxis() {
            if(!_initialized) {
                base.SetShootAxis();
                return;
            }
        }

        /// <summary>
        /// This is not used.
        /// </summary>
        /// <param name="movement">Movement.</param>
        public override void SetMovement(Vector2 movement) {
            if(!_initialized) base.SetMovement(movement);
        }

        /// <summary>
        /// This is not used.
        /// </summary>
        /// <param name="movement">Movement.</param>
        public override void SetSecondaryMovement(Vector2 movement) {
            if(!_initialized) base.SetSecondaryMovement(movement);
        }

        /// <summary>
        /// This is not used.
        /// </summary>
        /// <param name="">.</param>
        public override void SetHorizontalMovement(float horizontalInput) {
            if(!_initialized) base.SetHorizontalMovement(horizontalInput);
        }

        /// <summary>
        /// This is not used.
        /// </summary>
        /// <param name="">.</param>
        public override void SetVerticalMovement(float verticalInput) {
            if(!_initialized) base.SetVerticalMovement(verticalInput);
        }

        /// <summary>
        /// This is not used.
        /// </summary>
        /// <param name="">.</param>
        public override void SetSecondaryHorizontalMovement(float horizontalInput) {
            if(!_initialized) base.SetSecondaryHorizontalMovement(horizontalInput);
        }

        /// <summary>
        /// This is not used.
        /// </summary>
        /// <param name="">.</param>
        public override void SetSecondaryVerticalMovement(float verticalInput) {
            if(!_initialized) base.SetSecondaryVerticalMovement(verticalInput);
        }

        /// <summary>
        /// Gets the action name from a string that combines the PlayerID and the action name.
        /// </summary>
        /// <param name="action">The action string with PlayerID prefix.</param>
        /// <returns>The action string without the PlayerID prefix.</returns>
        private string StripPlayerIdFromActionName(string action) {
            if(string.IsNullOrEmpty(action)) return string.Empty;
            if(!action.StartsWith(PlayerID)) return action;
            return action.Substring(PlayerID.Length + 1); // strip PlayerID and underscore
        }

        /// <summary>
        /// Converts button input into MMInput.ButtonStates.
        /// </summary>
        /// <param name="player">The Rewired Player.</param>
        /// <param name="actionId">The Action Id.</param>
        /// <returns>Button state</returns>
        private static MMInput.ButtonStates GetButtonState(Rewired.Player player, int actionId) {
            MMInput.ButtonStates state = MMInput.ButtonStates.Off;
            if(player.GetButton(actionId)) state = MMInput.ButtonStates.ButtonPressed;
            if(player.GetButtonDown(actionId)) state = MMInput.ButtonStates.ButtonDown;
            if(player.GetButtonUp(actionId)) state = MMInput.ButtonStates.ButtonUp;
            return state;
        }

        /// <summary>
        /// Gets the Rewired Action Id for an Action name string.
        /// </summary>
        /// <param name="actionName">The Action name.</param>
        /// <param name="warn">Log a warning if the Action does not exist?</param>
        /// <returns>Returns the Action id or -1 if the Action does not exist.</returns>
        public static int GetRewiredActionId(string actionName, bool warn) {
            if(string.IsNullOrEmpty(actionName)) return -1;
            int id = ReInput.mapping.GetActionId(actionName);
            if(id < 0 && warn) Debug.LogWarning("No Rewired Action found for Action name \"" + actionName + "\"");
            return id;
        }
    }
}