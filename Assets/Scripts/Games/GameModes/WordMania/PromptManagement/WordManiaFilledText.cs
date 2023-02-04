using UnityEngine.EventSystems;

namespace Games.GameModes.WordMania.PromptManagement
{
    public class WordManiaFilledText : WordManiaWordChoice
    {
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            Destroy(gameObject);
        }
    }
}