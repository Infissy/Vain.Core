using System;
namespace Vain.InteractionSystem.InteractionGraph
{

    abstract class FieldType {}

    //Wrappers around custom type
    abstract class NumericFieldType : FieldType {}

    class FloatType : NumericFieldType {}

    class IntegerType : NumericFieldType {}

    class InteractionType : FieldType {}
}