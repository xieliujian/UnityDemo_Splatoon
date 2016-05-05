
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace Splatoon
{
    public class SpButton : Button
    {
        public void Press()
        {
            DoStateTransition(SelectionState.Pressed, true);
        }

        public void Normal()
        {
            DoStateTransition(SelectionState.Normal, true);
        }
    }
}
