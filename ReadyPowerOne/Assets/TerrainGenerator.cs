using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Terrain {Open,Wall,Coal,Exit,Enemy1,Enemy2};  //Wall, Open, Coal, Exit
public enum Direction {Left, Top, Right, Bottom};

public static class RoomTools {

	private static System.Random rng = new System.Random();  

	//https://stackoverflow.com/questions/273313/randomize-a-listt
	public static void Shuffle<T>(this IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

	public static List<Terrain> solidRow(Terrain terrain, int width) {
		List<Terrain> row = new List<Terrain> ();
		for (int i = 0; i < width; i++) {
			row.Add (terrain);
		}
		return row;
	}

	public static SimpleRoom generateWallRoom(int width, int height) {

		List<List<Terrain>> layout = new List<List<Terrain>> ();
		for ( int y=0; y<(height - 0); y++ )
		{
			layout.Add (solidRow(Terrain.Wall, width));
		}
		return new SimpleRoom(layout);
	}

	public static SimpleRoom generateEmptySimpleRoom(int width, int height) {

		List<List<Terrain>> layout = new List<List<Terrain>> ();
		layout.Add (solidRow (Terrain.Wall, width));
		for ( int y=1; y<(height - 1); y++ )
		{
			layout.Add (solidRow(Terrain.Open, width));
		}
		layout.Add (solidRow (Terrain.Wall, width));

		SimpleRoom room = new SimpleRoom(layout);
		simpleRoomTraverser (room, (x, y, t) => {
			if (x == 0 || x == width || y == 0 || y == height) {
				return Terrain.Wall;
			} else
				return null;
		});
		return room;
	}

	public static void simpleRoomTraverser(SimpleRoom room, Func<int, int, Terrain, Terrain?> f) {
		List<List<Terrain>> currentLayout = room.Layout;
		for (int y = 0; y < currentLayout.Count; y++) {
			for (int x = 0; x < currentLayout [y].Count; x++) {
				Terrain? newTerrain = f (x, y, currentLayout [y] [x]);
				if (newTerrain != null) {
					currentLayout [y] [x] = newTerrain.Value;
				}
			}
		}
		room.Layout = currentLayout;
	}

	public static List<Vector2> exitFinder(SimpleRoom room) {
		List<List<Terrain>> currentLayout = room.Layout;
		List<Vector2> exits = new List<Vector2>();

		for (int y = 0; y < currentLayout.Count; y++) {
			if (y == 0 || y == currentLayout.Count) {
				for (int x = 0; x < currentLayout [y].Count; x++) {
					if (currentLayout [y] [x] == Terrain.Open) {
						exits.Add (new Vector2 (x, y));
					}
				}
			} else {
				if (currentLayout [y] [0] == Terrain.Open) {
					exits.Add (new Vector2 (0, y));
				}
				if (currentLayout [y] [currentLayout[y].Count - 1] == Terrain.Open) {
					exits.Add (new Vector2 (currentLayout[y].Count - 1, y));
				}
			}
		}
		return exits;
	}

	public static SimpleRoom mergeRoom(SimpleRoom parentRoom, RoomCoordinates childRoom) {

		List<List<Terrain>> parentLayout = parentRoom.getLayout ();

		foreach (TerrainCoordinates tc in childRoom.terrainCoordinateIterator()) {
			parentLayout [tc.Y] [tc.X] = tc.Terrain;
		}
		parentRoom.Layout = parentLayout;
		return parentRoom;
	}

	public static SimpleRoom mergeHallway(SimpleRoom parentRoom, Hallway hallway) {
		List<List<Terrain>> parentLayout = parentRoom.getLayout ();

		foreach (TerrainCoordinates tc in hallway.terrainCoordinateIterator()) {
			
			if (parentLayout [tc.Y] [tc.X] == Terrain.Wall) {
				parentLayout [tc.Y] [tc.X] = tc.Terrain;
			}
		}
		parentRoom.Layout = parentLayout;
		return parentRoom;
	}
}

public interface Room
{
	List<List<Terrain>> getLayout ();

	int getWidth ();

	int getHeight ();

	List<Vector2> getExits();

}

public class SimpleRoom : Room
{
	private List<List<Terrain>> layout;

	public List<List<Terrain>> Layout {
		get {
			return this.layout;
		}
		set {
			layout = value;
		}
	}
	public SimpleRoom (List<List<Terrain>> layout)
	{
		this.Layout = layout;
	}

	public List<List<Terrain>> getLayout() {
		return this.Layout;
	}

