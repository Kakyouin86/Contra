using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class AdditionalCharacterLadderOverride : CharacterLadder
{
    protected override void Initialization()
    {
        base.Initialization();
        CurrentLadderClimbingSpeed = Vector2.zero;
        _boxCollider = this.gameObject.GetComponentInParent<BoxCollider2D>();
        _colliders = new List<Collider2D>();
        _characterHandleWeapon = this.gameObject.GetComponentInParent<Character>()?.FindAbility<CharacterHandleWeapon>();
    }

    protected override void StartClimbing()
    {
        if (CurrentLadder.LadderPlatform != null)
        {
            if (AboveLadderPlatform()
                && (LowestLadder == HighestLadder))
            {
                return;
            }
        }

        // we rotate our character if requested
        if (ForceRightFacing)
        {
            _character.Face(Character.FacingDirections.Right);
        }

        SetClimbingState();

        // we set collisions
        _controller.CollisionsOn();

        if ((_characterHandleWeapon != null) && (!_characterHandleWeapon.CanShootFromLadders))
        {
            _characterHandleWeapon.ForceStop();
        }

        if (CurrentLadder.CenterCharacterOnLadder)
        {
            {
                if (CurrentLadder.LadderType == Ladder.LadderTypes.Horizontal)//Leo Monge. This is to set the horizontal ladders aligned with the player in Y (first sentence only).
                {
                    _controller.SetTransformPosition(new Vector3(_controller.transform.position.x, CurrentLadder.transform.position.y, _controller.transform.position.z));
                }
                else
                {
                    _controller.SetTransformPosition(new Vector3(CurrentLadder.transform.position.x, _controller.transform.position.y, _controller.transform.position.z));
                }
            }
        }
    }

    protected override void SetClimbingState()
    {
        // we set its state to LadderClimbing
        _movement.ChangeState(CharacterStates.MovementStates.LadderClimbing);
        // it can't move freely anymore
        //_condition.ChangeState(CharacterStates.CharacterConditions.ControlledMovement);//Leo Monge. This was NOT commented.
        // we initialize the ladder climbing speed to zero
        CurrentLadderClimbingSpeed = Vector2.zero;
        // we make sure the controller won't move
        _controller.SetHorizontalForce(0);
        _controller.SetVerticalForce(0);
        // we disable the gravity
        _controller.GravityActive(false);
    }

    protected override void Climbing()
    {
        // we disable the gravity
        _controller.GravityActive(false);

        if (CurrentLadder.LadderPlatform != null)
        {
            if (!AboveLadderPlatform())
            {
                _controller.CollisionsOn();
            }
        }
        else
        {
            _controller.CollisionsOn();
        }

        // we set the force according to the ladder climbing speed
        if (CurrentLadder.LadderType == Ladder.LadderTypes.Simple)
        {
            _controller.SetVerticalForce(_verticalInput * LadderClimbingSpeed);
            // we set the climbing speed state.
            CurrentLadderClimbingSpeed = Mathf.Abs(_verticalInput) * transform.up;
        }

        if (CurrentLadder.LadderType == Ladder.LadderTypes.BiDirectional)
        {
            _controller.SetHorizontalForce(_horizontalInput * LadderClimbingSpeed);
            _controller.SetVerticalForce(_verticalInput * LadderClimbingSpeed);
            CurrentLadderClimbingSpeed = Mathf.Abs(_horizontalInput) * transform.right;
            CurrentLadderClimbingSpeed += Mathf.Abs(_verticalInput) * (Vector2)transform.up;
        }

        if (CurrentLadder.LadderType == Ladder.LadderTypes.Horizontal)//Leo Monge. This is new. It's a new type of ladder to move the player horizontally when climbing a ladder.
        {
            _controller.SetHorizontalForce(_horizontalInput * LadderClimbingSpeed);
            // we set the climbing speed state.
            CurrentLadderClimbingSpeed = Mathf.Abs(_horizontalInput) * transform.right;
            _characterHorizontalMovement.AbilityPermitted = true;
        }
    }
}
