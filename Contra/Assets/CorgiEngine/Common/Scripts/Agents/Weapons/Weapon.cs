﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using UnityEngine.Serialization;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
	/// <summary>
	/// A class used to store recoil properties, defining forces to apply on both ground and air, style, delay, and an associated feedback
	/// </summary>
	[System.Serializable]
	public class WeaponRecoilProperties
	{
		/// The force to apply to the Weapon owner if it's grounded when this recoil is triggered
		[Tooltip("The force to apply to the Weapon owner if it's grounded when this recoil is triggered")]
		public float RecoilForceGrounded = 10f;
		/// The force to apply to the Weapon owner if it's airborne when this recoil is triggered
		[Tooltip("The force to apply to the Weapon owner if it's airborne when this recoil is triggered")]
		public float RecoilForceAirborne = 10f;
		/// The rotation (in degrees) to apply to the recoil force
		[Tooltip("The rotation (in degrees) to apply to the recoil force")]
		public float RecoilAngleModifier = 0f;
		/// Multipliers to apply to the x and y components of the recoil force
		[Tooltip("Multipliers to apply to the x and y components of the recoil force")]
		public Vector2 DirectionMultiplier = Vector2.one;
		/// the chosen way to apply this recoil to the controller 
		[Tooltip("the chosen way to apply this recoil to the controller")]
		public DamageOnTouch.KnockbackStyles RecoilStyle = DamageOnTouch.KnockbackStyles.AddForce;
		/// the delay (in seconds) to apply before triggering the recoil
		[Tooltip("the delay (in seconds) to apply before triggering the recoil")]
		public float Delay = 0f;
		/// a feedback to play when this recoil triggers
		[Tooltip("a feedback to play when this recoil triggers")]
		public MMFeedbacks RecoilFeedback;
	}
    
	/// <summary>
	/// This base class, meant to be extended (see ProjectileWeapon.cs for an example of that) handles rate of fire (rate of use actually), and ammo reloading
	/// </summary>
	[SelectionBase]
	[MMRequiresConstantRepaint]
	public class Weapon : MMMonoBehaviour
    {
        /// the possible use modes for the trigger
        public enum TriggerModes { SemiAuto, Auto }
		/// the possible states the weapon can be in
		public enum WeaponStates { WeaponIdle, WeaponStart, WeaponDelayBeforeUse, WeaponUse, WeaponDelayBetweenUses, WeaponStop, WeaponReloadNeeded, WeaponReloadStart, WeaponReload, WeaponReloadStop, WeaponInterrupted, WeaponInCooldown }

		[MMInspectorGroup("General Settings", true, 12)]
        public Animator theAnimator; //Leo Monge: Need to ALWAYS bring it after update. This adds the Animator.
        public float customIntervalLeo = 0.5f; //Leo Monge
        public float intervalWaitFor; //Leo Monge
        public float counterIsShootingCounter = 0.0f; //Leo Monge
        /// is this weapon on semi or full auto ?
        [Tooltip("is this weapon on semi or full auto ?")]
		public TriggerModes TriggerMode = TriggerModes.Auto;
		/// whether or not this weapon can be interrupted 
		[Tooltip("whether or not this weapon can be interrupted ")]
		public bool Interruptable = false;
		/// If this is true, the weapon will initialize itself on start, otherwise it'll have to be init manually, usually by the CharacterHandleWeapon class
		[Tooltip("If this is true, the weapon will initialize itself on start, otherwise it'll have to be init manually, usually by the CharacterHandleWeapon class")]
		public bool InitializeOnStart = false;
		
		[Header("Delays")]
		/// the delay before use, that will be applied for every shot
		[Tooltip("the delay before use, that will be applied for every shot")]
		public float DelayBeforeUse = 0f;
		/// whether or not the delay before used can be interrupted by releasing the shoot button (if true, releasing the button will cancel the delayed shot)
		[Tooltip("whether or not the delay before used can be interrupted by releasing the shoot button (if true, releasing the button will cancel the delayed shot)")]
		public bool DelayBeforeUseReleaseInterruption = true;
		/// the time (in seconds) between two shots		
		[Tooltip("the time (in seconds) between two shots		")]
		public float TimeBetweenUses = 1f;
		/// whether or not the time between uses can be interrupted by releasing the shoot button (if true, releasing the button will cancel the time between uses)
		[Tooltip("whether or not the time between uses can be interrupted by releasing the shoot button (if true, releasing the button will cancel the time between uses)")]
		public bool TimeBetweenUsesReleaseInterruption = true;
		/// a duration, in seconds, at the end of the weapon's life cycle and before going back to Idle
		[Tooltip("a duration, in seconds, at the end of the weapon's life cycle and before going back to Idle")]
		public float CooldownDuration = 0f;
		
		[MMInspectorGroup("Burst Mode", true, 18)]
		/// if this is true, the weapon will activate repeatedly for every shoot request
		[Tooltip("if this is true, the weapon will activate repeatedly for every shoot request")]
		public bool UseBurstMode = false;
		/// the amount of 'shots' in a burst sequence
		[Tooltip("the amount of 'shots' in a burst sequence")]
		public int BurstLength = 3;
		/// the time between shots in a burst sequence (in seconds)
		[Tooltip("the time between shots in a burst sequence (in seconds)")]
		public float BurstTimeBetweenShots = 0.1f;

		[MMInspectorGroup("Magazine", true, 24)]

		/// whether or not the weapon is magazine based. If it's not, it'll just take its ammo inside a global pool
		[Tooltip("whether or not the weapon is magazine based. If it's not, it'll just take its ammo inside a global pool")]
		public bool MagazineBased = false;
		/// the size of the magazine
		[MMCondition("MagazineBased", true)]
		[Tooltip("the size of the magazine")]
		public int MagazineSize = 30;
		/// if this is true, pressing the fire button when a reload is needed will reload the weapon. Otherwise you'll need to press the reload button
		[MMCondition("MagazineBased", true)]
		[Tooltip("if this is true, pressing the fire button when a reload is needed will reload the weapon. Otherwise you'll need to press the reload button")]
		public bool AutoReload;
		/// the time it takes to reload the weapon
		[MMCondition("MagazineBased", true)]
		[Tooltip("the time it takes to reload the weapon")]
		public float ReloadTime = 2f;
		/// the amount of ammo consumed everytime the weapon fires
		[MMCondition("MagazineBased", true)]
		[Tooltip("the amount of ammo consumed everytime the weapon fires")]
		public int AmmoConsumedPerShot = 1;
		/// if this is set to true, the weapon will auto destroy when there's no ammo left
		[MMCondition("MagazineBased", true)]
		[Tooltip("if this is set to true, the weapon will auto destroy when there's no ammo left")]
		public bool AutoDestroyWhenEmpty;
		/// the delay (in seconds) before weapon destruction if empty
		[MMCondition("MagazineBased", true)]
		[Tooltip("the delay (in seconds) before weapon destruction if empty")]
		public float AutoDestroyWhenEmptyDelay = 1f;
		/// the current amount of ammo loaded inside the weapon
		[MMCondition("MagazineBased", true)]
		[MMReadOnly]
		[Tooltip("the current amount of ammo loaded inside the weapon")]
		public int CurrentAmmoLoaded = 0;

		[MMInspectorGroup("Position", true, 32)]

		/// an offset that will be applied to the weapon once attached to the center of the WeaponAttachment transform.
		[Tooltip("an offset that will be applied to the weapon once attached to the center of the WeaponAttachment transform.")]
		public Vector3 WeaponAttachmentOffset = Vector3.zero;
		/// should that weapon be flipped when the character flips ?
		[Tooltip("should that weapon be flipped when the character flips ?")]
		public bool FlipWeaponOnCharacterFlip = true;
		/// the FlipValue will be used to multiply the model's transform's localscale on flip. Usually it's -1,1,1, but feel free to change it to suit your model's specs
		[Tooltip("the FlipValue will be used to multiply the model's transform's localscale on flip. Usually it's -1,1,1, but feel free to change it to suit your model's specs")]
		public Vector3 FlipValue = new Vector3(-1,1,1);
		
		[MMInspectorGroup("Hands Positions", true, 60)]

		/// the transform to which the character's left hand should be attached to
		[Tooltip("the transform to which the character's left hand should be attached to")]
		public Transform LeftHandHandle;
		/// the transform to which the character's right hand should be attached to
		[Tooltip("the transform to which the character's right hand should be attached to")]
		public Transform RightHandHandle;

		[MMInspectorGroup("Movement", true, 28)]

		/// if this is true, a multiplier will be applied to movement while the weapon is equipped
		[Tooltip("if this is true, a multiplier will be applied to movement while the weapon is equipped")]
		public bool ModifyMovementWhileEquipped = false;
		/// the multiplier to apply to movement while equipped
		[Tooltip("the multiplier to apply to movement while equipped")]
		[MMCondition("ModifyMovementWhileEquipped", true)]
		public float PermanentMovementMultiplier = 0f;
		/// if this is true, a multiplier will be applied to movement while the weapon is active
		[Tooltip("if this is true, a multiplier will be applied to movement while the weapon is active")]
		public bool ModifyMovementWhileAttacking = false;
		/// the multiplier to apply to movement while attacking
		[Tooltip("the multiplier to apply to movement while attacking")]
		[MMCondition("ModifyMovementWhileAttacking", true)]
		public float MovementMultiplier = 0f;
		/// if this is true, movement will always be reset to initial speed after a multiplier stops being applied
		[Tooltip("if this is true, movement will always be reset to initial speed after a multiplier stops being applied")]
		public bool AlwaysResetMultiplierToInitial = true;
		/// if this is true all movement will be prevented (even flip) while the weapon is active
		[FormerlySerializedAs("PreventHorizontalMovementWhileInUse")] 
		[Tooltip("if this is true all movement will be prevented (even flip) while the weapon is active")]
		public bool PreventHorizontalGroundMovementWhileInUse = false;
		/// if this is true all horizontal air movement will be prevented (even flip) while the weapon is active
		[Tooltip("if this is true all horizontal air movement will be prevented (even flip) while the weapon is active")]
		public bool PreventHorizontalAirMovementWhileInUse = false;
		/// whether or not to apply a force when the weapon is in use
		[Tooltip("whether or not to set a force when the weapon is in use")]
		public bool SetForceWhileInUse = false;
		/// the force to apply when the weapon is in use, if SetForceWhileInUse is true
		[MMCondition("SetForceWhileInUse", true)]
		[Tooltip("the force to apply when the weapon is in use, if SetForceWhileInUse is true")]
		public Vector2 ForceWhileInUse =  Vector2.zero;
		/// whether or not to disable gravity while the weapon is in use
		[Tooltip("whether or not to disable gravity while the weapon is in use")]
		public bool DisableGravityWhileInUse = false;
		/// whether or not to disable flip while the weapon is in use
		[Tooltip("whether or not to disable flip while the weapon is in use")]
		public bool PreventFlipWhileInUse = false;


		[MMInspectorGroup("Animation", true, 20)]

		/// the other animators (other than the Character's) that you want to update every time this weapon gets used
		[Tooltip("the other animators (other than the Character's) that you want to update every time this weapon gets used")]
		public List<Animator> Animators;
		/// if this is true, the weapon's animator(s) will mirror the animation parameter of the owner character (that way your weapon's animator will be able to "know" if the character is walking, jumping, etc)
		[Tooltip("if this is true, the weapon's animator(s) will mirror the animation parameter of the owner character (that way your weapon's animator will be able to 'know' if the character is walking, jumping, etc)")]
		public bool MirrorCharacterAnimatorParameters = false;

		[MMInspectorGroup("Animation Parameters Names", true, 40)]

		/// the name of the parameter to send to true as long as this weapon is equipped, used or not. While all the other parameters defined here are updated by the Weapon class itself, and
		/// passed to the weapon and character, this one will be updated by CharacterHandleWeapon only.
		[Tooltip("the name of the parameter to send to true as long as this weapon is equipped, used or not. While all the other parameters defined here are updated by the Weapon class itself, and passed to the weapon and character, this one will be updated by CharacterHandleWeapon only.")]
		public string EquippedAnimationParameter;
		/// the name of the weapon's idle animation parameter : this will be true all the time except when the weapon is being used
		[Tooltip("the name of the weapon's idle animation parameter : this will be true all the time except when the weapon is being used")]
		public string IdleAnimationParameter;
		/// the name of the weapon's start animation parameter : true at the frame where the weapon starts being used
		[Tooltip("the name of the weapon's start animation parameter : true at the frame where the weapon starts being used")]
		public string StartAnimationParameter;
		/// the name of the weapon's delay before use animation parameter : true when the weapon has been activated but hasn't been used yet
		[Tooltip("the name of the weapon's delay before use animation parameter : true when the weapon has been activated but hasn't been used yet")]
		public string DelayBeforeUseAnimationParameter;
		/// the name of the weapon's single use animation parameter : true at each frame the weapon activates (shoots)
		[Tooltip("the name of the weapon's single use animation parameter : true at each frame the weapon activates (shoots)")]
		public string SingleUseAnimationParameter;
		/// the name of the weapon's in use animation parameter : true at each frame the weapon has started firing but hasn't stopped yet
		[Tooltip("the name of the weapon's in use animation parameter : true at each frame the weapon has started firing but hasn't stopped yet")]
		public string UseAnimationParameter;
		/// the name of the weapon's delay between each use animation parameter : true when the weapon is in use
		[Tooltip("the name of the weapon's delay between each use animation parameter : true when the weapon is in use")]
		public string DelayBetweenUsesAnimationParameter;
		/// the name of the weapon's in cooldown animation parameter : true when the weapon is in cooldown
		[Tooltip("the name of the weapon's in cooldown animation parameter : true when the weapon is in cooldown")]
		public string InCooldownAnimationParameter;
		/// the name of the weapon stop animation parameter : true after a shot and before the next one or the weapon's stop 
		[Tooltip("the name of the weapon stop animation parameter : true after a shot and before the next one or the weapon's stop ")]
		public string StopAnimationParameter;
		/// the name of the weapon reload start animation parameter
		[Tooltip("the name of the weapon reload start animation parameter")]
		public string ReloadStartAnimationParameter;
		/// the name of the weapon reload animation parameter
		[Tooltip("the name of the weapon reload animation parameter")]
		public string ReloadAnimationParameter;
		/// the name of the weapon reload end animation parameter
		[Tooltip("the name of the weapon reload end animation parameter")]
		public string ReloadStopAnimationParameter;
		/// the name of the weapon's angle animation parameter
		[Tooltip("the name of the weapon's angle animation parameter")]
		public string WeaponAngleAnimationParameter;
		/// the name of the weapon's angle animation parameter, adjusted so it's always relative to the direction the character is currently facing
		[Tooltip("the name of the weapon's angle animation parameter, adjusted so it's always relative to the direction the character is currently facing")]
		public string WeaponAngleRelativeAnimationParameter;
        
		[MMInspectorGroup("Feedbacks", true, 33)]

		/// the feedback to play when the weapon starts being used
		[Tooltip("the feedback to play when the weapon starts being used")]
		public MMFeedbacks WeaponStartMMFeedback;
		/// the feedback to play while the weapon is in use
		[Tooltip("the feedback to play while the weapon is in use")]
		public MMFeedbacks WeaponUsedMMFeedback;
		/// the feedback to play when the weapon stops being used
		[Tooltip("the feedback to play when the weapon stops being used")]
		public MMFeedbacks WeaponStopMMFeedback;
		/// the feedback to play when the weapon gets reloaded
		[Tooltip("the feedback to play when the weapon gets reloaded")]
		public MMFeedbacks WeaponReloadMMFeedback;
		/// the feedback to play when the weapon gets reloaded
		[Tooltip("the feedback to play when the weapon gets reloaded")]
		public MMFeedbacks WeaponReloadNeededMMFeedback;

		/// A MMFeedback to play when the weapon hits anything (damageable or not) 
		[Tooltip("A MMFeedback to play when the weapon hits anything (damageable or not)")]
		public MMFeedbacks WeaponOnHitFeedback;
		/// A MMFeedback to play when the weapon misses (what constitutes a miss is defined per Weapon subclass)
		[Tooltip("A MMFeedback to play when the weapon misses (what constitutes a miss is defined per Weapon subclass)")]
		public MMFeedbacks WeaponOnMissFeedback;
		/// A MMFeedback to play when the weapon hits a damageable
		[Tooltip("A MMFeedback to play when the weapon hits a damageable")]
		public MMFeedbacks WeaponOnHitDamageableFeedback;
		/// A MMFeedback to play when the weapon hits a non damageable object
		[Tooltip("A MMFeedback to play when the weapon hits a non damageable object")]
		public MMFeedbacks WeaponOnHitNonDamageableFeedback;
		/// A MMFeedback to play when the weapon kills something
		[Tooltip("A MMFeedback to play when the weapon kills something")]
		public MMFeedbacks WeaponOnKillFeedback;

		[MMInspectorGroup("Recoil", true, 33)]
        
		/// Whether or not to apply recoil to the Weapon owner every time this weapon gets used, regardless of the outcome
		[Tooltip("Whether or not to apply recoil to the Weapon owner every time this weapon gets used, regardless of the outcome")]
		public bool ApplyRecoilOnUse = false;
		/// The recoil to apply every time this weapon gets used
		[MMCondition("ApplyRecoilOnUse", true)]
		public WeaponRecoilProperties RecoilOnUseProperties;

		/// Whether or not to apply recoil to the Weapon owner every time this weapon hits a damageable object (an object with a Health component, basically)
		[Tooltip("Whether or not to apply recoil to the Weapon owner every time this weapon hits a damageable object (an object with a Health component, basically)")]
		public bool ApplyRecoilOnHitDamageable = false;
		/// the recoil to apply when this weapon hits a damageable 
		[MMCondition("ApplyRecoilOnHitDamageable", true)]
		public WeaponRecoilProperties RecoilOnHitDamageableProperties;

		/// Whether or not to apply recoil to the Weapon owner every time this weapon hits a non damageable object (a platform, prop, etc)
		[Tooltip("Whether or not to apply recoil to the Weapon owner every time this weapon hits a non damageable object (a platform, prop, etc)")]
		public bool ApplyRecoilOnHitNonDamageable = false;
		/// the recoil to apply when this weapon hits a non damageable
		[MMCondition("ApplyRecoilOnHitNonDamageable", true)]
		public WeaponRecoilProperties RecoilOnHitNonDamageableProperties;

		/// Whether or not to apply recoil to the Weapon owner every time the weapon misses its hit
		[Tooltip("Whether or not to apply recoil to the Weapon owner every time the weapon misses its hit")]
		public bool ApplyRecoilOnMiss = false; 
		/// the recoil to apply when this weapon hits nothing
		[MMCondition("ApplyRecoilOnMiss", true)]
		public WeaponRecoilProperties RecoilOnMissProperties;

		/// Whether or not to apply recoil to the Weapon owner every time the weapon kills its target
		[Tooltip("Whether or not to apply recoil to the Weapon owner every time the weapon kills its target")]
		public bool ApplyRecoilOnKill = false;
		/// The recoil to apply on kill
		[MMCondition("ApplyRecoilOnKill", true)]
		public WeaponRecoilProperties RecoilOnKillProperties;

		// ---------------------------------------------------------------------------------------------------------------------------

		/// the name of the inventory item corresponding to this weapon. Automatically set (if needed) by InventoryEngineWeapon
		public string WeaponID { get; set; }
		/// the weapon's owner
		public Character Owner { get; protected set; }
		/// the weapon's owner's CharacterHandleWeapon component
		public CharacterHandleWeapon CharacterHandleWeapon {get; set;}
		/// if true, the weapon is flipped
		public bool Flipped { get; set; }
		/// the WeaponAmmo component optionnally associated to this weapon
		public WeaponAmmo WeaponAmmo { get; protected set; }
		/// the weapon's state machine
		public MMStateMachine<WeaponStates> WeaponState;

		protected SpriteRenderer _spriteRenderer;
		protected CharacterGravity _characterGravity;
		protected CorgiController _controller;
		protected CharacterHorizontalMovement _characterHorizontalMovement;
		protected WeaponAim _aimableWeapon;
		protected float _permanentMovementMultiplierStorage = 1f;
		protected float _movementMultiplierStorage = 1f;
		protected Animator _ownerAnimator;

		protected float _delayBeforeUseCounter = 0f;
		protected float _delayBetweenUsesCounter = 0f;
		protected float _delayCooldownCounter = 0f;
		protected float _reloadingCounter = 0f;
		protected bool _triggerReleased = false;
		protected bool _reloading = false;
		protected ComboWeapon _comboWeapon;

		protected Vector3 _weaponOffset;
		protected Vector3 _weaponAttachmentOffset;
		protected Transform _weaponAttachment;
		protected List<HashSet<int>> _animatorParameters;
		protected HashSet<int> _ownerAnimatorParameters;
		protected bool _initialized = false;
		protected bool _applyForceWhileInUse = false;
		protected Vector2 _forceWhileInUse;
		protected bool _movementMultiplierNeedsResetting = false;

		// animation parameter
		protected const string _aliveAnimationParameterName = "Alive";
		protected int _equippedAnimationParameter;
		protected int _idleAnimationParameter;
		protected int _startAnimationParameter;
		protected int _delayBeforeUseAnimationParameter;
		protected int _singleUseAnimationParameter;
		protected int _useAnimationParameter;
		protected int _delayBetweenUsesAnimationParameter;
		protected int _inCooldownAnimationParameter;
		protected int _stopAnimationParameter;
		protected int _reloadStartAnimationParameter;
		protected int _reloadAnimationParameter;
		protected int _reloadStopAnimationParameter;
		protected int _weaponAngleAnimationParameter;
		protected int _weaponAngleRelativeAnimationParameter;
		protected int _aliveAnimationParameter;
		protected int _comboInProgressAnimationParameter;
		protected Vector2 _recoilDirection;
		protected bool _characterHorizontalMovementNotNull = false;
		protected bool _controllerNotNull = false;
		protected float _lastTurnWeaponOnAt = -float.MaxValue;
		protected bool _gravityBeforeUse = true;
		protected bool _canFlipBeforeUse = true;

		#region Initialization

		protected virtual void Start()
		{
            theAnimator = GameObject.FindWithTag("PlayerSprites").GetComponent<Animator>(); //Leo Monge: Need to ALWAYS bring it after update. This adds the Animator.
            if (InitializeOnStart)
			{
				Initialization();
			}
		}

		/// <summary>
		/// Initialize this weapon.
		/// </summary>
		public virtual void Initialization()
		{
			if (!_initialized)
			{
				Flipped = false;
				_spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
				_comboWeapon = this.gameObject.GetComponent<ComboWeapon>();
				WeaponState = new MMStateMachine<WeaponStates>(gameObject, true);
				_aimableWeapon = this.gameObject.GetComponent<WeaponAim>();
				WeaponAmmo = this.gameObject.GetComponent<WeaponAmmo>();
				_animatorParameters = new List<HashSet<int>>();
				InitializeAnimatorParameters();
				InitializeFeedbacks();
				_initialized = true;                
			}
			WeaponState.ChangeState(WeaponStates.WeaponIdle);
			if (WeaponAmmo == null)
			{
				CurrentAmmoLoaded = MagazineSize;
			}

			if (_characterHorizontalMovementNotNull && (this.enabled))
			{
				if (_characterHorizontalMovement.MovementSpeedMultiplier == 0f)
				{
					_characterHorizontalMovement.MovementSpeedMultiplier = 1f;
				}
				_permanentMovementMultiplierStorage = _characterHorizontalMovement.MovementSpeedMultiplier;
			}
            
			ResetMovementMultiplier();
		}
        
		/// <summary>
		/// Initializes all the feedbacks associated to this weapon
		/// </summary>
		protected virtual void InitializeFeedbacks()
		{
			WeaponStartMMFeedback?.Initialization(this.gameObject);
			WeaponUsedMMFeedback?.Initialization(this.gameObject);
			WeaponStopMMFeedback?.Initialization(this.gameObject);
			WeaponReloadNeededMMFeedback?.Initialization(this.gameObject);
			WeaponReloadMMFeedback?.Initialization(this.gameObject);
			WeaponOnHitFeedback?.Initialization(this.gameObject);
			WeaponOnMissFeedback?.Initialization(this.gameObject);
			WeaponOnHitDamageableFeedback?.Initialization(this.gameObject);
			WeaponOnHitNonDamageableFeedback?.Initialization(this.gameObject);
			WeaponOnKillFeedback?.Initialization(this.gameObject);
		}

		/// <summary>
		/// Initializes the combo comp if found
		/// </summary>
		public virtual void InitializeComboWeapons()
		{
			if (_comboWeapon != null)
			{
				_comboWeapon.Initialization();
			}
		}

		/// <summary>
		/// Sets the weapon's owner
		/// </summary>
		/// <param name="newOwner">New owner.</param>
		public virtual void SetOwner(Character newOwner, CharacterHandleWeapon handleWeapon)
		{
			Owner = newOwner;
			if (Owner != null)
			{
				CharacterHandleWeapon = handleWeapon;
				_characterGravity = Owner?.FindAbility<CharacterGravity>();
				_characterHorizontalMovement = Owner?.FindAbility<CharacterHorizontalMovement>();
				_characterHorizontalMovementNotNull = (_characterHorizontalMovement != null);
				_controller = Owner.GetComponent<CorgiController>();
				_controllerNotNull = (_controller != null);

				if (CharacterHandleWeapon.AutomaticallyBindAnimator)
				{
					if (CharacterHandleWeapon.CharacterAnimator != null)
					{
						_ownerAnimator = CharacterHandleWeapon.CharacterAnimator;
					}
					if (_ownerAnimator == null)
					{
						_ownerAnimator = CharacterHandleWeapon.gameObject.MMGetComponentNoAlloc<Character>().CharacterAnimator;
					}
					if (_ownerAnimator == null)
					{
						_ownerAnimator = CharacterHandleWeapon.gameObject.MMGetComponentNoAlloc<Animator>();
					}
				}
			}
		}

		#endregion Initialization
                
		/// <summary>
		/// On Update, we check if the weapon is or should be used
		/// </summary>
		protected virtual void Update()
		{
			ApplyOffset();
		}

		/// <summary>
		/// On LateUpdate, processes the weapon state
		/// </summary>
		protected virtual void LateUpdate()
		{
			UpdateAnimator();
			ProcessWeaponState();
		}

		#region Input

		/// <summary>
		/// Called by input, turns the weapon on
		/// </summary>
		public virtual void WeaponInputStart()
		{
			if (_reloading)
			{
				return;
			}
			if (WeaponState.CurrentState == WeaponStates.WeaponIdle)
			{
				_triggerReleased = false;
				TurnWeaponOn ();
			}
		}
		
		/// <summary>
		/// Describes what happens when the weapon's input gets released
		/// </summary>
		public virtual void WeaponInputReleased()
		{
			
		}

		/// <summary>
		/// Called by input, turns the weapon off if in auto mode
		/// </summary>
		public virtual void WeaponInputStop()
		{
			if (_reloading)
			{
				return;
			}
			_triggerReleased = true;
		}

		/// <summary>
		/// Describes what happens when the weapon starts
		/// </summary>
		public virtual void TurnWeaponOn()
		{
			if (Time.time - _lastTurnWeaponOnAt < TimeBetweenUses)
			{
				return;
			}

			_lastTurnWeaponOnAt = Time.time;
			
			TriggerWeaponStartFeedback();

			WeaponState.ChangeState(WeaponStates.WeaponStart);
			if ((_characterHorizontalMovementNotNull) && (ModifyMovementWhileAttacking))
			{
				_movementMultiplierStorage = _characterHorizontalMovement.MovementSpeedMultiplier;   
				_characterHorizontalMovement.MovementSpeedMultiplier = MovementMultiplier;
				_movementMultiplierNeedsResetting = true;
			}
			if (_comboWeapon != null)
			{
				_comboWeapon.WeaponStarted(this);
			}

			if (DisableGravityWhileInUse)
			{
				_gravityBeforeUse = _controller.IsGravityActive;
				_controller.GravityActive(false);
			}

			if (PreventFlipWhileInUse)
			{
				_canFlipBeforeUse = Owner.CanFlip;
				Owner.CanFlip = false;
			}
			if (SetForceWhileInUse || PreventHorizontalAirMovementWhileInUse || PreventHorizontalGroundMovementWhileInUse)
			{
				_applyForceWhileInUse = true;
				StartCoroutine(ApplyForceWhileInUseCo());
			}
		}

		/// <summary>
		/// Handles movement prevention in air or on ground
		/// </summary>
		protected virtual void PreventMovement()
		{
			if (PreventHorizontalGroundMovementWhileInUse && _characterHorizontalMovementNotNull && (_controller != null) && (_controller.State.IsGrounded))
			{
				_characterHorizontalMovement.SetHorizontalMove(0f);
				_characterHorizontalMovement.MovementForbidden = true;
			}
			if (PreventHorizontalAirMovementWhileInUse && _characterHorizontalMovementNotNull && (_controller != null) && (!_controller.State.IsGrounded))
			{
				_characterHorizontalMovement.SetHorizontalMove(0f);
				_characterHorizontalMovement.MovementForbidden = true;
			}
		}

		/// <summary>
		/// Turns the weapon off.
		/// </summary>
		public virtual void TurnWeaponOff()
		{
			if (_characterHorizontalMovementNotNull)
			{
				_characterHorizontalMovement.MovementSpeedMultiplier = _permanentMovementMultiplierStorage;
			}

			if ((WeaponState.CurrentState == WeaponStates.WeaponIdle || WeaponState.CurrentState == WeaponStates.WeaponStop))
			{
				return;
			}
			_triggerReleased = true;
			TriggerWeaponStopFeedback();
			WeaponState.ChangeState(WeaponStates.WeaponStop);
			if (_comboWeapon != null)
			{
				_comboWeapon.WeaponStopped(this);
			}

			RestoreMovement();
		}

		protected virtual void RestoreMovement()
		{
			if (PreventHorizontalGroundMovementWhileInUse && _characterHorizontalMovementNotNull)
			{
				_characterHorizontalMovement.MovementForbidden = false;
			}
			if (PreventHorizontalAirMovementWhileInUse && _characterHorizontalMovementNotNull)
			{
				_characterHorizontalMovement.MovementForbidden = false;
			}
			if (DisableGravityWhileInUse && _controllerNotNull)
			{
				_controller.GravityActive(_gravityBeforeUse);
			}
			if (PreventFlipWhileInUse)
			{
				Owner.CanFlip = _canFlipBeforeUse;
			}
			if (SetForceWhileInUse || PreventHorizontalAirMovementWhileInUse || PreventHorizontalGroundMovementWhileInUse)
			{
				_applyForceWhileInUse = false;
			}
		}

		/// <summary>
		/// Call this method to interrupt the weapon
		/// </summary>
		public virtual void Interrupt()
		{
			if (Interruptable)
			{
				WeaponState.ChangeState(WeaponStates.WeaponInterrupted);
			}
		}

		/// <summary>
		/// An internal coroutine used to apply a force while the weapon's in use
		/// </summary>
		/// <returns></returns>
		protected virtual IEnumerator ApplyForceWhileInUseCo()
		{
			while (_applyForceWhileInUse)
			{
				PreventMovement();

				if (SetForceWhileInUse)
				{
					_forceWhileInUse = ForceWhileInUse;
			        
					if (Owner != null)
					{
						_forceWhileInUse.x = Owner.IsFacingRight ? _forceWhileInUse.x : -_forceWhileInUse.x;
					}
		        
					_controller.SetForce(_forceWhileInUse);
				}
				
				yield return null;
			}
		}

		#endregion Input

		#region StateMachine

		/// <summary>
		/// Called every lastUpdate, processes the weapon's state machine
		/// </summary>
		protected virtual void ProcessWeaponState()
		{
			if (WeaponState == null) { return; }
            
			switch (WeaponState.CurrentState)
			{
				case WeaponStates.WeaponIdle:
					CaseWeaponIdle();
					break;

				case WeaponStates.WeaponStart:
					CaseWeaponStart();
					break;	

				case WeaponStates.WeaponDelayBeforeUse:
					CaseWeaponDelayBeforeUse();
					break;

				case WeaponStates.WeaponUse:
					CaseWeaponUse();
					break;

				case WeaponStates.WeaponDelayBetweenUses:
					CaseWeaponDelayBetweenUses();
					break;

				case WeaponStates.WeaponStop:
					CaseWeaponStop();
					break;
				
				case WeaponStates.WeaponInCooldown:
					CaseWeaponInCooldown();
					break;

				case WeaponStates.WeaponReloadNeeded:
					CaseWeaponReloadNeeded();
					break;

				case WeaponStates.WeaponReloadStart:
					CaseWeaponReloadStart();
					break;

				case WeaponStates.WeaponReload:
					CaseWeaponReload();
					break;

				case WeaponStates.WeaponReloadStop:
					CaseWeaponReloadStop();
					break;

				case WeaponStates.WeaponInterrupted:
					CaseWeaponInterrupted();
					break;
			}
		}

		protected virtual void CaseWeaponIdle()
		{
			ResetMovementMultiplier();
		}

		protected virtual void CaseWeaponStart()
		{
			if (DelayBeforeUse > 0)
			{
				_delayBeforeUseCounter = DelayBeforeUse;
				WeaponState.ChangeState(WeaponStates.WeaponDelayBeforeUse);
			}
			else
			{
				StartCoroutine(ShootRequestCo());
			}
		}

		protected virtual void CaseWeaponDelayBeforeUse()
		{
			_delayBeforeUseCounter -= Time.deltaTime;
			if (_delayBeforeUseCounter <= 0)
			{
				StartCoroutine(ShootRequestCo());
			}
		}

		protected virtual void CaseWeaponUse()
		{
			WeaponUse();
			_delayBetweenUsesCounter = TimeBetweenUses;
			WeaponState.ChangeState(WeaponStates.WeaponDelayBetweenUses);
		}

		protected virtual void CaseWeaponDelayBetweenUses()
		{
			if (_triggerReleased && TimeBetweenUsesReleaseInterruption)
			{
				TurnWeaponOff();
				return;
			}
			
			_delayBetweenUsesCounter -= Time.deltaTime;
			if (_delayBetweenUsesCounter <= 0)
			{
				if ((TriggerMode == TriggerModes.Auto) && !_triggerReleased)
				{
					StartCoroutine(ShootRequestCo());
				}
				else
				{
					TurnWeaponOff();
				}
			}
		}

		protected virtual void CaseWeaponStop()
		{
			_delayCooldownCounter = CooldownDuration;
			WeaponState.ChangeState(WeaponStates.WeaponInCooldown);
		}

		protected virtual void CaseWeaponInCooldown()
		{
			_delayCooldownCounter -= Time.deltaTime;
			if (_delayCooldownCounter <= 0)
			{
				WeaponState.ChangeState(WeaponStates.WeaponIdle);
			}
		}

		protected virtual void CaseWeaponReloadNeeded()
		{
			ReloadNeeded();
			TurnWeaponOff();
			ResetMovementMultiplier();
			WeaponState.ChangeState(WeaponStates.WeaponIdle);
		}

		protected virtual void CaseWeaponReloadStart()
		{
			ReloadWeapon();
			_reloadingCounter = ReloadTime;
			WeaponState.ChangeState(WeaponStates.WeaponReload);
		}

		protected virtual void CaseWeaponReload()
		{
			ResetMovementMultiplier();
			_reloadingCounter -= Time.deltaTime;
			if (_reloadingCounter <= 0)
			{
				WeaponState.ChangeState(WeaponStates.WeaponReloadStop);
			}
		}

		protected virtual void CaseWeaponReloadStop()
		{
			_reloading = false;
			WeaponState.ChangeState(WeaponStates.WeaponIdle);
			if (WeaponAmmo == null)
			{
				CurrentAmmoLoaded = MagazineSize;
			}
		}

		protected virtual void CaseWeaponInterrupted()
		{
			TurnWeaponOff();
			ResetMovementMultiplier();
			WeaponState.ChangeState(WeaponStates.WeaponIdle);
		}

		/// <summary>
		/// When the weapon is used, plays the corresponding sound
		/// </summary>
		protected virtual void WeaponUse()
		{
			TriggerWeaponUsedFeedback();
			ApplyRecoil(ApplyRecoilOnUse, RecoilOnUseProperties);   
		}

		#endregion StateMachine

		#region Permissions
		
		/// <summary>
		/// Determines whether or not the weapon can fire
		/// </summary>
		public virtual IEnumerator ShootRequestCo() //Leo Monge: Need to ALWAYS bring it after update. This whole thing should be edited because basically adds some "time" after shooting so it repeats the animation and adds a little timer so it finishes well the animations.
        {
            /*Inventory weaponInventory = GameObject.FindGameObjectWithTag("WeaponInventory").GetComponent<Inventory>();
            //float intervalLeo = 0.01f;
            float intervalLeo = 1f; // Original interval of 1 second.
            int remainingShots = UseBurstMode ? BurstLength : 1;
            float interval = UseBurstMode ? BurstTimeBetweenShots : intervalLeo;
            float counter = 0.00f;
            //float counter = 0.1f; // Counter for 0.1 seconds
            while (remainingShots > 0)
			{
                theAnimator.SetBool("isShooting", true);
                theAnimator.SetFloat("isShootingCounter", counter);
                ShootRequest();
				remainingShots--;
				yield return MMCoroutine.WaitFor(interval);
			}

            if (weaponInventory.Content[0].ItemName == "Flame Gun")
            {
                _aimableWeapon.IgnoreDownWhenGrounded = true;
                while (counter > 0)
                {
                    counter -= Time.deltaTime; // Decrease the counter
                    theAnimator.SetFloat("isShootingCounter", counter);
                    yield return null;
                }
            }

            theAnimator.SetBool("isShooting", false);
            TurnWeaponOff();*/
            
            //float customIntervalLeo = 0.5f; // Original interval of 1 second. Moved to 0.5. In 0.3 it creates bugs while shooting up.
            int remainingShots = UseBurstMode ? BurstLength : 1;
            intervalWaitFor = UseBurstMode ? BurstTimeBetweenShots : customIntervalLeo;
            //float counter = 0.0f; // Counter for 0.1 seconds
            while (remainingShots > 0) 
            {
                if (!gameObject.CompareTag("Untagged"))
                {
                    theAnimator.SetBool("isShooting", true);
                    theAnimator.SetFloat("isShootingCounter", counterIsShootingCounter);
                }
		        ShootRequest();
		        remainingShots--;
		        yield return MMCoroutine.WaitFor(intervalWaitFor);
            }

            if (!gameObject.CompareTag("Untagged"))
            {
                theAnimator.SetBool("isShooting", false);
                theAnimator.SetBool("IdleShootingStraight", false);
            }


            /*while (counter > 0) 
            { 
               counter -= Time.deltaTime; // Decrease the counter
               theAnimator.SetFloat("isShootingCounter", counter); 
               yield return null;
            }*/
        }

        /// <summary>
        /// Determines whether or not the weapon can fire
        /// </summary>
        protected virtual void ShootRequest()
		{
			// if we have a weapon ammo component, we determine if we have enough ammunition to shoot
			if (_reloading)
			{
				return;
			}

			if (MagazineBased)
			{
				if (WeaponAmmo != null)
				{
					if (WeaponAmmo.EnoughAmmoToFire())
					{
						WeaponState.ChangeState(WeaponStates.WeaponUse);	
					}
					else
					{
						if (AutoReload && MagazineBased)
						{
							InitiateReloadWeapon ();
						}
						else
						{
							WeaponState.ChangeState(WeaponStates.WeaponReloadNeeded);	
						}
					}
				}
				else
				{
					if (CurrentAmmoLoaded > 0)
					{
						WeaponState.ChangeState(WeaponStates.WeaponUse);	
						CurrentAmmoLoaded -= AmmoConsumedPerShot;
					}
					else
					{
						if (AutoReload)
						{
							InitiateReloadWeapon ();
						}
						else
						{
							WeaponState.ChangeState(WeaponStates.WeaponReloadNeeded);	
						}
					}
				}
			}
			else
			{
				if (WeaponAmmo != null)
				{
					if (WeaponAmmo.EnoughAmmoToFire())
					{
						WeaponState.ChangeState(WeaponStates.WeaponUse);	
					}
					else
					{
						WeaponState.ChangeState(WeaponStates.WeaponReloadNeeded);	
					}	
				}
				else
				{
					WeaponState.ChangeState(WeaponStates.WeaponUse);						
				}
            }
        }

		#endregion Permissions

		#region MovementMultiplier

		protected virtual void ResetMovementMultiplier ()
		{
			if (_movementMultiplierNeedsResetting && _characterHorizontalMovementNotNull)
			{ 
				if (ModifyMovementWhileAttacking)
				{
					_characterHorizontalMovement.MovementSpeedMultiplier = _movementMultiplierStorage;
					if (AlwaysResetMultiplierToInitial) { _characterHorizontalMovement.ResetMovementSpeedMultiplier(); }
				}
				_movementMultiplierNeedsResetting = false;
			}

			if (ModifyMovementWhileEquipped && _characterHorizontalMovementNotNull)
			{
				_characterHorizontalMovement.MovementSpeedMultiplier = PermanentMovementMultiplier;
			}
		}

		#endregion MovementMultiplier

		#region Reload

		/// <summary>
		/// Describes what happens when the weapon needs a reload
		/// </summary>
		protected virtual void ReloadNeeded()
		{
			TriggerWeaponReloadNeededFeedback ();
		}

		public virtual void InitiateReloadWeapon()
		{
			// if we're already reloading, we do nothing and exit
			if (_reloading || !MagazineBased)
			{
				return;
			}
			WeaponState.ChangeState(WeaponStates.WeaponReloadStart);
			_reloading = true;
		}

		/// <summary>
		/// Reloads the weapon
		/// </summary>
		/// <param name="ammo">Ammo.</param>
		protected virtual void ReloadWeapon()
		{
			if (MagazineBased)
			{
				TriggerWeaponReloadFeedback();	
			}
		}

		#endregion Reload

		#region Flip

		/// <summary>
		/// Flips the weapon.
		/// </summary>
		public virtual void FlipWeapon()
		{
			Flipped = !Flipped;

			if (_comboWeapon != null)
			{
				_comboWeapon.FlipUnusedWeapons();
			}
		}


		protected Vector3 _newHandleAngles;
		/// <summary>
		/// Flips the weapon model.
		/// </summary>
		public virtual void FlipWeaponModel()
		{	
			if (_spriteRenderer != null)
			{
				_spriteRenderer.flipX = !_spriteRenderer.flipX;
			} 
			else
			{
				transform.localScale = Vector3.Scale (transform.localScale, FlipValue);
				if (LeftHandHandle != null)
				{
					_newHandleAngles = LeftHandHandle.transform.localEulerAngles;
					_newHandleAngles.y = LeftHandHandle.transform.localEulerAngles.y + 180;
					LeftHandHandle.transform.localEulerAngles = _newHandleAngles;
				}
				if (RightHandHandle != null)
				{
					_newHandleAngles = RightHandHandle.transform.localEulerAngles;
					_newHandleAngles.y = RightHandHandle.transform.localEulerAngles.y + 180;
					RightHandHandle.transform.localEulerAngles = _newHandleAngles;
				}
			}	
		}

		#endregion Flip

		#region Destruction

		/// <summary>
		/// Destroys the weapon
		/// </summary>
		/// <returns>The destruction.</returns>
		public virtual IEnumerator WeaponDestruction()
		{
			Debug.Log("HERE????");
			yield return new WaitForSeconds (AutoDestroyWhenEmptyDelay);
			// if we don't have ammo anymore, and need to destroy our weapon, we do it
			TurnWeaponOff();
			Destroy (this.gameObject);
            
			if (WeaponID != null)
			{
				// we remove it from the inventory
				List<int> weaponList = Owner.gameObject.MMGetComponentNoAlloc<Character>()?.FindAbility<CharacterInventory>().WeaponInventory.InventoryContains(WeaponID);
				if (weaponList.Count > 0)
				{
					Owner.gameObject.MMGetComponentNoAlloc<Character>()?.FindAbility<CharacterInventory>().WeaponInventory.DestroyItem (weaponList [0]);
				}	
			}
		}

		#endregion Destruction

		#region Position

		/// <summary>
		/// Applies the offset specified in the inspector
		/// </summary>
		protected virtual void ApplyOffset()
		{
			_weaponAttachmentOffset = WeaponAttachmentOffset;

			if (Flipped)
			{
				_weaponAttachmentOffset.x = -WeaponAttachmentOffset.x;
			}

			if (_characterGravity != null)
			{
				_weaponAttachmentOffset = MMMaths.RotateVector2 (_weaponAttachmentOffset,_characterGravity.GravityAngle);
			}
			// we apply the offset
			if (transform.parent != null)
			{
				_weaponOffset = transform.parent.position + _weaponAttachmentOffset;
				transform.position = _weaponOffset;
			}	
		}

		#endregion Position

		#region Recoil

		public virtual void ApplyRecoil(bool shouldApplyRecoil, WeaponRecoilProperties properties)
		{
			if (!shouldApplyRecoil)
			{
				return;
			}
			if (properties.Delay > 0f)
			{
				StartCoroutine(ApplyRecoilCoroutine(properties));
			}
			else
			{
				ApplyRecoilInternal(properties);
			}
		}

		protected virtual IEnumerator ApplyRecoilCoroutine(WeaponRecoilProperties properties)
		{
			yield return MMCoroutine.WaitFor(properties.Delay);
			ApplyRecoilInternal(properties);
		}

		protected virtual void ApplyRecoilInternal(WeaponRecoilProperties properties)
		{
			_recoilDirection = GetRecoilDirection(properties);

			float force = _controller.State.IsGrounded ? properties.RecoilForceGrounded : properties.RecoilForceAirborne;

			switch (properties.RecoilStyle)
			{
				case DamageOnTouch.KnockbackStyles.AddForce:
					_controller.AddForce(_recoilDirection.normalized * force * properties.DirectionMultiplier);
					break;
				case DamageOnTouch.KnockbackStyles.SetForce:
					_controller.SetForce(_recoilDirection.normalized * force * properties.DirectionMultiplier);
					break;
			}

			properties.RecoilFeedback?.PlayFeedbacks();
		}

		protected virtual Vector2 GetRecoilDirection(WeaponRecoilProperties properties)
		{
			Vector2 recoilDirection = this.Owner.IsFacingRight ? -this.transform.right : this.transform.right;
			float recoilAngle = this.Owner.IsFacingRight ? properties.RecoilAngleModifier : -properties.RecoilAngleModifier;
			return MMMaths.RotateVector2(recoilDirection, recoilAngle);
		}

		#endregion Recoil

		#region Feedbacks

		/// <summary>
		/// Plays the weapon's start sound
		/// </summary>
		protected virtual void TriggerWeaponStartFeedback()
		{
			WeaponStartMMFeedback?.PlayFeedbacks(this.transform.position);
		}	

		/// <summary>
		/// Plays the weapon's used sound
		/// </summary>
		protected virtual void TriggerWeaponUsedFeedback()
		{
			WeaponUsedMMFeedback?.PlayFeedbacks(this.transform.position);
		}	

		/// <summary>
		/// Plays the weapon's stop sound
		/// </summary>
		protected virtual void TriggerWeaponStopFeedback()
		{
			WeaponStopMMFeedback?.PlayFeedbacks(this.transform.position);
		}	

		/// <summary>
		/// Plays the weapon's reload needed sound
		/// </summary>
		protected virtual void TriggerWeaponReloadNeededFeedback()
		{
			WeaponReloadNeededMMFeedback?.PlayFeedbacks(this.transform.position);
		}	

		/// <summary>
		/// Plays the weapon's reload sound
		/// </summary>
		protected virtual void TriggerWeaponReloadFeedback()
		{
			WeaponReloadMMFeedback?.PlayFeedbacks(this.transform.position);
		}

		/// <summary>
		/// Plays a feedback when the weapon hits something
		/// </summary>
		protected virtual void TriggerWeaponOnHitFeedback()
		{
			WeaponOnHitFeedback?.PlayFeedbacks(this.transform.position);
		}

		/// <summary>
		/// Plays a feedback when the weapon doesn't hit something
		/// </summary>
		protected virtual void TriggerWeaponOnMissFeedback()
		{
			WeaponOnMissFeedback?.PlayFeedbacks(this.transform.position);
		}

		/// <summary>
		/// Plays a feedback when the weapon hits a damageable
		/// </summary>
		protected virtual void TriggerWeaponOnHitDamageableFeedback()
		{
			WeaponOnHitDamageableFeedback?.PlayFeedbacks(this.transform.position);
		}

		/// <summary>
		/// Plays a feedback when the weapon hits a non damageable
		/// </summary>
		protected virtual void TriggerWeaponOnHitNonDamageableFeedback()
		{
			WeaponOnHitNonDamageableFeedback?.PlayFeedbacks(this.transform.position);
		}

		/// <summary>
		/// Plays a feedback when the weapon kills something
		/// </summary>
		protected virtual void TriggerWeaponOnKillFeedback()
		{
			WeaponOnKillFeedback?.PlayFeedbacks(this.transform.position);
		}

		#endregion Feedbacks

		#region Animation

		/// <summary>
		/// Adds required animator parameters to the animator parameters list if they exist
		/// </summary>
		public virtual void InitializeAnimatorParameters()
		{
			for (int i = 0; i < Animators.Count; i++)
			{
				_animatorParameters.Add(new HashSet<int>());
				AddParametersToAnimator(Animators[i], _animatorParameters[i]);
				
				if (MirrorCharacterAnimatorParameters)
				{
					MMAnimatorMirror mirror = Animators[i].gameObject.AddComponent<MMAnimatorMirror>();
					mirror.SourceAnimator = _ownerAnimator;
					mirror.TargetAnimator = Animators[i];
					mirror.Initialization();
				}
			}

			if (_ownerAnimator != null)
			{
				_ownerAnimatorParameters = new HashSet<int>();
				AddParametersToAnimator(_ownerAnimator, _ownerAnimatorParameters);
			}            
		}

		protected virtual void AddParametersToAnimator(Animator animator, HashSet<int> list)
		{
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, EquippedAnimationParameter, out _equippedAnimationParameter, AnimatorControllerParameterType.Bool, list);
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, WeaponAngleAnimationParameter, out _weaponAngleAnimationParameter, AnimatorControllerParameterType.Float, list);
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, WeaponAngleRelativeAnimationParameter, out _weaponAngleRelativeAnimationParameter, AnimatorControllerParameterType.Float, list);
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, IdleAnimationParameter, out _idleAnimationParameter, AnimatorControllerParameterType.Bool, list);
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, StartAnimationParameter, out _startAnimationParameter, AnimatorControllerParameterType.Bool, list);
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, DelayBeforeUseAnimationParameter, out _delayBeforeUseAnimationParameter, AnimatorControllerParameterType.Bool, list);
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, DelayBetweenUsesAnimationParameter, out _delayBetweenUsesAnimationParameter, AnimatorControllerParameterType.Bool, list);
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, InCooldownAnimationParameter, out _inCooldownAnimationParameter, AnimatorControllerParameterType.Bool, list);
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, StopAnimationParameter, out _stopAnimationParameter, AnimatorControllerParameterType.Bool, list);
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, ReloadStartAnimationParameter, out _reloadStartAnimationParameter, AnimatorControllerParameterType.Bool, list);
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, ReloadStopAnimationParameter, out _reloadStopAnimationParameter, AnimatorControllerParameterType.Bool, list);
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, ReloadAnimationParameter, out _reloadAnimationParameter, AnimatorControllerParameterType.Bool, list);
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, SingleUseAnimationParameter, out _singleUseAnimationParameter, AnimatorControllerParameterType.Bool, list);
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, UseAnimationParameter, out _useAnimationParameter, AnimatorControllerParameterType.Bool, list);
			MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, _aliveAnimationParameterName, out _aliveAnimationParameter, AnimatorControllerParameterType.Bool, list);

			if (_comboWeapon != null)
			{
				MMAnimatorExtensions.AddAnimatorParameterIfExists(animator, _comboWeapon.ComboInProgressAnimationParameter, out _comboInProgressAnimationParameter, AnimatorControllerParameterType.Bool, list);
			}
		}

		/// <summary>
		/// Override this to send parameters to the character's animator. This is called once per cycle, by the Character
		/// class, after Early, normal and Late process().
		/// </summary>
		public virtual void UpdateAnimator()
		{
			for (int i = 0; i < Animators.Count; i++)
			{
				UpdateAnimator(Animators[i], _animatorParameters[i]);
			}

			if ((_ownerAnimator != null) && (WeaponState != null) && (_ownerAnimatorParameters != null))
			{
				UpdateAnimator(_ownerAnimator, _ownerAnimatorParameters);
			}
		}

		protected virtual void UpdateAnimator(Animator animator, HashSet<int> list)
		{
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _equippedAnimationParameter, true, list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _idleAnimationParameter, (WeaponState.CurrentState == Weapon.WeaponStates.WeaponIdle), list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _startAnimationParameter, (WeaponState.CurrentState == Weapon.WeaponStates.WeaponStart), list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _delayBeforeUseAnimationParameter, (WeaponState.CurrentState == Weapon.WeaponStates.WeaponDelayBeforeUse), list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _useAnimationParameter, (WeaponState.CurrentState == Weapon.WeaponStates.WeaponDelayBeforeUse || WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse || WeaponState.CurrentState == Weapon.WeaponStates.WeaponDelayBetweenUses), list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _singleUseAnimationParameter, (WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse), list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _delayBetweenUsesAnimationParameter, (WeaponState.CurrentState == Weapon.WeaponStates.WeaponDelayBetweenUses), list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _inCooldownAnimationParameter, (WeaponState.CurrentState == Weapon.WeaponStates.WeaponInCooldown), list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _stopAnimationParameter, (WeaponState.CurrentState == Weapon.WeaponStates.WeaponStop), list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _reloadStartAnimationParameter, (WeaponState.CurrentState == Weapon.WeaponStates.WeaponReloadStart), list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _reloadAnimationParameter, (WeaponState.CurrentState == Weapon.WeaponStates.WeaponReload), list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _reloadStopAnimationParameter, (WeaponState.CurrentState == Weapon.WeaponStates.WeaponReloadStop), list);

			if (Owner != null)
			{
				MMAnimatorExtensions.UpdateAnimatorBool(animator, _aliveAnimationParameter, (Owner.ConditionState.CurrentState != CharacterStates.CharacterConditions.Dead), list);
			}

			if (_aimableWeapon != null)
			{
				MMAnimatorExtensions.UpdateAnimatorFloat(animator, _weaponAngleAnimationParameter, _aimableWeapon.CurrentAngle, list);
				MMAnimatorExtensions.UpdateAnimatorFloat(animator, _weaponAngleRelativeAnimationParameter, _aimableWeapon.CurrentAngleRelative, list);
			}
			else
			{
				MMAnimatorExtensions.UpdateAnimatorFloat(animator, _weaponAngleAnimationParameter, 0f, list);
				MMAnimatorExtensions.UpdateAnimatorFloat(animator, _weaponAngleRelativeAnimationParameter, 0f, list);
			}

			if (_comboWeapon != null)
			{
				MMAnimatorExtensions.UpdateAnimatorBool(animator, _comboInProgressAnimationParameter, _comboWeapon.ComboInProgress, list);
			}
		}

		public virtual void ResetComboAnimatorParameter()
		{
			if (_comboWeapon != null)
			{
				for (int i = 0; i < Animators.Count; i++)
				{
					MMAnimatorExtensions.UpdateAnimatorBool(Animators[i], _comboInProgressAnimationParameter, _comboWeapon.ComboInProgress, _animatorParameters[i]);
				}

				if ((_ownerAnimator != null) && (WeaponState != null) && (_ownerAnimatorParameters != null))
				{
					MMAnimatorExtensions.UpdateAnimatorBool(_ownerAnimator, _comboInProgressAnimationParameter, _comboWeapon.ComboInProgress, _ownerAnimatorParameters);
				}
			}
		}

		protected virtual void ResetAnimatorParameters(Animator animator, HashSet<int> list)
		{
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _equippedAnimationParameter, false, list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _idleAnimationParameter, false, list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _startAnimationParameter, false, list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _delayBeforeUseAnimationParameter, false, list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _useAnimationParameter, false, list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _singleUseAnimationParameter, false, list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _delayBetweenUsesAnimationParameter, false, list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _inCooldownAnimationParameter, false, list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _stopAnimationParameter, false, list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _reloadStartAnimationParameter, false, list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _reloadAnimationParameter, false, list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _reloadStopAnimationParameter, false, list);
			MMAnimatorExtensions.UpdateAnimatorFloat(animator, _weaponAngleAnimationParameter, 0f, list);
			MMAnimatorExtensions.UpdateAnimatorFloat(animator, _weaponAngleRelativeAnimationParameter, 0f, list);
			MMAnimatorExtensions.UpdateAnimatorBool(animator, _comboInProgressAnimationParameter, false, list);
		}

		public virtual void ResetAnimatorParameters()
		{
			if (_animatorParameters != null)
			{
				for (int i = 0; i < Animators.Count; i++)
				{
					ResetAnimatorParameters(Animators[i], _animatorParameters[i]);
				}	
			}

			if ((_ownerAnimator != null) && (WeaponState != null) && (_ownerAnimatorParameters != null))
			{
				ResetAnimatorParameters(_ownerAnimator, _ownerAnimatorParameters);
			}
		}

		#endregion Animation

		#region  Events

		public virtual void WeaponHit()
		{
			TriggerWeaponOnHitFeedback();
	        
		}
		public virtual void WeaponHitDamageable()
		{
			TriggerWeaponOnHitDamageableFeedback();
			ApplyRecoil(ApplyRecoilOnHitDamageable, RecoilOnHitDamageableProperties);
		}
		public virtual void WeaponHitNonDamageable()
		{
			TriggerWeaponOnHitNonDamageableFeedback();
			ApplyRecoil(ApplyRecoilOnHitNonDamageable, RecoilOnHitNonDamageableProperties);   
	        
		}
		public virtual void WeaponMiss()
		{
			TriggerWeaponOnMissFeedback();
			ApplyRecoil(ApplyRecoilOnMiss, RecoilOnMissProperties);   
	        
		}
		public virtual void WeaponKill()
		{
			TriggerWeaponOnKillFeedback();
			ApplyRecoil(ApplyRecoilOnKill, RecoilOnKillProperties);   
		}

		protected virtual void OnDisable()
		{
			RestoreMovement();
			ResetAnimatorParameters();
		}

		#endregion
        
        
	}
}