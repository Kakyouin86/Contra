using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Rewired.Integration.CorgiEngine {

    /// <summary>
    /// A very simple input manager to handle the demo character's input and make it move
    /// </summary>
    public class RewiredRogueDemoCharacterInputManager : MoreMountains.InventoryEngine.DemoCharacterInputManager {

        /// <summary>
        /// Handles the demo character movement input.
        /// </summary>
        protected override void HandleDemoCharacterInput() {
            if(_pause) {
                DemoCharacter.SetMovement(0, 0);
                return;
            }
            Rewired.Player player = ReInput.players.GetPlayer(0);
            DemoCharacter.SetMovement(player.GetAxis("Horizontal"), player.GetAxis("Vertical"));
        }
    }
}