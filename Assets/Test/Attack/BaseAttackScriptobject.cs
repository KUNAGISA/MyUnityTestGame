using UnityEngine;

namespace Attack
{
    public abstract class BaseAttackScriptobject : ScriptableObject, ILogicCreator
    {
        IAttackLogic ILogicCreator.CreateLogic() => CreateAttackLogic();

        protected abstract IAttackLogic CreateAttackLogic();
    }
}
