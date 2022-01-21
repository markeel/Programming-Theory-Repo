
# Game Design for Programming Theory

Game to show use of Inheritance, Polymorphism, Encapsulation, Abstraction

That means there needs to be things that are other things, or we can't do inheritance.  There need to be
things that all these things do, but that some do differently or there can't be polymorphism.  There needs to be controls
on the use of these things so that access to data is controlled (providing encapsulation).  There needs to be common elements
that can be shared (abstracting actions).

If we went really simple there could just be shapes that represent pieces moved on a board (sorta like chess pieces) that each
have some common methods (for instance how the piece can move within the context of a larger movement algorithm).  

# Prototype

What about a game on a 6x6 grid.  There are 3 player pieces.  A sphere, a cube, and a cylinder.  These are all pieces.  
There are 2 environment pieces a missle and a launcher.

GameManager
  - Loop
    - Wait for player to pick a position
    - Move player piece to picked position
    - Move all environment pieces until no more moves left
       - Give some time for the piece to move into position
       - A little animation (co-routine or just on update?)

Tile
    - Occupied()
    - IsSolid();

NormalTile : Tile

HoleTile : Tile
    - IsSolid() : false

Piece
  - prefab
  - destroyable()
  - destroy()
  - recharge()
  - move()

EnvironmentPiece : Piece
  - canMove()
  - move()

Launcher : EnvironmentPiece

Missle : EnvironmentPiece

PlayerPiece : Piece
  - allowed()  - abstract method that indicates what pieces can be moved where
  - move()     - accepts a click on the piece, to select, as mouse moves around the squares highlight based on 
                 whether it is allowed.  If clicking again when allowed, then 
  
Sphere : Piece
  - allowed()  - sphere can be rolled to any adjacent spot (horizontally, vertically, or diagonally) that is not occupied by
                 another piece, a missle launcher, or a hole.

Cube : Piece
  - allowed()  - can go horizontally or vertically but not diagonally and not into a hole or launcher

Pyramid : Piece
  - allowed()  - can go diagonally only and not into another piece, hole or launcher
  
There are a random number of "holes" on the 6x6 grid that prevent access.  
  
The pieces start out on bottom edge.  
If there is not a missle, a launcher appears randomly on the top edge and shoots a missle toward the sphere.
- The missle replaces the launcher on the next turn and begins its 2 space moves.
Every turn there is a missle it moves toward the sphere. 
A missle moves 2 squares in any direction every turn.
- goes over holes, 
- if it hits a cube the missle is destroyed but the cube is unaffected
- If it hits a cylinder the missle is destroyed but the cylinder is destroyed as well
- If it hits a sphere the game is lost
If a player piece moves onto a missle the same action is taken as if the missle moved onto it.

If the sphere makes it to the top row, the game is won.


    [_][_][_][_][_][_]
    [_][_][_][_][_][_]
    [_][_][_][_][_][_]
    [_][_][_][_][_][_]
    [_][_][_][_][_][_]
    [_][_][_][_][_][_]

