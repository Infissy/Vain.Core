using System;
using System.Diagnostics;
using Godot;

namespace Vain.UI
{

    //? Temporary class, might need in case of 
    public partial class HealthBar : ProgressBar
    {
        
        public float MaxHealth 
        {
            set
            {
                base.MaxValue = value;
            }
        }
        public float Health 
        {
            set
            {
                
                base.Value = value;
                
            }
        }

        

    }
}