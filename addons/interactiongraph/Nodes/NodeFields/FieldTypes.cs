using System;
namespace Vain.InteractionSystem.InteractionGraph
{

    abstract class FieldType {}


    abstract class InternalFieldType : FieldType {}
    //Wrappers around custom type
    abstract class NumericFieldType : InternalFieldType {}

    class FloatType : NumericFieldType {}

    class IntegerType : NumericFieldType {}

    class InteractionType : FieldType {}
}