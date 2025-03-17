using Eflatun.SceneReference;
using System;

namespace NULL.Scenes
{
    [Serializable]
    public class SceneData
    {
        public SceneReference Reference;
        public string Name => Reference.Name;
        public SceneType SceneType;
    }
}
