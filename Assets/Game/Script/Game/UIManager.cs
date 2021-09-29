using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public void ButtonNext()
        {
            RuntimeStorageData.PLAYER.level += 1;
            GameManager.LoadScene(SceneName.Game, true);
        }

        public void ButtonBack()
        {
            RuntimeStorageData.PLAYER.level -= 1;
            if (RuntimeStorageData.PLAYER.level < 1)
                RuntimeStorageData.PLAYER.level = 1;

            GameManager.LoadScene(SceneName.Game, true);
        }

        public void Replay()
        {
            GameManager.LoadScene(SceneName.Game, true);
        }

        public void TapToPlay()
        {
            StaticVariable.GameState = GameState.PLAY;
        }
    }
}
