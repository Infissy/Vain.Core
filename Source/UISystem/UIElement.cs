using Godot;

using System;
using System.Collections.Generic;

namespace Vain.UI
{
    //UIElement is the base class of all UIElements that are going to be a wrapper between the component and the engine element
    
    //This way it's easier moving between engines and/or making custom controls for the elements without changing the component itself
    public abstract class UIElement
    {


        Control _node;


        public Control Node =>  _node;
        
        public UIElement(Control node)
        {

            this._node = node;
            


        }

    }

}