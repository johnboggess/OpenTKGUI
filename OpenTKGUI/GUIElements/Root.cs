using System;
using System.Collections.Generic;
using System.Text;

using OpenTKGUI.Enums;

using OpenTK.Mathematics;
namespace OpenTKGUI.GUIElements
{
    public class Root : GUIContainer
    {
        internal Root() { }

        public override void AddChild(GUIElement element, params object[] args)
        {
            if (_Children.Count > 0)
                throw new Exception("Root can only have one child");
            base.AddChild(element);
        }
    }
}
