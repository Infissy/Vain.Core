using System;
using Godot;

namespace Vain.InteractionSystem.InteractionGraph
{
    
    abstract partial class InlineInputField : NodeField
    {
        
        string _name;
        LineEdit _input;



        internal string FieldName 
        {
            get => GetChild<Label>(0).Text;
            set
            {
                var label = GetChildOrNull<Label>(0);
                if(label != null)
                {

                    label.Name = value.Trim();
                    label.Text = value;
                }
                else
                {
                    _name = value;
                }
            } 
        }
        


        public override void _Ready()
        {
        

            base._Ready();

        
            if(GetChildOrNull<Label>(0) == null)
            {
                
                var label = new Label();

                label.Text = _name;

                AddChild(label);


                _input = new LineEdit();
                AddChild(_input);

                switch (Type.GetTypeCode(typeof(T)))
                {
                    case TypeCode.Int32:
                        {

                            _input.TextSubmitted +=  TestInt;
                        }
                        break;
                    case TypeCode.Double:
                    case TypeCode.Single:
                        {
                            _input.TextSubmitted += TestFloat;
                           
                        }
                        break;
                    default:
                        break;                        
                    

                    
                }
                FieldName = _name;
            }

            foreach (var child in GetChildren())
                GD.Print(child);

        }

        protected abstract void TestField(string text);
        


        void TestInt(String text) 
        {
            int res = 0;
            var parsed = int.TryParse(text,out res);
            _input.Text = parsed ? res.ToString() : "0";

        }


        void TestFloat(String text)
        {
            float res = 0;
            var parsed = float.TryParse(text,out res);
            _input.Text = parsed ? res.ToString() : "0";
        }


    }
}