using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSkill : Skill {

    public override Type type { get { return Type.Character; } }

    protected PlayerCharacter character;

    public void Initialize(PlayerCharacter character) {
        this.character = character;
        if (level <= 0) {
            return;
        }
        Initialize();
    }

    protected abstract void Initialize();
}
