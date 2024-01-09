# Axe Attacks
Axe Attacks is a first person dungeon escape demo.
The player needs to go through 5 levels. On this path, the player will be met with ninjas, who will try to stop him in his tracks. He can choose to destroy them, or not.
## Assets
The NPCs (ninjas) plus their animations are from [Mixamo](mixamo.com) and the axe is made by my friend, a great 3D artist, [Jovan Ivanov](https://www.linkedin.com/in/jovan-ivanov-077568181/). Everything else, including textures and animation programming, plus the rooms, were made by me.
## Code
Let's start talking about the Demo Code.
### Architecture
The game itself has 5 unique states.
- Start state (Main menu screen)
- Play state (Main game, all of the game logic is here)
- Gameover state (Player death, restart of levels)
- Level transition state (Player got to the end level portal)
- Last but not least, Victory state

We will be talking only about _Playstate_, as there is all of the game logic, other states are just scene transitions.
### Play state (scene)
#### Level Maker
Lets start from the Dungeon creation.\
We always start from the base room, named "0".
Each room has 4 holes (possible new rooms), and we check each.\
If we skip all remaining holes, but there are more rooms to be made, we just reset all skipped holes to empty, and start again, but now with a bigger chance of making a room.\
All other holes are filled with prefabs that match the hole size.\
With this, we always get a randomized level layout with given amount of rooms.\
We check if the current hole that we're checking is full on multiple levels. But mainly, we spawn **2 spheres**, 1 small: to check right in front of the hole, if we started from this place, or if another room made a room right there, and 2nd, bigger room, if there is a room that is not aligned exactly with our current room. On the first check, we leave the hole open, and on the second, closed, as player could drop out of the level.\
After we have made all of our rooms, we fill the remaining holes and move on.
#### NPC Maker
So, we have the level layout. The rooms are connected and spawned. Now its time for enemies.\
We do this in 2 parts. First part, is creating information about all the NPCs that are going to be spawned. Every room, based on its size, has a maximum amount of NPCs it can store. And because every room is required to be rectangular, it will always have good spawn points for them. There is no minimum or required amount of NPCs, as rooms can be empty. I decided to leave to chance if the level is going to be crawling with enemies, or if it's going to be empty.\
So, the 1st part of NPC spawning is getting each NPCs spawn coordinates and IDs.\
After this, we just instantiate each NPC to the given coordinates and give them names based on their ID (I forgot to say this, but it is the exact same way of naming the rooms).\
Later, its easier to find the exact NPC/room you need, if you can just parse the child object name of their parent object ("rooms" or "NPCs") and instantly get the ID of the wanted object.
#### The player
I'm using the built in Player Controller object for player, which includes the collider and first person camera. The axe asset is glued to the camera, and added animation to trigger every time the user attacks. Movement and gravity are also very trivially defined, although it took some polishing time. Definitely on the mouse movement part.
#### NPC movement
NPCs use Unity's built in NavMesh, which is instantiated after Dungeon is created.\
So, in between the Room making and NPC spawning.\
Which also took some time, as the NPCs were spawning on the walls for the most time for some reason!
#### NPC Logic
I made a Monobehaviour state machine for each NPC. They have these states: _idle_, _chase_, _combat_ and _return_.
Each state does exactly what the name implies.\
_Idle_ is when the NPC is spawned, or when it returns to the original point and doesn't interact with the player in any way.\
_Chase_ is when the player gets close enough for the NPC to notice the player, but not close enough for the NPC to attack the player.\
_Combat_ is when the NPC actually starts attacking the player.\
_Return_ is when the player has ran far enough from the NPC, that the NPC starts returning to its original coordinates.
#### Combat system
Very basic, but also tied to the animations. \
NPCs & Player have their own Combat Monobehaviour attached. NPCs keep track of their Animations, and the distance between them & the player. \
The player on the other hand, shoots a raycast from the camera center, with a short range, and check if it hit anything. If it did, it gets that NPCs info, and sends a bool to trigger a _hit_ method, which also triggers _hit_ animation. \
So, NPCs will deal damage to the player if in range, every _animation duration_ seconds, and player will deal damage if the raycast hit, and the axe animation is on standby.
#### Winning / Losing the game
We start the game at the Main Menu screen, through which, we can start the game. \
When the game loads, makes the level with the amount of rooms tied to the level number, and spawns the NPCs, we begin. \
We need to find the portal, which was spawned in the latest made room, which doesn't have to be the furthest away one! \
This portal, gets us to the next level, which will be 4x level bigger. Unless, the ninjas get us! Then we go back to the main menu, and start from the level 1. \
If we manage to get to the portal 5 times, we go to the victory screen, and the game is over. We can always do it again, and see what the random level generator has in store for us!
#### Final Thoughts
This is in no way the final version of this project, I want to continue updating it in the future, especially the Dungeon Generation code, which is a bit messy, as it was the first part I wrote. \
Thank you for checking out this project of mine, it took me a couple of months to make all of this, as I also tried making my own NPCs, but it didn't work out. \
You can find me on [LinkedIn](https://www.linkedin.com/in/branko-dinic-928b83243/).
