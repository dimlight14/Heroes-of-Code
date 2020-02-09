using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesOfCode.Tilemap
{
    [CreateAssetMenu(menuName = "Path Image Sprite Settings")]
    public class PathImageSpriteSettings : ScriptableObject
    {
        [System.Serializable]
        private struct ValuePair
        {
            public TilePathImageController.PathImageType key;
            public Sprite value;
        }

        [SerializeField]
        private List<ValuePair> pathImages = null;

        [HideInInspector]
        public Dictionary<TilePathImageController.PathImageType, Sprite> pathImagesMap {
            get {
                var dictionary = new Dictionary<TilePathImageController.PathImageType, Sprite>();
                foreach (var keyPair in pathImages) {
                    dictionary.Add(keyPair.key, keyPair.value);
                }
                return dictionary;
            }
        }
    }
}
