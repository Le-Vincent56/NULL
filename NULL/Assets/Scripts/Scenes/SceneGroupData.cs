using UnityEngine;

namespace NULL.Scenes
{
    [CreateAssetMenu(fileName = "Scene Group Data", menuName = "Data/Scene Group")]
    public class SceneGroupData : ScriptableObject
    {
        public SceneGroup[] SceneGroups;
        [HideInInspector] public int InitialScene;
    }
}
