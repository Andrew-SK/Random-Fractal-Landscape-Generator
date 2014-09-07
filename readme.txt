The landscape pattern is generated within the diamondsquare method of the
landscape class this takes the form of a 2d array of float values
(landscape.heightMap[][]) , these points are then converted into an array 
of vertices using the x and y index values with scalling to determine the
 vertex position in world space and the values found in the array for the 
height values 

(VertexPositionNormalColor)  given a scale at which to disperse the points
 in the world space.

The colour of the terrain is calculated by taking the heighest, 30% of vertices 
and colouring them as snow, the next 2% down are coloured as rock.

Then the bottom 30% of vertices are covered in water, the remaining zone is
coloured green for grass.

( see Landscape.calcColourZones() )

The camera mouse and keyboard movement controls act similar to most modern
fps games, mouse controls the orientation of the camera using rotation 
martices to manipulate the target while the keyboard controls position of the
camera in the world either with translation matrices or vectors.

players are prevented from flying below the level of the terrain and also from
breaching the bounds of the landscape on the X and Z axis'.

the Direction of the Directional light is determined by trigonometric
functions, though it appears to change very slowly.

  