	public int getWidth ()
	{
		return Layout[0].Count;
	}

	public int getHeight ()
	{
		return Layout.Count;
	}

	public List<Vector2> getExits ()
	{
		return RoomTools.exitFinder (this);
	}
}

public class FunkyRoomFactory: RoomFactory {
	public override Room getRoom () {

		Terrain O = Terrain.Open;
		Terrain W = Terrain.Wall;

		List<List<Terrain>> funkyLayout = new List<List<Terrain>>();

		funkyLayout.Add(new List<Terrain> { W, W, W, W, W, W, W, W, W });
		funkyLayout.Add(new List<Terrain> { W, O, O, O, O, O, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, O, O, W, W, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, O, O, W, O, O, O, W });
		funkyLayout.Add(new List<Terrain> { O, O, O, O, W, W, O, O, O });
		funkyLayout.Add(new List<Terrain> { W, W, O, O, O, W, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, W, O, O, W, W, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, W, O, O, O, O, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, W, W, W, W, W, W, W, W });

		return new SimpleRoom(funkyLayout);
	}
}

public abstract class RoomFactory
{
	abstract public Room getRoom ();
}

public class RectangleRoomFactory : RoomFactory
{
	public int width;
	public int height;
	public int exitCount;

	public override Room getRoom ()
	{
		SimpleRoom room = RoomTools.generateEmptySimpleRoom (width, height);

		//Add exits iteratively across different walls

		List<Direction> doorOrder = new List<Direction> { Direction.Bottom, Direction.Left, Direction.Right, Direction.Top };

		doorOrder.Shuffle ();

//
//
//		Func<int, int, Terrain, Terrain?> exitApplier = (x, y, existingTerrain) => {
//
//		}

		//TODO: Don't create a door on an already existing door slot

		List<List<Terrain>> layout = room.Layout;

		System.Random rnd = new System.Random ();

		int idx;

		for (int i = 0; i < exitCount; i++) {

			Direction doorLocation = doorOrder [i % doorOrder.Count];
			switch (doorLocation) {
			case Direction.Top:
				idx = rnd.Next (1, layout [0].Count - 1);
				layout [0] [idx] = Terrain.Open;
				break;
			case Direction.Bottom:
				idx = rnd.Next (1, layout [layout.Count - 1].Count - 1);
				layout [layout.Count - 1] [idx] = Terrain.Open;
				break;
			case Direction.Left:
				idx = rnd.Next (1, layout.Count - 1);
				layout [idx] [0] = Terrain.Open;
				break;
			case Direction.Right:
				idx = rnd.Next (1, layout.Count - 1);
				layout [idx] [layout.Count - 1] = Terrain.Open;
				break;
			}
		}

		return room;
	}

	public RectangleRoomFactory (int width, int height, int exitCount)
	{
		this.width = width;
		this.height = height;
		this.exitCount = exitCount;
		//First create a block of all O, then add outside W; then add exits
	}
}

//public class RoomProbability() {
//	
//}

//Cause no tuples - heh
public class TerrainCoordinates {
	int x;
	int y;
	Terrain terrain;

	public TerrainCoordinates (int x, int y, Terrain terrain)
	{
		this.x = x;
		this.y = y;
		this.terrain = terrain;
	}
	public int X {
		get {
			return this.x;
		}
	}

	public int Y {
		get {
			return this.y;
		}
	}

	public Terrain Terrain {
		get {
			return this.terrain;
		}
	}
}

public class RoomCoordinates { 
	public static int padding = 1;

	public int x1 { get; set; } 
	public int y1 { get; set; } 
	public Room room { get; set; }

	private List<Vector2> exits = null;

	public int x2 () {
		return this.x1 + room.getWidth() + padding;
	}

	public int y2 () {
		return this.y1 + room.getHeight() + padding;
	}

	public RoomCoordinates(Room room, int x, int y) {
		this.x1 = x;
		this.y1 = y;
		this.room = room;
	}

	public Boolean doesCollide(int x, int y) {
		return this.doesCollide (x, y, x, y);
	}

	public Boolean doesCollide(int x1, int y1, int x2, int y2) {
		return (x1 < this.x2() && x2 > this.x1 && y1 > this.y2() && y2 < this.y1);
	}

	public Boolean doesCollide(RoomCoordinates otherRoomCoordinates) {
		return (otherRoomCoordinates.x1 < this.x2() && otherRoomCoordinates.x2() > this.x1 && 
			otherRoomCoordinates.y1 > this.y2() && otherRoomCoordinates.y2() < this.y1);
	}

