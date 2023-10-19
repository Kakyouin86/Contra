using UnityEngine;
using System.Collections;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class AdditionalLadderOverride : Ladder
{
    public bool canNotDettach;
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