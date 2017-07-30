using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Terrain {Open,Wall,Coal,Exit,Enemy1,Enemy2,BossEnemy};  //Wall, Open, Coal, Exit
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

//	public static List<Vector2> exitFinder(SimpleRoom room) {
//		List<List<Terrain>> currentLayout = room.Layout;
//		List<Vector2> exits = new List<Vector2>();
//
//		for (int y = 0; y < currentLayout.Count; y++) {
//			if (y == 0 || y == currentLayout.Count) {
//				for (int x = 0; x < currentLayout [y].Count; x++) {
//					if (currentLayout [y] [x] == Terrain.Open) {
//						exits.Add (new Vector2 (x, y));
//					}
//				}
//			} else {
//				if (currentLayout [y] [0] == Terrain.Open) {
//					exits.Add (new Vector2 (0, y));
//				}
//				if (currentLayout [y] [currentLayout[y].Count - 1] == Terrain.Open) {
//					exits.Add (new Vector2 (currentLayout[y].Count - 1, y));
//				}
//			}
//		}
//		return exits;
//	}

	public static List<Vector2> exitFinder(SimpleRoom room) {
//		List<List<Terrain>> currentLayout = room.Layout;
		List<Vector2> exits = new List<Vector2>();

//		for (int y = 0; y < currentLayout.Count; y++) {
//			if (y == 0 || y == currentLayout.Count) {
//				for (int x = 0; x < currentLayout [y].Count; x++) {
//					if (currentLayout [y] [x] == Terrain.Open) {
//						exits.Add (new Vector2 (x, y));
//					}
//				}
//			} else {
//				if (currentLayout [y] [0] == Terrain.Open) {
//					exits.Add (new Vector2 (0, y));
//				}
//				if (currentLayout [y] [currentLayout[y].Count - 1] == Terrain.Open) {
//					exits.Add (new Vector2 (currentLayout[y].Count - 1, y));
//				}
//			}
//		}
		exits.Add(new Vector2(room.getWidth() / 2, room.getHeight() / 2));

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

	virtual public List<Vector2> getExits ()
	{
		return RoomTools.exitFinder (this);
	}
}

public class SimpleRoomWithExits: SimpleRoom {

	private List<Vector2> exits;

	public SimpleRoomWithExits(List<List<Terrain>> layout, List<Vector2> exits): base(layout) {
		this.exits = exits;
	}

	override public List<Vector2> getExits ()
	{
		return exits;
	}
}

public class FunkyRoomFactory: RoomFactory {
	public override Room getRoom () {

		Terrain O = Terrain.Open;
		Terrain W = Terrain.Wall;
		Terrain E = Terrain.Enemy1;
		Terrain I = Terrain.Enemy2;
		Terrain C = Terrain.Coal;

		List<List<Terrain>> funkyLayout = new List<List<Terrain>>();

		funkyLayout.Add(new List<Terrain> { W, W, W, W, W, W, W, W, W });
		funkyLayout.Add(new List<Terrain> { W, O, O, O, O, O, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, E, O, O, O, I, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, O, W, O, W, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, O, O, C, O, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, O, W, O, W, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, I, O, O, O, E, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, O, O, O, O, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, W, W, W, W, W, W, W, W });

		return new SimpleRoomWithExits(funkyLayout, new List<Vector2> {
			new Vector2(1, 1), new Vector2(7,1), new Vector2(7, 1), new Vector2(7, 7)
		});
	}
}

