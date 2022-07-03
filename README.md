# mazes
A Unity-based game with interesting mazes
![](https://imgur.com/lLBGXeU)

## Preamble
This is a project I made when I was *very* new to programming and was my first introduction to Unity and C# (more on that below). 
Most of the interesting code that *I* wrote can be found in `generator.cs`.

## Game Overview
The basic premise of this game is that there's a maze and that the player starts at the entrace and tries to find the exit. If that sounds riveting, 
you can play it [here](https://gamepipe.io/@william-galvinxrk0/mazes-).

## Internals
The most interesting part of this project was procedurally generating and solving the maze so that a new random one loads every time the page is refeshed. The graphics, 
as you may be able to tell, were somewhat of an afterthought. 

## DFS? Who's She?
I made the maze-solving functionality before I actaully learned about depth-first search and, as the fine TAs at UW (hey, that's me!) would say, the 
*choose-explore-unchoose* pattern. So my DFS is sort of like a recursive implementation of the non-recursive implementation of a DFS maze-solver. But, it works!

## Thoughts and Reflections
I remember it was fun to make this game, but in hindsight, it's certainly very boring to play. In terms of documentation and readability, it has none.
