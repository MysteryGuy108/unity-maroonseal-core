using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal {
    static public class StringFormatting
    {   
        static public string GetIterativeName(string newNametag) {

            string[] splitNametag = newNametag.Split('_');

            if (int.TryParse(splitNametag[^1], out int id)) {
                int digitLength = splitNametag[^1].Length;
                newNametag = newNametag.Remove(newNametag.Length - digitLength, digitLength);
                id++;
                newNametag += id.ToString();
            }
            else { newNametag += "_0"; }

            return newNametag;
        }
    }
}