public class ComplexRoomFactory: RoomFactory {
	public override Room getRoom () {

		Terrain O = Terrain.Open;
		Terrain W = Terrain.Wall;
		Terrain E = Terrain.Enemy1;
		Terrain I = Terrain.Enemy2;
		Terrain C = Terrain.Coal;
		Terrain B = Terrain.BossEnemy;

		List<List<Terrain>> funkyLayout = new List<List<Terrain>>();

		funkyLayout.Add(new List<Terrain> { W, W, O, W, W, W, W, W, W, W, W, W, W, O, W, W });
		funkyLayout.Add(new List<Terrain> { W, O, O, O, O, O, O, O, O, O, O, O, O, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, E, O, O, O, I, O, O, O, O, O, O, E, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, O, O, O, O, O, O, O, O, O, O, O, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, W, O, W, W, W, O, B, O, B, W, W, W, O, W, W });
		funkyLayout.Add(new List<Terrain> { W, W, O, W, W, W, O, O, C, O, W, W, W, O, W, W });
		funkyLayout.Add(new List<Terrain> { W, W, I, W, W, W, W, W, W, W, W, W, W, I, W, W });
		funkyLayout.Add(new List<Terrain> { W, W, O, W, W, W, W, W, W, W, W, W, W, O, W, W });
		funkyLayout.Add(new List<Terrain> { W, W, O, W, W, W, W, W, W, W, W, W, W, O, W, W });
		funkyLayout.Add(new List<Terrain> { W, W, O, W, W, W, W, W, W, W, W, W, W, O, W, W });
		funkyLayout.Add(new List<Terrain> { W, O, O, O, E, O, W, W, W, W, O, O, O, E, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, O, O, O, O, W, W, W, W, O, O, O, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, O, O, O, O, O, O, O, O, O, O, O, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, W, W, W, W, W, W, W, O, W, W, W, W, W, W, W });

		return new SimpleRoomWithExits(funkyLayout, 
			new List<Vector2> {
				new Vector2(2, 0), 
				new Vector2(9, funkyLayout.Count),
				new Vector2(13, 0)});
	}
}

public class DiamondRoomFactory: RoomFactory {
	public override Room getRoom () {

		Terrain O = Terrain.Open;
		Terrain W = Terrain.Wall;
		Terrain B = Terrain.BossEnemy;
		Terrain C = Terrain.Coal;

		List<List<Terrain>> funkyLayout = new List<List<Terrain>>();

		funkyLayout.Add(new List<Terrain> { W, W, W, W, O, W, W, W, W });
		funkyLayout.Add(new List<Terrain> { W, W, W, O, O, O, W, W, W });
		funkyLayout.Add(new List<Terrain> { W, W, O, O, O, O, O, W, W });
		funkyLayout.Add(new List<Terrain> { W, O, O, O, C, O, O, O, W });
		funkyLayout.Add(new List<Terrain> { O, O, O, C, B, C, O, O, O });
		funkyLayout.Add(new List<Terrain> { W, O, O, O, C, O, O, O, W });
		funkyLayout.Add(new List<Terrain> { W, W, O, O, O, O, O, W, W });
		funkyLayout.Add(new List<Terrain> { W, W, W, O, O, O, W, W, W });
		funkyLayout.Add(new List<Terrain> { W, W, W, W, O, W, W, W, W });

		return new SimpleRoomWithExits(funkyLayout, 
			new List<Vector2> {
				new Vector2(4, 0), 
				new Vector2(4, funkyLayout.Count),
				new Vector2(0, 4),
				new Vector2(8, 4)});
	}
}

