using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController
{

    public override void Death()
    {
        if (SkillManager.Instance.HealOnDeath)
        {
            SkillManager.Instance.HealPlayer(10f);
        }
        if (SkillManager.Instance.BoomOnDeath)
        {

        }
        base.Death();
    }

}