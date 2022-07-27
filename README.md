# Kalkuz Systems
Kalkuz Systems is a Custom Package for Unity which aims reusability, faster productions times and scalability for both strangers 
who are going to use this for free and the developer itself. You are welcome to use the package for free.
## Disclaimer
- This package is in development, not ready to use yet as it has fluctuating changes.
- This package does not provide you a full game-maker interface. You will most probably need to write down code extending from the package's own scripts or 
understand the working principles if you want to create unique gameplay features.
## Contains
### 1. Skill System
The Skill System is designed using Scriptable Object architecture and has a variety of different skill types. The system is designed to be customizable at runtime which means
there are skill and skill attachments which are going to empower the main skill. Also the structure can be used in static perspective to maintain the skill features.
To be more precise, there are some examples what you can build using the architecture:
- Archero-like games (By using the dynamic capability of the system.)
- MOBA (By using the static definition of the system. Also the package contains several examples for LoL characters.)
- Skyrim-like (System can be used with Third Person perspective.)
- Diablo-like (Especially the system is very similar to Path of Exile's skill gem system.)
### 2. Buff System
The Buff System can work by itself but it is strongly suggested to be used with skill system. The properties of the buff system is as follows:
- Empowering the character. (Speed gain for a short time, heal overtime, increase resistance etc.)
- Crowd control over a character (Knockback, Damage overtime, Slow down, Stun etc.)
- Manage the summons that come from Skills (For ex. controlling the count of maximum summons and referencing their summoner.)
- Hold permanent status effects (Similar to Rogue-like games' curse or bless features.)
### 3. Grid System (Currently Only 2D rectangular grids)
Grid System is a basic grid and management of this grid to provide an environment for grid-based games. What can be done using this is:
- City builders (SimCity, Cities: Skylines) 
- Terraria-like block based worlds (Terraria, Starbound, Oxygen Not Included etc.)
- Tower defence 
- Turn-based Isometrics (Divinity : Original Sin 2 etc.)
- Automation & Colony Games (Factorio, RimWorld etc.)
### 4. Item & Inventory & Crafting System 
There are options that creating items or managing an inventory with these items as well as creating recipes and craft items. Games can be an example of these:
- Every game containing inventory, especially RPGs
- Match-craft games (Doodle God, Merge games on mobile)
### 5. Utility Scripts
The package contains utility functions, extensions and more to minimize coding effort by decreasing long-line operations to a method call, or providing frequently used functions
like "Hit Stop" and Filling the gaps where Unity's built-in methods lacks such as Physics2D.SectorCast or Physics.ConeCast
