using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Vain.UI
{
    //This is the base class for the ui, should handle element initializations and their position in the tree
    
    //It behaves like a factory, initialization UIelements with the parameters they need through register/unregister
    public class UI 
    {

        Control _root;

        List<UIElement> _elements = new List<UIElement>();

        static UI _ui;

        public static UI Instance => _ui; 


        public UI(Control control)
        {
            _root = control;

            if(_ui == null)
                _ui = this;
        }

        //Loads scene with same name as UIElement if found, otherwise it loads a simple control node
        static public T Register<T>() where T : UIElement
        {
            

            const string PATH = "Scenes/UI";

            
            
            

            var scene = ResourceLoader.Load<PackedScene>($"{PATH}/{typeof(T).Name}.tscn");

            Control elementNode;

            if(scene != null)
            {
                elementNode = scene.Instance<Control>();

                Instance._root.AddChild(elementNode);


            }
            else
            {
                elementNode = new Control();
            }


            T element = Activator.CreateInstance(typeof(T),elementNode) as T;


            Instance._elements.Add(element);


            return element;
        }


        static void Unregister<T>(T t) where T : UIElement
        {
            var element = t as UIElement;


            
            element.Node.QueueFree();
            Instance._elements.Remove(element);



        }


        


    }
    
}