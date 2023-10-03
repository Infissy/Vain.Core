namespace Vain.Core
{
    //Rapresents every single tangible entity in Vain
    //Since entities can be of different types (Chracters/Spells/Spelldrop/etc) there is no way to have a single parent class since every entity inherits from a different node.
    //So every entity has to implement this interface to be accessed in the command sistem and more abstract stuff that will be implemented later.
    

    public interface IEntity
    {
        /// <summary>
        /// At the moment unused, will have some backend implementation in future versions
        /// </summary>
        /// <value></value>
        uint RuntimeID {get;}
    }

   
}