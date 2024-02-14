using UnityEngine;
using System.Collections;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class AdditionalCharacterHandleWeaponOverride : CharacterHandleWeapon
{
    public override void ShootStart()
    {
        // if the Shoot action is enabled in the permissions, we continue, if not we do nothing. If the player is dead we do nothing.
        if (!AbilityAuthorized
            || (CurrentWeapon == null)
            || ((_condition.CurrentState != CharacterStates.CharacterConditions.Normal) && (_condition.CurrentState != CharacterStates.CharacterConditions.ControlledMovement)))
        {
            return;
        }

        if (!CanShootFromLadders && (_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing))
        {
            return;
        }

        //  if we've decided to buffer input, and if the weapon is in use right now
        if (BufferInput && (CurrentWeapon.WeaponState.CurrentState != Weapon.WeaponStates.WeaponIdle))
        {
            // if we're not already buffering, or if each new input extends the buffer, we turn our buffering state to true
            if (!_buffering || NewInputExtendsBuffer)
            {
                _buffering = true;
                _bufferEndsAt = Time.time + MaximumBufferDuration;
            }
        }

        if (_movement.CurrentState == CharacterStates.MovementStates.Rolling) //Leo Monge. It was just CurrentWeapon.WeaponInputStart(); //if (_movement.CurrentState == CharacterStates.MovementStates.Dashing)
        {
            CurrentWeapon.WeaponInputStop();//Leo Monge. This makes that if you are rolling, "Rolling", then it will STOP the gun from firing. Very important!
        }
        else
        {
            PlayAbilityStartFeedbacks(); //Leo Monge. This and the rest of the two lines are the ones originally here.
            MMCharacterEvent.Trigger(_character, MMCharacterEventTypes.HandleWeapon, MMCharacterEvent.Moments.Start);
            CurrentWeapon.WeaponInputStart();
        }
    }
}
