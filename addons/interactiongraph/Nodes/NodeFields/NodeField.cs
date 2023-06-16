#if TOOLS
using System;
using Godot;
using System.Reflection;
namespace Vain.InteractionSystem.InteractionGraph
{
    [Tool]
    internal partial class NodeField : HBoxContainer 
    {

        string _fieldName;
        bool _inline = false;
    
        

        //Teoporary, for inline testing
        Type _type;
        LineEdit _lineEdit;
        






        internal string FieldName 
        {
            get => GetChildOrNull<Label>(0)?.Text;
            set
            {
                var label = GetChildOrNull<Label>(0);
                if(label != null)
                {

                    label.Name = value.Trim();
                    label.Text = value;
                }
               
            }



        }
        




        //TODO: Dynamic typing, find a better solution in the future, since types are parsed dinamically it's not possibile to use generic unless checking for every type possible
              
        internal object Value 
        {
            get;
            set;
            
        }
        
        private NodeField(){}

        internal NodeField(Type type, String name)
        {


            if(type.IsPrimitive)
                _inline = true;



            this._fieldName = name;
            this.Name = name;
            
        }
        public override void _Ready()
        {
        
            
           


            base._Ready();
            var label = GetChildOrNull<Label>(0);
            if(label == null)
            {

                label = new Label();

                this.Name =  _fieldName;


            


                label.Name = "Label";
                label.Text = _fieldName;
            


                AddChild(label);
            
                

                
                if(_inline)
                {
                    addInlineField(_type);
                }



                foreach (var child in GetChildren())
                {
                    child.Owner = this.GetParent();
                    
                }
                
            }
        }


        void addInlineField(Type type)
        {
            
            if(type == typeof(Boolean))
            {
                var checkButton = new CheckButton();
                
                checkButton.ToggleMode = true;
                checkButton.Pressed += () => 
                {  
                    this.Value = checkButton.ButtonPressed;
                };
                checkButton.Name = "CheckButton";
                AddChild(checkButton);

                return;
            }
            

            _type = type;

            _lineEdit = new LineEdit();
            

            _lineEdit.TextSubmitted += testInput;
            _lineEdit.Name = "LineEdit";
            
            AddChild(_lineEdit);
           
        
        }
        
        
        void testInput(String text)
        {
            object value = null;
            if(_type == typeof(Int32))
            {
                int res = 0;
                var parsed = int.TryParse(text, out res);
                value = parsed ? res : 0;
               
            } 


            else if(_type == typeof(Single) || _type == typeof(Double))
            {
                float res;
                var parsed = float.TryParse(text, out res);
                value =  parsed ? res : 0;
               
            } 

            else
            {
                value = text;
            }

            
            _lineEdit.Text = value.ToString();
            this.Value = value;
        }


     


    
        



    }

}
#endif