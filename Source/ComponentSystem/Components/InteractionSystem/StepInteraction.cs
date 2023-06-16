namespace Vain.InteractionSystem
{
    //Simple interaction doesn't require state 
    internal abstract partial class StepInteraction : Interaction 
    {
        internal abstract Interaction RequiredInteraction {get;set;}

    }    
}