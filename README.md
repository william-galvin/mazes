# [Mazes](https://gamepipe.io/@william-galvinxrk0/mazes-)
A Unity game with procedurally generated mazes.

![](https://github.com/william-galvin/mazes/blob/main/screenshots/Screenshot%202022-07-02%20173209.png?raw=true)
![](https://github.com/william-galvin/mazes/blob/main/screenshots/Screenshot%202022-07-02%20174012.png?raw=true)

## Preamble
This is a project I made when I was *very* new to programming, and was my introduction to Unity and C#—so the code and game design leave a lot to be desired, 
but I'm choosing to leave it unaltered. (Mostly as reminder to myself why commments are important.) 

Most of the interesting code that I wrote can be found in `generator.cs`—this project also includes lots of snippets and assets taken directly, or nearly so, from
tutorials and asset packs.

## Game Overview
The basic premise of this game is that there's a maze and that the player starts at the entrace and tries to find the exit. If that sounds riveting, 
you can play it [here](https://gamepipe.io/@william-galvinxrk0/mazes-).

The most interesting part of this project was procedurally generating and solving the maze so that a new random one loads every time the page is refeshed. The graphics, 
as you may be able to tell, were somewhat of an afterthought.

## Features
There are two view modes, third-person (default), and top-down. When you're in the third-person mode, there's a minimap in the corner with an abbreviated top-down version. There's also a headlamp on the character, which makes it possible to see your position and orientation from the top.

A help menu displays the game-playing intructions and mechanics, including that if you press `shift`, a line appears and guides you out of the maze. 

That's pretty much it—it's a *very* exciting game.

## DFS? Who's She?
I made the maze-solving functionality before I actaully learned about depth-first search and, as the fine TAs at UW (hey, that's me!) would say, the 
*choose-explore-unchoose* pattern. So my DFS is sort of like a recursive implementation of the iterative implementation of a DFS maze-solver. But, it works!

## Known Bugs
- Sometimes the maze solver doesn't display

## Thoughts and Reflections
I remember it was fun to make this game, but in hindsight, it's certainly very boring to play. In terms of documentation and readability, it has none.