public class LineRoomFactory: RoomFactory {
	public override Room getRoom () {

		Terrain O = Terrain.Open;
		Terrain W = Terrain.Wall;
		Terrain E = Terrain.Enemy1;
		Terrain C = Terrain.Coal;

		List<List<Terrain>> funkyLayout = new List<List<Terrain>>();

		funkyLayout.Add(new List<Terrain> { W, W, W });
		funkyLayout.Add(new List<Terrain> { W, C, W });
		funkyLayout.Add(new List<Terrain> { W, E, W });
		funkyLayout.Add(new List<Terrain> { W, E, W });
		funkyLayout.Add(new List<Terrain> { W, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, W });
		funkyLayout.Add(new List<Terrain> { W, O, W });

		return new SimpleRoomWithExits(funkyLayout, new List<Vector2> {new Vector2(2, 1)});
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
	private Boolean ifEnemies;

	private double enemy1Probability = 0.015;
	private double enemy2Probability = 0.015;

	public override Room getRoom ()
	{
		System.Random rnd = new System.Random ();
		SimpleRoom room = RoomTools.generateEmptySimpleRoom (width, height);
		List<List<Terrain>> layout = room.Layout;

		if (this.ifEnemies) {

			//Add one bad guy in every room first

			int firstEnemyX = rnd.Next(1,(width - 1));
			int firstEnemyY = rnd.Next(1,(height - 1));
			List<Terrain> randomEnemyList = new List<Terrain> ();
			randomEnemyList.Add (Terrain.Enemy1);
			randomEnemyList.Add (Terrain.Enemy2);
			randomEnemyList.Shuffle ();
			layout [firstEnemyX] [firstEnemyY] = randomEnemyList[0];


			for (int x = 1; x < (width - 1); x++) {
				for (int y = 1; y < (height - 1); y++) {
					Double nextDie = rnd.NextDouble ();
					if (nextDie < enemy1Probability) {
						layout [y] [x] = Terrain.Enemy1;
					}
					else if (nextDie < (enemy1Probability + enemy2Probability)) {
						layout [y] [x] = Terrain.Enemy2;
					}
				}
			}

		}

		//Add Coal in every room last
		int coalX = rnd.Next(1,(width - 1));
		int coalY = rnd.Next(1,(height - 1));
		layout [coalX] [coalY] = Terrain.Coal;

		//Add exits iteratively across different walls

//		List<Direction> doorOrder = new List<Direction> { Direction.Bottom, Direction.Left, Direction.Right, Direction.Top };

//		doorOrder.Shuffle ();

//
//
//		Func<int, int, Terrain, Terrain?> exitApplier = (x, y, existingTerrain) => {
//
//		}

		//TODO: Don't create a door on an already existing door slot

//		List<List<Terrain>> layout = room.Layout;

//		System.Random rnd = new System.Random ();
//
//		int idx;

//		for (int i = 0; i < exitCount; i++) {
//
//			Direction doorLocation = doorOrder [i % doorOrder.Count];
//			switch (doorLocation) {
//			case Direction.Top:
//				idx = rnd.Next (1, layout [0].Count - 1);
//				layout [0] [idx] = Terrain.Open;
//				break;
//			case Direction.Bottom:
//				idx = rnd.Next (1, layout [layout.Count - 1].Count - 1);
//				layout [layout.Count - 1] [idx] = Terrain.Open;
//				break;
//			case Direction.Left:
//				idx = rnd.Next (1, layout.Count - 1);
//				layout [idx] [0] = Terrain.Open;
//				break;
//			case Direction.Right:
//				idx = rnd.Next (1, layout.Count - 1);
//				layout [idx] [layout.Count - 1] = Terrain.Open;
//				break;
//			}
//		}

		return room;
	}

	public RectangleRoomFactory (int width, int height, int exitCount, Boolean ifEnemies = true)
	{
		this.width = width;
		this.height = height;
		this.exitCount = exitCount;
		this.ifEnemies = ifEnemies;
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
		return this.y1 - room.getHeight() - padding;
	}

	public RoomCoordinates(Room room, int x, int y) {
		this.x1 = x;
		this.y1 = y;
		this.room = room;
	}

//	public Boolean doesCollide(int x, int y) {
//		return this.doesCollide (x, y, this.x1, this.y1);
//	}

	public Boolean doesCollide(int x1, int y1, int x2, int y2) {
		return (x1 < this.x2() && x2 > this.x1 && y1 > this.y2() && y2 < this.y1);
	}

	public Boolean doesCollide(RoomCoordinates otherRoomCoordinates) {
		return doesCollide (otherRoomCoordinates.x1, otherRoomCoordinates.y1, otherRoomCoordinates.x2(), otherRoomCoordinates.y2());
//		return (otherRoomCoordinates.x1 < this.x2() && otherRoomCoordinates.x2() > this.x1 && 
//			otherRoomCoordinates.y1 > this.y2() && otherRoomCoordinates.y2() < this.y1);
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

		while (Math.Abs(X2 - currentX) > 0 || Math.Abs(Y2 - currentY) > 0) {
			if (direction) {
				int deltaX = Math.Min (rng.Next (5), Math.Abs (X2 - currentX));

				for (int i = 0; i < deltaX; i++) {
					currentX += xSign;
					yield return new TerrainCoordinates (currentX, currentY, Terrain.Open);
				}
				direction = !direction;
//				}
			} else {
				int deltaY = Math.Min(rng.Next (5), Math.Abs(Y2 - currentY));
				for (int i = 0; i < deltaY; i++) {
					currentY += ySign;
					yield return new TerrainCoordinates (currentX, currentY, Terrain.Open);
				}
				direction = !direction;
//				}
			}
		}

	}
}

public static class TerrainGenerator
{

//	void Start() {
//		Room room = GenerateLevel (100, 100, 0);
//		List<List<Terrain>> layout = room.getLayout ();
//
////		Debug.Log (layout.ToString ());
////
////		Debug.Log ("potato");
//
//		foreach (List<Terrain> row in layout) {
//			List<String> strRow = new List<String> ();
//			foreach (Terrain tr in row) {
//				if (tr == Terrain.Open)
//					strRow.Add ("O");
//				else if (tr == Terrain.Wall)
//					strRow.Add ("W");
//				else
//					strRow.Add (tr.ToString ());
//			}
//			String[] strArray = strRow.ToArray ();
//			print (String.Join (",", strArray));
//		}
//
//		print ("potato");
//	}

