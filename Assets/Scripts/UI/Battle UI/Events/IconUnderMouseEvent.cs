using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesOfCode.UI
{
    public class IconUnderMouseEvent : CustomEvent
    {
        public IDisplayIconWrapper Icon;
        public bool IconBelongsToActiveUnit;
    }
}
