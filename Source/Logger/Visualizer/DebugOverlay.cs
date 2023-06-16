using Godot;
using System;
using System.Collections.Generic;
using Vain.Core;


namespace Vain.Log.Visualizer
{
	public partial class DebugOverlay : Control
    {
        VBoxContainer _container;
        
       
        Dictionary<string, Field> _fields = new Dictionary<string, Field>();

	    public override void _EnterTree()
		{
			base._EnterTree();
			SingletonManager.Register(this);
            this.ZIndex = 100;
		}
        public override void _Ready()
        {
           
			_container = new VBoxContainer();
			
           

			this.AddChild(_container);
        }

        void ClearFields()
        {
            foreach (var field in _fields.Values)
            {
                field.QueueFree();
            }

            _fields.Clear();
        }

        public void Log(string label, string message)
        {
            if(!_fields.ContainsKey(label))
            {
                _fields.Add(label,new Field(label));
                _container.AddChild(_fields[label]);
            }
          
             _fields[label].Value = message;
        }
        void DeleteField(string label)
        {
            if(_fields.ContainsKey(label))
                _fields[label].QueueFree();
        }
    }
}