	private static int roomPlacementAttempts = 250;
	private static int roomPositioningRetries = 10;
	private static int maxRooms = 28;
	private static int levelBorder = 2;

	//For now needs to add up to 1
	private static List<KeyValuePair<double, RoomFactory>> getRoomProbabilities(int level) {
		if (level == 0) {
			return new List<KeyValuePair<double, RoomFactory>>()
			{
				new KeyValuePair<double, RoomFactory>( 0.3, new RectangleRoomFactory(4,4,2)),
				new KeyValuePair<double, RoomFactory>( 0.3, new RectangleRoomFactory(6,6,2)),
				new KeyValuePair<double, RoomFactory>( 0.3, new RectangleRoomFactory(9,9,2)),
				new KeyValuePair<double, RoomFactory>( 0.05, new RectangleRoomFactory(12,12,3)),
				//new KeyValuePair<double, RoomFactory>( 0.2, new DiamondRoomFactory()),
				new KeyValuePair<double, RoomFactory>( 0.05, new FunkyRoomFactory()) //,
				//new KeyValuePair<double, RoomFactory>( 0.2, new LineRoomFactory())
			};
		} else
			throw new NotImplementedException ();
	}

	public static Room GenerateLevel(int width, int height, int level) {

//		print ("beginning");

		//Cheat and use room generator and then keep layout
		SimpleRoom baseRoom = RoomTools.generateWallRoom (width, height);

		List<KeyValuePair<double, RoomFactory>> roomProbabilities = getRoomProbabilities (level);

		System.Random r = new System.Random();

		//TODO: Merge room and roomCoordinates
		List<RoomCoordinates> placedRoomCoordinates = new List<RoomCoordinates> ();

		//Add Starting and Ending rooms

		RoomFactory startingRoomFactory = new RectangleRoomFactory (6, 6, 1, false);

		placedRoomCoordinates.Add(new RoomCoordinates(startingRoomFactory.getRoom(), width / 2 - 3, height - 10));
		placedRoomCoordinates.Add(new RoomCoordinates(startingRoomFactory.getRoom(), width / 2 - 3, 2));

		DiamondRoomFactory bossFactory = new DiamondRoomFactory ();

		//Add 2 Boss Rooms

		List<RoomFactory> requiredRoomFactories = new List<RoomFactory> ();
		requiredRoomFactories.Add (new ComplexRoomFactory ());
		requiredRoomFactories.Add(bossFactory);
		requiredRoomFactories.Add(bossFactory);


		for (int i = 0; i < roomPlacementAttempts; i++) {
			if (placedRoomCoordinates.Count >= maxRooms)
				break;

			double diceRoll = r.NextDouble();

			double cumulative = 0.0;
			RoomFactory selectedRoomFactory = new RectangleRoomFactory(4,4,4);  //Default room in case RoomProbabilities are borked

			if (requiredRoomFactories.Count > 0) {
				selectedRoomFactory = requiredRoomFactories [0];
				requiredRoomFactories.RemoveAt(0);
			} else {
				foreach (KeyValuePair<double, RoomFactory> entry in roomProbabilities) {
					cumulative += entry.Key;
					if (diceRoll < cumulative)
					{
						selectedRoomFactory = entry.Value;
						break;
					}
				} 
			}

			//Try to place each room N times
			Room roomToPlace = selectedRoomFactory.getRoom();

			int xMax = width - roomToPlace.getWidth () - levelBorder;
			int yMax = height - roomToPlace.getHeight () - levelBorder;

			for (int j = 0; j < roomPositioningRetries; j++) {
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
					break;
				}
			}

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

		placedRoomCoordinates.Sort((first, second) => {
			return (-1 * first.y1).CompareTo(-1 * second.y1);
		}
			);

		//First add a sequential list of hallways from room to room so as to guarantee connections
		for (int i = 0; i < (placedRoomCoordinates.Count - 1); i++) {
			List<Vector2> exits1 = placedRoomCoordinates [i].getExits ();
			List<Vector2> exits2 = placedRoomCoordinates [i + 1].getExits ();


			//Calculate all distances between everything in exits1 and everything in exits2 and use closest exits for linking

			float minDistance = int.MaxValue;
			Vector2 exit1Candidate = exits1[0];
			Vector2 exit2Candidate = exits2[0];
			foreach (Vector2 ex1 in exits1) {
				foreach (Vector2 ex2 in exits2) {
					float dist = Vector2.Distance(ex1,ex2);
					if (dist < minDistance) {
						minDistance = dist;
						exit1Candidate = ex1;
						exit2Candidate = ex2;
					}
				}
			}

			hallways.Add (new Hallway(exit1Candidate, exit2Candidate));

			//Add some skip hallways if they make sense
			if (i < (placedRoomCoordinates.Count - 2)) {
				List<Vector2> exits3 = placedRoomCoordinates [i + 2].getExits ();

				float altMinDistance = int.MaxValue;
				Vector2 altExit1Candidate = exits1[0];
				Vector2 exit3Candidate = exits3[0];
				foreach (Vector2 ex1 in exits1) {
					foreach (Vector2 ex3 in exits3) {
						float dist = Vector2.Distance(ex1,ex3);
						if (dist < altMinDistance) {
							altMinDistance = dist;
							exit1Candidate = ex1;
							exit3Candidate = ex3;
						}
					}
				}
				if (altMinDistance <= 1.5 * minDistance) {
					hallways.Add (new Hallway(exit1Candidate, exit3Candidate));
				}
			}

				

//			exits1.Shuffle ();
//			exits2.Shuffle ();


		}


		//Add a few more random hallways

//		placedRoomCoordinates.Shuffle ();
//
//		for (int i = 0; i < (1 + (placedRoomCoordinates.Count - 1) / 3); i++) {  //(1 + (placedRoomCoordinates.Count - 1) / 3)
//			
//			List<Vector2> exits1 = placedRoomCoordinates [i % placedRoomCoordinates.Count].getExits ();
//			List<Vector2> exits2 = placedRoomCoordinates [(i + 1) % placedRoomCoordinates.Count].getExits ();
//
////			print ("exits");
////			exits1.Shuffle ();
////			exits2.Shuffle ();
////			print (exits1[0].x);
////			print (exits1[0].y);
////			print (exits2[0].x);
////			print (exits2[0].y);
//
//			hallways.Add (new Hallway(exits1[0], exits2[0]));
//		}

		foreach (RoomCoordinates rc in placedRoomCoordinates) {
			baseRoom = RoomTools.mergeRoom (baseRoom, rc);
		}
			
		foreach (Hallway hw in hallways) {
			baseRoom = RoomTools.mergeHallway (baseRoom, hw);
		}

		return baseRoom;
	}

}