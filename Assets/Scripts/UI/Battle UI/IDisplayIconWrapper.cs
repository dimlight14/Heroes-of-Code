using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesOfCode.UI
{
    public interface IDisplayableIcon
    {
        string GetDescription();
        string GetName();
        Sprite GetImage();
    }
}
