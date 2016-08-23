
using UnityEngine;
using System.Collections;

namespace Splatoon
{
    public class CommonFunction
    {
        public static Transform FindObject(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                {
                    return child;
                }
                else if (child.childCount > 0)
                {
                    Transform childTrans = FindObject(child, name);
                    if (childTrans != null)
                        return childTrans;
                }
            }

            return null;
        }
    }
}

