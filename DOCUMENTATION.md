
![TIM_EntityRelationshipDiagram](https://github.com/Kerbaltec-Solutions/this_is_MOD-D-ABLE/assets/61379284/e9418a73-2ac7-4ae7-b63f-e128460d53ee)

a class diagram which sums up the different types of creatures 
and their attributes as well as some major operations

![TIM_classDiagram-1](https://github.com/Kerbaltec-Solutions/this_is_MOD-D-ABLE/assets/61379284/9f40a051-f6bf-41c9-9355-717f577dd6e7)

an Enity Relationship Diagram that shows the most important Objects of the project
and how they are related to one another

# Automated tests

Running automated tests on the game wasnt a useful option for us. 

Firstly, the game relies heavily on user interactions and real-time inputs, which makes it difficult to create automated scripts that could accurately imitate human behavior. 

Additionally, the game featured procedurally generated maps and entities, after all the goal of a game is to create unique situations and provide the player with different expiriences each time they are playing. This makes it difficult to find automated ways to test possible constellations, because they change constantly.

Moreover, there are a lot of small features in this project that would have to be tested seperately, which would result in a lot of small tests that dont provide meaningful information, because the real in-game situations are a result of the interplay of all components.

We were, however, capable of at least implementing a compilation test.

# Playtesting 

Instead we choose to conduct small tests on the general functionallity of a feature while working on it, to imediately see its interaction with the game. 

Frequent playing and trying to provoke unexpected behaviour on purpose, gave further insight into the weaknesses of the game and revealed some major bugs that could be fixed (for example that Entities teleported to the curser when targeting it). 

Of course it can't be guaranteed that there arent any bugs left, to find more one would have to conduct more tests with more participants (that are not currently working on the project) which still wouldnt rule out all errors.

Playtesting can also provide valuable information on the general user-expirience: the design of th UI, the games difficulty level and for example its overall performance.
For example, some functions that the player were only created because tests revealed an inconvinience that those functions improved.

We also found out that printing the game map one by one in the console tends to take too long on some devices. A possible future solution would be to design a proper GUI outside of the console. This could also improve the usability, for example by making navigation easier.

There is still a lot of room for improvements and Playtesting is has a key part in finding the direction in which the project should develop.
