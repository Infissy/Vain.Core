Intended behaviours
1. Area force curves projectiles
2. Area force can compress projectile (smaller but more powerful)
3. Very fast projectile can split slow projectile
4. Fire evaporates ice(creates pressure)



Params 
1. Pressure (force) (can change shape and damage)
2. State (gas, liquid, solid)
    1. Gas can ignite, change in temperature creates force and can some gas can ignite
    2. Liquid can split to area
    3. Solid can split into chunks, higher temperature transforms it into liquid

3. Temperature (base color, then higher temperature reddish1 color)





Tecniques :
1. All solid spells() as one big spell with different parametes, those different parameters are saved into resource as various spells

2. Make use of the viewport, create the hitbox based on the projection of the area
    2. That might require some adjustments in the engine
    2. Maybe create a damage buffer, get position inside collider and apply based on collision
        2. Position through globalposition, query for position only the characters inside the collider