using System;
using System.Linq;
using Godot;

namespace Vain.InteractionSystem.InteractionGraph
{
    
    internal class InteractionNodeBuilder
    {   
        GraphNode _node;


        public static InteractionNodeBuilder CreateNode(GraphNode template, string name)
        {   
            var interactionNodeBuilder = new InteractionNodeBuilder();

            

            interactionNodeBuilder._node = template;
            interactionNodeBuilder._node.Name = name;
            
            
            
            return interactionNodeBuilder;    
        }



        
       
        public InteractionNodeBuilder AddNumericField(string name)
        {
             var numericField = new NodeField<FloatType>();
            numericField.FieldName = name;
            _node.AddChild(numericField);

            return this;
        }
        

        public InteractionNodeBuilder AddInteractionField(string name)
        {
            var numericField = new NodeField<FloatType>();
            numericField.FieldName = name;
            _node.AddChild(numericField);
        }

        public Control BuildNode()
        {
            return _node;
        }
    }
}