# High Level Design Document

Project Title: Become Pacman (Fake)

Document version: 0

Printing date: 26 Jan 2023

Group ID:

Group Member

    Terrance
    Department of Philosophy, The Chinese University of Hong Kong

    woo pok
    Department of Physics, The Chinese University of Hong Kong

    Thomas
    Department of Computer Science and Engineering, The Chinese University of Hong Kong

    Jay
    Department of Computer Science and Engineering, The Chinese University of Hong Kong

    1155143569 Mok Owen
    Department of Mathematics, The Chinese University of Hong Kong

Table of content
1. [Introduction](#introduction)
    - [Project Overview](#project-overview)
    - [System Featere](#system-feature)
2. [System Achitecture](#system-architecture)
    - [Technologies](#technologies)
    - [Architecture Diagram](#architecture-diagram)
    - [System Component](#system-components)

---

## Introduction

### Project Overview

This project is to create a Pacman game through Unity game engine. The infamous Pacman game is one side an arcade game to escape from the chasing of ghosts, but one side has the potential to become a dungeon-like horror game, such as dark deception. We aim at producing a brand new Pacman game inheriting the idea of traditional Pacman, but enhancing the experience by introducing horror features. The project will be built under the guidance of software development, and be evaluated by aspects of software engineering taught in the course CSCI3100 Software Engineering.

### System Feature

The system will feature:

1. 3-D constructions: Apart from traditional Pacman games, this project will introduce a 3-D maze with 3-D objects for advanced experience of Pacman. 
2. Props: Pacman will interact with props inside the maze. When Pacman eats special props, there will be buffs or debuffs according to the function of that prop. Usual beans will be placed as main props.
3. Third-person camera: The sight of player will no longer be a god view, but focusing on the surrounding of the Pacman. Players will eventually experience the feeling of being a Pacman.
4. Minimap: Similar to many RPG (Role-playing Games), a minimap will be shown on the screen for player to know where he is as player's sight is no longer a bird-eye view. Some information of the maze will be shown on the minimap, for enhancing player's experience.
5. Timer: A timer will be placed on top of the screen. The time will be used to calculate the score of the player and saved as a record.
6. Teleport: With different levels of the Pacman game, the length of the gameplay will be increased by inserting teleports. The teleports may bring players to a specific point, or act as a bridge between mazes.
7. Fog of war: As difficulties diverges, player may see only the sight of the Pacman, but no longer its surrounding. In particular, under the situation of fog, player has no ability to see what's behind the Pacman if he isn't facing in that direction.
8. Artificial intelligence of the ghost: Ghast might move in random direction so that player could easily escape from their chase. However, once the difficulty is being raised, the ghast may have the ability to trace player's movement. They could also reflect from histories to attack the frequently appear spot of the player, so no exact solution could be given to win the game easily.
9. Store system: As if most of the arcade games, player may buy characters or any items from store to enhance their in-game experience.

---

## System Architecture

### Technologies

This project will be produced using the following technologies:
- Game engine: Unity (Programming language: C#)
- Documentation: Markdown, Microsoft Word
- Database: Firebase

### Architecture Diagram

### System Components

### Features

- similar to dark deception

1. 3D
2. multiple props
3. third-personal view, follow camera
4. a minimap is showed on the top-right corner (show ghosts for 2 seconds (skill or props))
5. Timer
6. different (physical) levels ~teleport
7. 戰爭迷霧 (limited vision) (difficulites)
8. Advanced Ghost movements (record player past movements) instead of random
9. Buy Character