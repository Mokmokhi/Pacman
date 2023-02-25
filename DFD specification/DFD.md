# DFD Specification Document

Project Title: Become Pac-Man (Fake)

Document version: 0

Printing date: 26 Jan 2023

Group ID: F8

Group Member

    1155127434 HO Chun Lung Terrance
    Department of Philosophy, The Chinese University of Hong Kong

    1155143519 WOO Pok
    Department of Physics, The Chinese University of Hong Kong

    1155157839 NG Yu Chun Thomas
    Department of Computer Science and Engineering, The Chinese University of Hong Kong

    1155157719 LEUNG Kit Lun Jay
    Department of Computer Science and Engineering, The Chinese University of Hong Kong

    1155143569 MOK Owen
    Department of Mathematics, The Chinese University of Hong Kong

## High Level Context Diagram

> Our game data has the flow of above:

> At first, the users need to input their username and password into the login procedure, and the input data will be matched with the Game Database to authorize the user. After that, the authorized user will be directed to the UI Manager. The details will be described at UI DFD later. The UI Manager receives user's input and data from Game Database and Game Local Files, and output user's activities to Game Flow to run the game. The details of Game Flow will be described later as well. The functions mentioned above will all output graphic data to Rendering in the Unity engine and output as visible graphic to the Game Window for users to view.

### User - Software interaction Diagram

> The above diagram describe the general interaction between the user's input and the software. The user's input will be converted to game variables by the scripts stored in the Game Assets, and stored to the local memory. The memory has data exchange between the Game Local Files and the Game Database. While the scripts in the Game Assets are running, they receive and also write game data to the memory, which can also be sednt to the Game Local Files and the Game Database. Running scripts will send game data to the Unity engine, and media files in the Game Assets like images, videos or animations, will be loaded to the engine too. These variables will be converted to render variables and output as graphic data to the renderer to do rendering. Lastly, the actual graphic will be shown on the Game Window to the user.

## Feature Diagrams
### Game Flow

<img src="Pictures\DFD\gameflow_DFD.png" alt="picture" width="400"/>

> The Sign-up function is to create a new datafile for new users of the game, while the Login function is to identify who is accessing the application and provide security check for the user datafile.

> The game procedure is to integrate the gameplay from level selection and game setting for each user.

> The setting overwrite function is to manage the in-game experience for users.

> The shop function is to manage items of a user.

> The record update function is to update the game record to the database after each gameplay.

> The show record function is to display statistics of users and the whole game.

## UI DFD

### UI Rendering

The UI Manager handles input and output of the UI. For example, during game, a 'game end' event can be invoked when a player choose to quit the game after pausing or a ghost killed the pacman. For the first case, a button UI element calls UI Manager to end a game; for the second case, an other system calls UI Manager. After that, the UI Manager will choose to show and render the UI panel by calling the SwitchPanel() function.

UI panel list stores a list of displyable panels where a panel contains a list of UI elements. The panel list may contain panels such as title main screen panel, shop panel and game pause panel. A UI panel may contains elements such as button, text and slider.

### Title screen UI

As mentoned before, UI Manager handles input and rendering of UI. The UI Manager may call different functions according to the inputs. Change game setting function change the player game setting data such as music volume and graphic setting. It will update the Game Setting List which stores all player setting on the database. 

Our Pacman has a shop system where player can buy skin or other game items. The game item list stores all game item while user profile list stores all user profile. A user profile contains the player's owned game items and score.

A record list stores all players highest score. the world record will be displayed on the title screen for players to compare.

Everytime when the game is started, it will fetch the player game setting and the player profile for the game rendering.

### In-game UI

During game, a game event, such as game pause and game end, can be called by different systems, like UI Manager and Player Controller. For instance, a game end event can be invoked by the UI Manager when a player clicked the end button in pause panel or by the Player Controller when a player health is below 0. UI Manager handles all the game events in game and react according to it. 

When game is running normally, there are also data needed to be rendered, such as player health and scores. Those data will be fetched from Game State and Player State and keep updating. 

If a player would like to change setting in game after pausing, the new game setting data will be passed to the change game setting function and it would update the Game Setting List Database. When the game resumes, it will first update the game setting and then continue the game.

## Player Control

For the data flow of the player control, the game will receive signals from the user's input device and perform different actions. For example, the user can move Pacman and change its vision using a keyboard and mouse, also one can pause the game by pressing the "Esc" key. On moving, the game checks for any collisions to update the player's state, such as collisions with ghosts will decrease lives, collisions with power pellets will enter the powered state, and collisions with walls refuse the player to move along the direction. The player state (transform, "isPowered", remaining pellets, etc.) will be saved and output to the engine.

## Ghost AI

During the game, for each ghost object, it will perform all of its action based on a central function "Ghost decision maker". This function will take data from two storages: First, it will take the coordinates, Ghost ID, and ghost state (alive or dead, or other states) from the data store "Ghost object", which should be created for each ghost by each match; Secondly, it will take the game difficulty from "difficulty setting" to consider ghost action complexity.

With the above data, the Ghost Decision Maker will anticipate any input events received by the ghost object, including: Wall collision, Crossroad encounter, Player sighted, and Player collision. From each of the five event inputs, the Decision Maker will output its action based on the data from Ghost object and Difficulty Setting, and output the movement, Ghost ID, and Ghost state of the ghost object into a ghost action. It can also ouput a damage value as the Player Damage (such as when colliding with a non-powered player). For each output of the Decision maker, it will also output an Update that will update the data store of Ghost Object. As for when encoutering a Powered player Collision event, this will directly update the Ghost state of the object from "alive" into "dead" as a way to kill the ghost, the Decision maker will then output Ghost actions based on this new state (such as stopping and playing death animation.)