## Outil simpliste de génération procédurale
L'outil de génération procédurale permet, comme son nom l'indique, la génération d'objets, salles, couloirs et emplacements procédurale donc aléatoire mais controlée dans un contexte numérique. 

Cet outil propose le choix entre 5 algorithmes ayant un but similaire mais une exécution différente : le Simple Room Placement, le Binary Space Partitioning, le Cellular Automata, la NoiseMap, et enfin une application du système de noise, le Terrain Generator.

Un prototype de terrain généré procéduralement et customisable est également proposé et sera expliqué plus loin.


## Tutoriel de génération / Setup sur Unity
Après avoir lancé le projet, il sera temps de choisir votre méthode de génération.
Pour faire ceci, il suffit d'aller sur Components/ProceduralGeneration et choisir le dossier de l'algorithme souhaité.

Vous allez donc remarquer la présence d'un script et d'un scriptable object. Il vous faut donc vous munir du scriptable object et le déplacer au champ dédié dans le Procedural Method Generator script, assigné au ProceduralGridGenerator positionné sur la hiérarchie.

Vous pouvez ainsi modifier vos paramètres à votre guise. Les paramètres seront ainsi affichés sur l'inspecteur.

### NAVIGATION :
- [Simple Room Placement](#Simple-Room-Placement)
- [Binary Space Partioning](#Binary-Space-Partioning)
- [Cellular Automata](#Cellular-Automata)
- [NoiseMaps](#NoiseMaps)
- [Terrain Generation](#Terrain-Generation)


## Simple Room Placement

### Fonctionnement : 

Cet algorithme vise à placer des rooms sur un sol, donc des salles de forme rectangulaire et de taille aléatoire mais controlée grâce aux paramètres MaxSize et MinSize, définissant ainsi les limites d'une room.
Si une room est génerée et sa position coincide avec une partie d'une room existante, elle n'est donc pas génerée et laisse place à la génération d'une autre room tant que le nombre d'étapes de génération maximal, ou le nombre de rooms maximal ne soit atteint. Ensuite, à la fin de la génération, des couloirs apparaîtront pour simuler une connexion entre les rooms, comme ci-dessous

<img width="485" height="486" alt="image" src="https://github.com/user-attachments/assets/1c0ec955-c8b7-4982-987d-4573828f3fe0" />

### Détails des paramètres : 
 - Max Size (Vector2Int) : Les dimensions maximales à une room.

 - Min Size (Vector2Int) : Les dimensions minimales à une room.
  
 - Max Steps (Int) : Le nombre d'étapes de génération maximal, comprenant donc la génération de rooms et de couloirs. En arrivant à cette limite, la génération s'arrête et le sol se génère.

 - Max Rooms (Int) : Le nombre de rooms maximal à générer. En arrivant à cette limite, les couloirs commencent à lier les rooms.

<img width="324" height="141" alt="image" src="https://github.com/user-attachments/assets/cd0f7a41-992d-466a-8f10-80ae60c8a4f1" />

## Binary Space Partioning

### Fonctionnement : 

Cet algorithme diffère du SRP dans son approche de l'aléatoire, malgré leur objectif totalement similaire. Le BSP se charge de diviser récursivement un rectangle de base, donc racine, en plusieurs petits rectangles (nodes) qui vont donc, à la rencontre de notre contrainte, devenir des leaves, donc ils ne pourront plus se séparer et donner naissance à 2 nouveaux rectangles. Après la fin de la génération des leaves, des couloirs seront déterminés en fonction du parent de deux leaves soeurs. Voici un petit visuel :

<img width="508" height="488" alt="image" src="https://github.com/user-attachments/assets/b1f50707-490c-43d1-9d5c-4a71f4820f21" />


### Détails des paramètres : 
 - Max Steps (Int) : Le nombre d'étapes de génération maximal, comprenant donc la génération de rooms et de couloirs. En arrivant à cette limite, la génération s'arrête et le sol se génère.
   
 - Horizontal Split Chance (Float) : Entre 0 et 1, c'est la valeur qui déterminera si la node va se séparer horizontalement ou verticalement, 1 étant une valeur sûre de séparation horizontale.

 - Split Ratio (Vector2Int) : Les coordonnées détérminant la séparation de la node.

 - Max Split Attempt (Int) : La valeur qui limite le nombre de séparation, donc de leaves.

 - LeafMinSize (Vector2Int) : Détermine la taille minimale d'une node pour devenir une leaf.

 - RoomSize (Vector2Int x2) : Détermine les limites minimum et maximum de la taille d'une room.

<img width="546" height="220" alt="image" src="https://github.com/user-attachments/assets/338fc345-3fef-4102-ae4c-d76495be89ff" />



## Cellular Automata

### Fonctionnement : 

Cet algorithme, comparé aux 2 autres, génère simplement un sol avec des cellules en forme de carré et colorées différement selon la densité du noise (bleu pour eau, et vert pour sol) : plus on maximise la noise density, plus il y aura une chance de générer une cellule de sol. Ensuite certaines contraintes s'appliquent en fonctionnement de ce qui entoure chaque cellule. Cela va donc déterminer l'identité d'une cellule.

<img width="528" height="79" alt="image" src="https://github.com/user-attachments/assets/021543d8-69d3-4d2e-89e9-5b87a06a4d23" />


### Détails des paramètres : 
 - Max Steps (Int) : Le nombre d'étapes de génération maximal, comprenant donc la génération de rooms et de couloirs. En arrivant à cette limite, la génération s'arrête et le sol se génère.
 - Ground Density : c'est la noise density, ce qui détermine la chance de spawn une cellule de sol.
 - Min Ground Neighbour Count : le nombre minimum de cellules terrestres voisines pour changer la cellule en sol.

<img width="323" height="324" alt="image" src="https://github.com/user-attachments/assets/d94c9b6d-689a-424c-8816-5e8cb5a18458" />



## NoiseMaps

### Fonctionnement : 

Cet algorithme génère un terrain 2D avec des heightmaps en utilisant du noise. Le concept du noise génère un array de floats qui vont former des noise density par cellule de la grille/terrain. Ainsi, le noise est facilement modulable avec une panoplie de paramètres. Les différents terrains/couleurs d'une cellule seront donc déterminés avec une comparaison entre le noise et la height du terrain choisi, donc en crescendo eau, herbe, sable et roche. 

<img width="474" height="302" alt="image" src="https://github.com/user-attachments/assets/8404cbe6-c8e6-4bb5-904b-1f8cb64bc9e0" />


### Détails des paramètres : 
 - Max Steps (Int) : Le nombre d'étapes de génération maximal, comprenant donc la génération de rooms et de couloirs. En arrivant à cette limite, la génération s'arrête et le sol se génère.

 - Noise Type (FastNoise.NoiseType) : les plus utilisés étants le OpenSimplex2 et le Perlin.

 - Fréquence (float) : Un effet de zoom, manipulant l'échelle du noise pattern.

 - Amplitude (float) : Un effet dramatique. Plus l'amplitude est haute, plus l'hauteur se met en relief.

 - Octaves (Int) : Un nombre d'octaves est le nombre de couches que possède le noise. Plus d'octaves, plus de détails.

 - Lacunarité (float) : Module la fréquence entre les octaves.

 - Persistance : Module l'amplitude entre les octaves.

 - Heights : La comparaison entre les heights et le noise détermine ainsi la probabilité de spawn une cellule d'un certain terrain.

<img width="322" height="323" alt="image" src="https://github.com/user-attachments/assets/6b2bd036-7b32-4f7c-b741-f66b3d86189e" />

### TERRAIN GENERATOR : 

Cet algorithme utilise la même logique que les noisemaps ainsi que les mêmes paramètres, mais à une exécution sur un terrain 3D, ici modelant les reliefs du terrain.

<img width="448" height="424" alt="image" src="https://github.com/user-attachments/assets/5af28189-ab4f-4f96-804d-cc7976f85e65" />


<img width="682" height="423" alt="image" src="https://github.com/user-attachments/assets/ec60f2fe-fd7b-4202-abff-b8050435707b" />





<!-- GamePlan : 
Explain the program
Talk about the use of each method
Talk about their parameters if applicable
Recommended Settings/Scriptables if applicable
Show examples -->

<!-- Comments are here, below is some format info -->

<!-- 
For dropdown menus : 
<details>
<summary>Details</summary>

  - [Introduction](#introduction)
  - [Get Started](#Get-Started)
    - [How to install](#How-to-install)
    - [Using Scriptable Object](#Using-Scriptable-Object)
    - [How to use editable variables](#How-to-use-editable-variable)
  - [Features](#Features)
  - [Documentation](#Documentation)
  - [Misusing / Limitations](#Misusing-/-Limitations)
  - [License](#License)


For images : 
<img width="1881" height="942" alt="image" src="https://github.com/user-attachments/assets/90596d99-89f8-4239-bead-6ae489d08ad2" />

For links : 
- [Download UnityHub](https://unity.com/download)

For code snippets : 
```csharp
private readonly Cell[,] _gridArray;       
private readonly List<Cell> _cells;

public Vector3 OriginPosition { get; }
public float CellSize { get; }
public int Width { get; }
public int Lenght { get; }
public IReadOnlyList<Cell> Cells => _cells;
```
-->
