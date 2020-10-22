using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTKGUI.GUIElements
{
    public class GUIContainer : GUIElement
    {
        public virtual void AddChild(GUIElement element, params object[] args)
        {
            GUIManager._QueueElementToBeAdded(this, element);
        }
    }
}
