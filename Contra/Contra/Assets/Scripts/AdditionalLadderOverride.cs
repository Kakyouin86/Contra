using UnityEngine;
using System.Collections;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class AdditionalLadderOverride : Ladder
{
    //Ladder line 14 public enum LadderTypes { Simple, BiDirectional, Horizontal } //Leo Monge
    //CharacterLadder:
    /* protected override void Initialization()
       {
       base.Initialization();
       CurrentLadderClimbingSpeed = Vector2.zero;
       //_boxCollider = this.gameObject.GetComponentInParent<BoxCollider2D>();//Leo Monge.
       _boxCollider = GameObject.FindWithTag("LadderCollider").GetComponent<BoxCollider2D>();//Leo Monge. This adds the collider of the Ladder Collider only
       _colliders = new List<Collider2D>();
       _characterHandleWeapon = this.gameObject.GetComponentInParent<Character>()?.FindAbility<CharacterHandleWeapon>();
       }
     */
    //CharacterWallClinging line 21 [Range(0.0000000001f, 1)]//Leo Monge: it was 0.01f originally. With this small value, you can't detach from the wall.

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        // we check that the object colliding with the ladder is actually a corgi controller and a character
        //CharacterLadder characterLadder = collider.gameObject.MMGetComponentNoAlloc<Character>()?.FindAbility<CharacterLadder>(); //Leo Monge. This was originally the only sentence. Then goes to the if.
        if (collider.gameObject.tag == "LadderCollider")
        {
            CharacterLadder characterLadder =
                collider.GetComponentInParent<Character>()?.FindAbility<CharacterLadder>();

            if (characterLadder == null)
            {
                return;
            }

            characterLadder.AddCollidingLadder(_collider2D);
            {
                if (gameObject.tag == "HorizontalLadder")
                {
                    collider.GetComponentInParent<Character>().FindAbility<CharacterLadder>().NoInputClimb = true;
                }
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collider)
    {
        // we check that the object colliding with the ladder is actually a corgi controller and a character
        //CharacterLadder characterLadder = collider.gameObject.MMGetComponentNoAlloc<Character>()?.FindAbility<CharacterLadder>();//Leo Monge. This was originally the only sentence. Then goes to the if.
        if (collider.gameObject.tag == "LadderCollider")
        {
            CharacterLadder characterLadder =
                collider.GetComponentInParent<Character>()?.FindAbility<CharacterLadder>();

            if (characterLadder == null)
            {
                return;
            }

            characterLadder.RemoveCollidingLadder(_collider2D);
            {
                if (gameObject.tag == "HorizontalLadder")
                {
                    collider.GetComponentInParent<Character>().FindAbility<CharacterLadder>().NoInputClimb = false;
                }
            }
        }
    }
}