	//Cache the exits
	public List<Vector2> getExits() {
		if (exits == null) {
			exits = new List<Vector2> ();

			foreach (Vector2 exit in room.getExits()) {
				exits.Add (new Vector2 (exit.x + this.x1, exit.y + this.y1));
			}
		}
		return exits;
	}

	public System.Collections.Generic.IEnumerable<TerrainCoordinates> terrainCoordinateIterator() {
		for (int y = 0; y < room.getHeight(); y++) {
			for (int x = 0; x < room.getWidth(); x++) {
				yield return new TerrainCoordinates (x + this.x1, y + this.y1, room.getLayout() [y][x]);
			}
		}
	}

}

public class Hallway {
	public int x1;
	public int y1;
	public int x2;
	public int y2;

	private static System.Random rng = new System.Random(); 

	public int X1 {
		get {
			return this.x1;
		}
	}

	public int Y1 {
		get {
			return this.y1;
		}
	}

	public int X2 {
		get {
			return this.x2;
		}
	}

	public int Y2 {
		get {
			return this.y2;
		}
	}

	public Hallway (int x1, int y1, int x2, int y2)
	{
		this.x1 = x1;
		this.y1 = y1;
		this.x2 = x2;
		this.y2 = y2;
	}

	public Hallway (Vector2 v1, Vector2 v2) : this((int)v1.x, (int)v1.y, (int)v2.x, (int)v2.y) {
	}

	public System.Collections.Generic.IEnumerable<TerrainCoordinates> terrainCoordinateIterator() {
		//Move between 0 and 5 spaces in direction of x1 to x2 (max at x2)
		//Then move between 0 and 5 spaces in direction of y1 - y2 (max at y2)
		//repeat until we're at x2,y2

		int xSign = Math.Sign (X2 - X1);
		int ySign = Math.Sign (Y2 - Y1);

		int currentX = X1;
		int currentY = Y1;
//		yield return new TerrainCoordinates (currentX, currentY, Terrain.Open);

		Boolean direction = true;  //true == x; false == y;

		while (Math.Abs(X2 - currentX) > 0 && Math.Abs(Y2 - currentY) > 0) {
			if (direction) {
				int deltaX = xSign * Math.Min (rng.Next (5), Math.Abs (X2 - currentX));
				for (int i = 0; i <= Math.Abs(X2 - currentX); i++) {
					currentX += xSign;
					yield return new TerrainCoordinates (currentX, currentY, Terrain.Open);
					direction = !direction;
				}
			} else {
				int deltaY = ySign * Math.Min(rng.Next (5), Math.Abs(Y2 - currentY));
				for (int i = 0; i <= Math.Abs(Y2 - currentY); i++) {
					currentY += ySign;
					yield return new TerrainCoordinates (currentX, currentY, Terrain.Open);
					direction = !direction;
				}
			}
		}

	}
}

public class TerrainGenerator : MonoBehaviour
{

	void Start() {
		Room room = GenerateLevel (100, 100, 0);
		List<List<Terrain>> layout = room.getLayout ();

//		Debug.Log (layout.ToString ());
//
//		Debug.Log ("potato");

		foreach (List<Terrain> row in layout) {
			List<String> strRow = new List<String> ();
			foreach (Terrain tr in row) {
				if (tr == Terrain.Open)
					strRow.Add ("O");
				else if (tr == Terrain.Wall)
					strRow.Add ("W");
				else
					strRow.Add (tr.ToString ());
			}
			String[] strArray = strRow.ToArray ();
			print (String.Join (",", strArray));
		}

		print ("potato");
	}

	private static int roomPlacementAttempts = 50;
	private static int roomPositioningRetries = 4;
	private static int maxRooms = 20;
	private static int levelBorder = 2;

	//For now needs to add up to 1
	private static Dictionary<double, RoomFactory> getRoomProbabilities(int level) {
		if (level == 0) {
			return new Dictionary<double, RoomFactory>()
			{
				{ 0.3, new RectangleRoomFactory(6,6,2)},
				{ 0.5, new RectangleRoomFactory(8,8,2)},
				{ 0.2, new FunkyRoomFactory()}
			};
		} else
			throw new NotImplementedException ();
	}

