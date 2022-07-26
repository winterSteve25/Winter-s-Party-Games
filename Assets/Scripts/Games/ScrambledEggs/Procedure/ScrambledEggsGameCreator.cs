using Games.ScrambledEggs.Data;
using UnityEngine;
using Utils;

namespace Games.ScrambledEggs.Procedure
{
    public class ScrambledEggsGameCreator : MonoBehaviour
    {
        [SerializeField] private int wordTasksCount;
        [SerializeField] private int drawingTasksCount;
        
        private void Awake()
        {
            GlobalData.Set(GameConstants.GlobalData.LatestGameData, new ScrambledEggsGameData(wordTasksCount, drawingTasksCount));
            Destroy(gameObject);
        }
    }
}