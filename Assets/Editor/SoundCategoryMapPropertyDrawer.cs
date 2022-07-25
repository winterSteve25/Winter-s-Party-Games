using UnityEditor;
using Utils.Audio;

namespace Editor
{
    [CustomPropertyDrawer(typeof(SoundCategoryMap))]
    public class SoundCategoryMapPropertyDrawer : SerializableDictionaryPropertyDrawer
    {
        
    }
}