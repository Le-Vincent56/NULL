using System.Collections.Generic;
using System;
using System.Linq;

namespace NULL.Scenes
{
    public enum SceneType 
    { 
        ActiveScene, 
        MainMenu, 
        UserInterface, 
        HUD, 
        Cinematic, 
        Environment, 
        Tooling 
    }

    [Serializable]
    public class SceneGroup
    {
        public string GroupName = "New Scene Group";
        public List<SceneData> Scenes;

        /// <summary>
        /// Finds the scene name by the scene type
        /// </summary>
        public string FindSceneNameByType(SceneType sceneType)
        {
            return Scenes.FirstOrDefault(scene => scene.SceneType == sceneType)?.Reference.Name;
        }
    }
}
