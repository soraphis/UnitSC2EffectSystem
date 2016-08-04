# UnitSC2EffectSystem
Simple prove-of-concept of how an sc2-galaxy-editor like effect system could be implemented in unity3d

## Strukture

The System is found in Soraphis/EffectSystem. It has dependencys to the Soraphis/Lib folder.
So, to use it you'd use the whole Soraphis folder.

in "_Game" there is a example implementation-

## Usage

### Create a new Effect

Open the File Soraphis/EffectSystem/Template, copy and paste the content to your new file and do as the description in the first rows says.

## Example Project

In the Example Project is a Cube-Object. It has a "Click To Damage" Component, where you can specify an Effect, to be appliet (on itself) when the cube is clicked
Currently the missile effect does nothing and has no values. But there are 2 types of Damage Effects, one DoT and one instant damage effect.
