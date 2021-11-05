using UnityEngine;

namespace Game
{
    public static class GameInit
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitGameEvn()
        {
            var _ = Game2D.Instance;
        }
    }
}