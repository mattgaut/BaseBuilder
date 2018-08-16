using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSkill : Skill {

    public override Type type { get { return Type.Character; } }

    public abstract void Initialize(PlayerCharacter character);
}
