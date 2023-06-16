using Godot;

namespace Vain.SpellSystem.Elemental
{   
    [Tool]
    public partial class ElementalParticleResource : Resource
    {
        [Export]
        public float Size {get;private set;}
        [Export]
        public Color Color {get;private set;}
        
        /// <summary>
        /// How much can the link withstand before breaking (for solid spell to break into pieces) 
        /// </summary>
        [Export]
        public float LinkStrenght {get;private set;}

        /// <summary>
        /// How much the link stretches based on force 
        /// </summary>
        [Export]
        public float LinkElasticity {get;private set;} 
        
        
        /// <summary>
        /// Describes how much the particle allows other particle to get inside it. 0 is infinitely dense, so acts like sand, higher the values higher should resemble a gas
        /// </summary>
  
        [Export]
        public float Elasticity {get;private set;}

        /// <summary>
        /// Attraction force (gravity) that the particle exerts to other particles
        /// </summary>
        
        [Export]
        public float AttractionForce {get;private set;}

        [Export]
        public float Mass  {get;private set;}


        [Export]
        public float Lifetime {get;private set;}


        internal ElementalParticle GetParticle()
        {
            return new ElementalParticle()
            {
                Radius = Size,
                Lifetime = Lifetime,
                Elasticity = Elasticity
            };


        }
    }
}