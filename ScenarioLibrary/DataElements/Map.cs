using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IORAMHelper;

namespace ScenarioLibrary.DataElements
{
	/// <summary>
	/// Contains the map data.
	/// 
	/// Coordinate System: (0,0) in upper left corner, becomes left corner in diamond map.
	/// Here the X axis is considered vertical, the Y axis horizontal (makes sense in diamond mode).
	/// </summary>
	public class Map
	{
		#region Fields

		/// <summary>
		/// The Y coordinate of the player 1 starting camera.
		/// </summary>
		public int Player1CameraY { get; set; }

		/// <summary>
		/// The X coordinate of the player 1 starting camera.
		/// </summary>
		public int Player1CameraX { get; set; }

		/// <summary>
		/// The map type for the AI (one of the #load-if-defined constants).
		/// </summary>
		public int AiMapCode { get; set; }

		/// <summary>
		/// The map width (max. 256).
		/// </summary>
		public uint MapWidth { get; set; }

		/// <summary>
		/// The map height (max. 256).
		/// </summary>
		public uint MapHeight { get; set; }

		/// <summary>
		/// The terrain data for all map tiles. Length: MapWidth * MapHeight.
		/// </summary>
		public List<MapTileTerrainData> Tiles { get; set; }

		#endregion

		#region Functions

		/// <summary>
		/// Reads the data element from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
		public Map ReadData(RAMBuffer buffer)
		{
			// Separator
			ScenarioDataElementTools.AssertTrue(buffer.ReadUInteger() == 0xFFFFFF9D);

			Player1CameraY = buffer.ReadInteger();
			Player1CameraX = buffer.ReadInteger();
			AiMapCode = buffer.ReadInteger();
			MapWidth = buffer.ReadUInteger();
			MapHeight = buffer.ReadUInteger();

			Tiles = new List<MapTileTerrainData>((int)MapWidth * (int)MapHeight);
			for(int i = 0; i < MapWidth * MapHeight; ++i)
				Tiles.Add(new MapTileTerrainData().ReadData(buffer));

			return this;
		}

		/// <summary>
		/// Writes the data element into the given buffer (at its current position).
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
		public void WriteData(RAMBuffer buffer)
		{
			buffer.WriteUInteger(0xFFFFFF9D);

			buffer.WriteInteger(Player1CameraY);
			buffer.WriteInteger(Player1CameraX);
			buffer.WriteInteger(AiMapCode);
			buffer.WriteUInteger(MapWidth);
			buffer.WriteUInteger(MapHeight);

			ScenarioDataElementTools.AssertListLength(Tiles, (int)MapWidth * (int)MapHeight);
			Tiles.ForEach(t => t.WriteData(buffer));
		}

		#endregion

		#region Sub types

		/// <summary>
		/// Contains the terrain information of a map tile.
		/// </summary>
		public class MapTileTerrainData
		{
			#region Fields

			public byte TerrainId { get; set; }
			public byte Elevation { get; set; }
			public byte Unused { get; set; }

			#endregion

			#region Functions

			/// <summary>
			/// Reads the data element from the given buffer.
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
			public MapTileTerrainData ReadData(RAMBuffer buffer)
			{
				TerrainId = buffer.ReadByte();
				Elevation = buffer.ReadByte();
				Unused = buffer.ReadByte();

				return this;
			}

			/// <summary>
			/// Writes the data element into the given buffer (at its current position).
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
			public void WriteData(RAMBuffer buffer)
			{
				buffer.WriteByte(TerrainId);
				buffer.WriteByte(Elevation);
				buffer.WriteByte(Unused);
			}

			#endregion
		}

		#endregion
	}
}