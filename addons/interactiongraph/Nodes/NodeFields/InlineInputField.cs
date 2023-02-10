using System;
using Godot;

namespace Vain.InteractionSystem.InteractionGraph
{

    abstract partial class InlineInputField<T> : NodeField<T> 
    {

        Contro



        protected abstract void TestField(string text);



        void TestInt(String text)
        {
            int res = 0;
            var parsed = int.TryParse(text, out res);
            _input.Text = parsed ? res.ToString() : "0";

        }


        void TestFloat(String text)
        {
            float res = 0;
            var parsed = float.TryParse(text, out res);
            _input.Text = parsed ? res.ToString() : "0";
        }


    }
}