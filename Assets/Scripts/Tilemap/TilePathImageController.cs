using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesOfCode.Tilemap
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TilePathImageController : MonoBehaviour
    {
        public enum PathImageType
        {
            DiagonalTopLeft,
            DiagonalBottomLeft,
            Horizontal,
            Vertical,
            EndPath,
            None
        }
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private PathImageSpriteSettings spriteSettings = null;
        private Dictionary<PathImageType, Sprite> spriteMap;
        private PathImageType currImageType = PathImageType.None;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteMap = spriteSettings.pathImagesMap;
        }

        public void ClearPathImage() {
            currImageType = PathImageType.None;
            spriteRenderer.sprite = null;
        }
        public void SetPathImageAsEnd() {
            currImageType = PathImageType.None;
            spriteRenderer.sprite = SelectPathImage(PathImageType.EndPath);
        }

        public void SetPathImage(Vector2 prevPos, Vector2 thisPos) {
            PathImageType newImageType = DetermineImageType(prevPos, thisPos);
            if (newImageType == currImageType) return;

            spriteRenderer.sprite = SelectPathImage(newImageType);
        }
        private Sprite SelectPathImage(PathImageType imageType) {
            currImageType = imageType;
            Sprite returnSprite = spriteMap[imageType];
            return returnSprite;
        }
        private PathImageType DetermineImageType(Vector2 prevPos, Vector2 thisPos) {
            if (thisPos.x < prevPos.x) {
                if (thisPos.y < prevPos.y) {
                    return PathImageType.DiagonalBottomLeft;
                }
                else if (thisPos.y > prevPos.y) {
                    return PathImageType.DiagonalTopLeft;
                }
                else return PathImageType.Horizontal;
            }
            else if (thisPos.x > prevPos.x) {
                if (thisPos.y < prevPos.y) {
                    return PathImageType.DiagonalTopLeft;
                }
                else if (thisPos.y > prevPos.y) {
                    return PathImageType.DiagonalBottomLeft;
                }
                else return PathImageType.Horizontal;
            }
            else {
                return PathImageType.Vertical;
            }
        }
    }
}
