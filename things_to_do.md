
Movable component needs to tell the mesh componemnt where to place itself, make the system for that kind of information sharing


For now the spell generation is weird, there is spellbehavior which is the component that dictates the main spell "projectile" ie, spellcaster which is the entity able to cast such spell, and spelldrop, which is the drop entity
Wherever spellcaster goes on top(for now) of a spelldrop he picks up sharing resource information
Maybe make a spellcontroller on which entities can query the spell, having all resource handlign separated