	public Room GenerateLevel(int width, int height, int level) {

		print ("beginning");

		//Cheat and use room generator and then keep layout
		SimpleRoom baseRoom = RoomTools.generateWallRoom (width, height);

		Dictionary<double, RoomFactory> roomProbabilities = getRoomProbabilities (level);

		System.Random r = new System.Random();

		//TODO: Merge room and roomCoordinates
		List<RoomCoordinates> placedRoomCoordinates = new List<RoomCoordinates> ();

		for (int i = 0; i < roomPlacementAttempts; i++) {
			double diceRoll = r.NextDouble();

			double cumulative = 0.0;
			RoomFactory selectedRoomFactory = new RectangleRoomFactory(4,4,4);  //Default room in case RoomProbabilities are borked

			foreach (KeyValuePair<double, RoomFactory> entry in roomProbabilities) {
				cumulative += entry.Key;
				if (diceRoll < cumulative)
				{
					selectedRoomFactory = entry.Value;
					break;
				}
			} 

			//Try to place each room N times
			Room roomToPlace = selectedRoomFactory.getRoom();

			int xMax = width - roomToPlace.getWidth () - levelBorder;
			int yMax = height - roomToPlace.getHeight () - levelBorder;

			for (int j = 0; j < roomPlacementAttempts; j++) {
				int roomX = r.Next (levelBorder, xMax);
				int roomY = r.Next (levelBorder, yMax);

				RoomCoordinates newRoomCoordinates = new RoomCoordinates (roomToPlace, roomX, roomY);

				Boolean roomOverlap = false;
				foreach (RoomCoordinates coor in placedRoomCoordinates) {
					if (newRoomCoordinates.doesCollide (coor)) {
						roomOverlap = true;
						break;
					}
				}

				if (!roomOverlap) {
					placedRoomCoordinates.Add (newRoomCoordinates);
				}
				if (placedRoomCoordinates.Count >= maxRooms)
					break;
			}
			if (placedRoomCoordinates.Count >= maxRooms)
				break;

		}


		//Time for some hallways!
		//Get list of all exits
		//Take random exit
		//Find closest of either nearest wall or nearest exit not from same room; 
		//if wall, do nothing; 
		//if exit draw one of several hallway patterns

		//Alternative

		//Connect random exit from every room to random exit in next room
		//Randomly connect a few more exits

		List<Hallway> hallways = new List<Hallway> ();

		//First add a sequential list of hallways from room to room so as to guarantee connections
		for (int i = 0; i < (placedRoomCoordinates.Count - 1); i++) {
			List<Vector2> exits1 = placedRoomCoordinates [i].getExits ();
			List<Vector2> exits2 = placedRoomCoordinates [i + 1].getExits ();

			exits1.Shuffle ();
			exits2.Shuffle ();

			hallways.Add (new Hallway(exits1[0], exits2[0]));
		}

		//Add a few more random hallways

		placedRoomCoordinates.Shuffle ();

		for (int i = 0; i < (1 + (placedRoomCoordinates.Count - 1) / 3); i++) {  //(1 + (placedRoomCoordinates.Count - 1) / 3)
			
			List<Vector2> exits1 = placedRoomCoordinates [i % placedRoomCoordinates.Count].getExits ();
			List<Vector2> exits2 = placedRoomCoordinates [(i + 1) % placedRoomCoordinates.Count].getExits ();

//			print ("exits");
//			exits1.Shuffle ();
//			exits2.Shuffle ();
//			print (exits1[0].x);
//			print (exits1[0].y);
//			print (exits2[0].x);
//			print (exits2[0].y);

			hallways.Add (new Hallway(exits1[0], exits2[0]));
		}

		foreach (RoomCoordinates rc in placedRoomCoordinates) {
			baseRoom = RoomTools.mergeRoom (baseRoom, rc);
		}

//		print ("hallways");
//		print (hallways.Count);
//		print (hallways[0]);
//		print (hallways[0].X1);
//		print (hallways[0].Y1);
//		print (hallways[0].X2);
//		print (hallways[0].Y2);
//		print ("internal");
//		foreach (TerrainCoordinates tc in hallways[0].terrainCoordinateIterator()) {
//			print ("tc: " + tc.X.ToString() + ", " + tc.Y.ToString ());
//		}
//		print ("hallways");
//		print (hallways.Count);
//		print (hallways[1]);
//		print (hallways[2].X1);
//		print (hallways[3].Y1);
//		print (hallways[4].X2);
//		print (hallways[5].Y2);
		foreach (Hallway hw in hallways) {
			baseRoom = RoomTools.mergeHallway (baseRoom, hw);
		}

		return baseRoom;
	}

}