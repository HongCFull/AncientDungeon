using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Element
{
    public enum ElementType
    {
        EMPTY,
        FIRE,
        WATER,
        ICE,
        GRASS,
        DARK,
        HOLY
    }
    
    public static class ElementTypeToInt
    {
        public static int CastElementTypeToInt(ElementType elementType)
        {
            switch (elementType) {
                case ElementType.EMPTY:
                    return 0;
                
                case ElementType.FIRE:
                    return 1;
                
                case ElementType.WATER:
                    return 2;
                
                case ElementType.ICE:
                    return 3;
                
                case ElementType.GRASS:
                    return 4;
                
                case ElementType.DARK:
                    return 5;
                
                case ElementType.HOLY:
                    return 6;
                
                default:
                    return 0;
            }
        }    
    }
}
