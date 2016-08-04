# UnitSC2EffectSystem
Simple prove-of-concept of how an sc2-galaxy-editor like effect system could be implemented in unity3d

## Strukture

The System is found in Assets/Soraphis/EffectSystem. It has dependencys to the Assets/Soraphis/Lib folder.
So, to use it you'd use the whole Soraphis folder.

**Note:** Since i use [Alex Zhdankin's C# 5.0 and 6.0 integration](https://bitbucket.org/alexzzzz/unity-c-5.0-and-6.0-integration/overview) for every project of mine, the package has a depencie to this too. its already implemented in this demo project.
('Assets/CSharp 6.0 Support' + 'CSharp60Support' folders)

in "_Game" there is a example implementation.

## Usage

### Create a new Effect

Open the File Assets/Soraphis/EffectSystem/Template, copy and paste the content to your new file and do as the description in the first rows says.

## Example Project

In the Example Project is a Cube-Object. It has a "Click To Damage" Component, where you can specify an Effect, to be appliet (on itself) when the cube is clicked
Currently the missile effect does nothing and has no values. But there are 2 types of Damage Effects, one DoT and one instant damage effect.
