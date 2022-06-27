using System;
using System.Diagnostics;
using Godot;

namespace Vain.UI
{
    public class HealthBarUI : UIElement
    {
        
        
        float _health;

        public HealthBarUI(Control node) : base(node){}

        public float Health 
        {
            get => _health;

            set
            {
                
                _health = value;
                
            }
        }

        public Vector2 Position
        {
            set 
            {
                Node.RectGlobalPosition = value;
            }
        }
        

    }
}