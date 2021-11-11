using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Element
{
    public static class ElementBuffFactor
    {
        /// <summary>
        /// First entry = attacker
        /// Second entry = receiver
        /// Data = elemental multiplier
        /// </summary>
        //public static List<List<float>> elementCounterTable;
        public static float[,] elementCounterTable;

        static ElementBuffFactor()
        {
            elementCounterTable = new float[7, 7];
            int empty = ElementTypeToInt.CastElementTypeToInt(ElementType.EMPTY);
            int fire = ElementTypeToInt.CastElementTypeToInt(ElementType.FIRE);
            int water = ElementTypeToInt.CastElementTypeToInt(ElementType.WATER);
            int ice = ElementTypeToInt.CastElementTypeToInt(ElementType.ICE);
            int grass = ElementTypeToInt.CastElementTypeToInt(ElementType.GRASS);
            int dark = ElementTypeToInt.CastElementTypeToInt(ElementType.DARK);
            int holy = ElementTypeToInt.CastElementTypeToInt(ElementType.HOLY);

            //Empty attacker: 
            elementCounterTable[empty,empty] = 1f;
            elementCounterTable[empty,fire] = 1f;
            elementCounterTable[empty,water] = 1f;
            elementCounterTable[empty,ice] = 1f;
            elementCounterTable[empty,grass] = 1f;
            elementCounterTable[empty,dark] = 1f;
            elementCounterTable[empty,holy] = 1.25f;

            //fire attacker: 
            elementCounterTable[fire,empty] = 1f;
            elementCounterTable[fire,fire] = 1f;
            elementCounterTable[fire,water] = 0.85f;
            elementCounterTable[fire,ice] = 1.25f;
            elementCounterTable[fire,grass] = 1.25f;
            elementCounterTable[fire,dark] = 1f;
            elementCounterTable[fire,holy] = 0.85f;
            
            //water attacker:
            elementCounterTable[water,empty] = 1f;
            elementCounterTable[water,fire] = 1.25f;
            elementCounterTable[water,water] = 1f;
            elementCounterTable[water,ice] = 1f;
            elementCounterTable[water,grass] = 0.85f;
            elementCounterTable[water,dark] = 1f;
            elementCounterTable[water,holy] = 1f;

            //ice attacker:
            elementCounterTable[ice,empty] = 1f;
            elementCounterTable[ice,fire] = 0.85f;
            elementCounterTable[ice,water] = 1f;
            elementCounterTable[ice,ice] = 1f;
            elementCounterTable[ice,grass] = 1.25f;
            elementCounterTable[ice,dark] = 1f;
            elementCounterTable[ice,holy] = 1f;
            
            //grass attacker:
            elementCounterTable[grass,empty] = 1f;
            elementCounterTable[grass,fire] = 0.85f;
            elementCounterTable[grass,water] = 1.25f;
            elementCounterTable[grass,ice] = 1f;
            elementCounterTable[grass,grass] = 1f;
            elementCounterTable[grass,dark] = 1f;
            elementCounterTable[grass,holy] = 1.25f;
            
            //dark attacker:
            elementCounterTable[dark,empty] = 1f;
            elementCounterTable[dark,fire] = 1.25f;
            elementCounterTable[dark,water] = 1f;
            elementCounterTable[dark,ice] = 1f;
            elementCounterTable[dark,grass] = 1f;
            elementCounterTable[dark,dark] = 1.25f;
            elementCounterTable[dark,holy] = 0.85f;
            
            //holy attacker:
            elementCounterTable[holy,empty] = 1f;
            elementCounterTable[holy,fire] = 0.85f;
            elementCounterTable[holy,water] = 1f;
            elementCounterTable[holy,ice] = 1.25f;
            elementCounterTable[holy,grass] = 0.85f;
            elementCounterTable[holy,dark] = 1.25f;
            elementCounterTable[holy,holy] = 1f;
        }


        
        public static float GetElementalBuffFactor(ElementType attackElement, ElementType receiverElement)
        {
            int attacker = ElementTypeToInt.CastElementTypeToInt(attackElement);
            int receiver = ElementTypeToInt.CastElementTypeToInt(receiverElement);
            return elementCounterTable[attacker,receiver];
        }

        //public static List<ElementType>
    }
    
}
