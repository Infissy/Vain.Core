using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;


using Godot;

using Vain.InteractionSystem;
namespace Vain
{

    //TODO: ? maybe rename, bad name
    public partial class PlayableGame : Node
    {  
 
        //Searches for attributes up to 3 levels down
    const int MAX_LEVEL_DEPTH_ANALYISIS = 4;
        
        List<Node> singletonList = new List<Node>();

        public override void _EnterTree()
        {
            base._EnterTree();

            exploreTree(this,0);

            singletonList.ForEach(singleton => SingletonManager.Register(singleton));


        }

        void exploreTree(Node parent,int level)
        {
            if(level < MAX_LEVEL_DEPTH_ANALYISIS)
            {
                filterNode(parent);
                foreach (var child in parent.GetChildren())
                {
                    exploreTree(child,level + 1);
                }    
                
            }
        }


        void filterNode(Node node)
        {
            //Check only for node with script
            if(node.GetScript().Obj != null)
            {
                var nodeType = node.GetType();

                //If node is singleton
                if(nodeType.GetCustomAttribute<SingletonAttribute>() != null)
                {

                    singletonList.Add(node);
                    
                }

            }






        }

    }
}