#4D SOCIETY

## Narrative & Sequence

_4D SOCIETY_ is a first person exploration game. You are an agent for PI, Paranormal Investigations, and have been assigned to a scouting job in a quarantined area. This area is documented to have eradicated everything in a 3km radius and turned it to black sand. Above the center of this area is a circular weather-like phenomenon that appears to pull in (or "swallow") matter.

Underneath this vacuum in the sky is a small block with an opening. At this point the player has control, and they go through this concrete entrance. They are instructed to do so through text on their screen that represents communication from their superiors. They notice the inside is a non-euclidean space, and is much larger inside than outside.

Indoors, there is a short sequence of minimal and simple brutalist corridors (foreshadowing for what's to come later), where the player loses contact with their superiors, and is also exposed to holographic text which describes, in some vague detail, the philosophy of the "4D SOCIETY".

Following this area, the player encounters a "cut" in reality, where the world breaks down. Through the holographic text, they are instructed that clicking left click will "reveal" parts of the world to them.

Wherever they click, the surfaces where their click "collides" or is aimed towards will reveal a part of the surface. All unrevealed surfaces have a "leak" effect, mixed with some noise to avoid the entirely glitchy effect of a leak, and instead make it appear more intentional, softer, and more painterly.

They now navigate this (non-euclidean?), glitchy environment through this discovery navigation system. At times this is broken up with more cuts in reality that bring the player back to the nautral world, feauturing more bruatlist architecture and narrative exposition through text.

A series of these cuts happens, and eventually the player is told that they are now an "agent" of the 4D SOCIETY, and they exit out of the cube they once entered, and are told to spread the word of their worldview.

## Technicalities

There are a few technical challenges to this project.

1. The real-world areas require light baking for realistic lighting, and each of these scenes need to be built carefully to ensure good quality, as well as reasonable bake times. This means that bake settings need to be set per-scene, assets of proper scale and sizing need to be created to not cramp too many texels into a small surface, or stretch only a handful of texels over a large surface.

2. Given the desire for seamless transitions between glitchy and real-world areas, a system of more streamlined scene loading needs to be created and implemeneted.

3. A series of custom shaders must be created to enforce the glitchy atmosphere aesthetic. Notably: a world-space shader that reveals surfaces where a player clicks, a screen-space shader that 'leaks' through not clearing the camera buffer where surfaces have not been revealed, and a shader that plays with culling to create the illusion of non-euclidean space (though through some clever level design, this could simply be done with teleportation).