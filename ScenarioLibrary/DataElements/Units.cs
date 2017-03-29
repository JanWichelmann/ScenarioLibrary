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
	/// Contains the units placed on the map.
	/// </summary>
	public class Units
	{
		#region Fields

		/// <summary>
		/// The player resources (duplicate) and population limits. Length: 8 entries.
		/// </summary>
		public List<ResourcePopulationEntry> PlayerResourcesPopulationLimits { get; set; }

		/// <summary>
		/// The unit sections (normally 9, one per player).
		/// </summary>
		public List<PlayerUnitsEntry> UnitSections { get; set; }

		#endregion

		#region Functions

		/// <summary>
		/// Reads the data element from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
		public Units ReadData(RAMBuffer buffer)
		{
			int unitSectionCount = buffer.ReadInteger();

			PlayerResourcesPopulationLimits = new List<ResourcePopulationEntry>(8);
			for(int i = 0; i < 8; i++)
				PlayerResourcesPopulationLimits.Add(new ResourcePopulationEntry().ReadData(buffer));

			UnitSections = new List<PlayerUnitsEntry>(unitSectionCount);
			for(int i = 0; i < unitSectionCount; i++)
				UnitSections.Add(new PlayerUnitsEntry().ReadData(buffer));

			return this;
		}

		/// <summary>
		/// Writes the data element into the given buffer (at its current position).
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
		public void WriteData(RAMBuffer buffer)
		{
			buffer.WriteInteger(UnitSections.Count);

			ScenarioDataElementTools.AssertListLength(PlayerResourcesPopulationLimits, 8);
			PlayerResourcesPopulationLimits.ForEach(e => e.WriteData(buffer));

			UnitSections.ForEach(s => s.WriteData(buffer));
		}

		#endregion

		#region Sub types

		/// <summary>
		/// Represents one player starting resource/population limit entry.
		/// </summary>
		public class ResourcePopulationEntry
		{
			#region Fields

			/// <summary>
			/// Gold amount.
			/// </summary>
			public float Gold { get; set; }

			/// <summary>
			/// Wood amount.
			/// </summary>
			public float Wood { get; set; }

			/// <summary>
			/// Food amount.
			/// </summary>
			public float Food { get; set; }

			/// <summary>
			/// Stone amount.
			/// </summary>
			public float Stone { get; set; }

			/// <summary>
			/// "Ore" amount (unused by default).
			/// </summary>
			public float Ore { get; set; }

			/// <summary>
			/// Padding, always 0.
			/// </summary>
			public uint Padding { get; set; }

			/// <summary>
			/// The player's population limit.
			/// </summary>
			public float PopulationLimit { get; set; }

			#endregion

			#region Functions

			/// <summary>
			/// Reads the data element from the given buffer.
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
			public ResourcePopulationEntry ReadData(RAMBuffer buffer)
			{
				Gold = buffer.ReadFloat();
				Wood = buffer.ReadFloat();
				Food = buffer.ReadFloat();
				Stone = buffer.ReadFloat();
				Ore = buffer.ReadFloat();
				Padding = buffer.ReadUInteger();
				PopulationLimit = buffer.ReadFloat();

				return this;
			}

			/// <summary>
			/// Writes the data element into the given buffer (at its current position).
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
			public void WriteData(RAMBuffer buffer)
			{
				buffer.WriteFloat(Gold);
				buffer.WriteFloat(Wood);
				buffer.WriteFloat(Food);
				buffer.WriteFloat(Stone);
				buffer.WriteFloat(Ore);
				buffer.WriteUInteger(Padding);
				buffer.WriteFloat(PopulationLimit);
			}

			#endregion
		}

		/// <summary>
		/// Contains the units of one player.
		/// </summary>
		public class PlayerUnitsEntry
		{
			#region Fields

			/// <summary>
			/// The units.
			/// </summary>
			public List<UnitEntry> Units { get; set; }

			#endregion

			#region Functions

			/// <summary>
			/// Reads the data element from the given buffer.
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
			public PlayerUnitsEntry ReadData(RAMBuffer buffer)
			{
				int unitCount = buffer.ReadInteger();
				Units = new List<UnitEntry>(unitCount);
				for(int i = 0; i < unitCount; ++i)
					Units.Add(new UnitEntry().ReadData(buffer));

				return this;
			}

			/// <summary>
			/// Writes the data element into the given buffer (at its current position).
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
			public void WriteData(RAMBuffer buffer)
			{
				buffer.WriteInteger(Units.Count);
				Units.ForEach(u => u.WriteData(buffer));
			}

			#endregion
		}

		/// <summary>
		/// Entry for one unit.
		/// </summary>
		public class UnitEntry
		{
			#region Fields

			/// <summary>
			/// The unit X coordinate.
			/// </summary>
			public float PositionX { get; set; }

			/// <summary>
			/// The unit Y coordinate.
			/// </summary>
			public float PositionY { get; set; }

			/// <summary>
			/// The unit Z coordinate.
			/// </summary>
			public float PositionZ { get; set; }

			/// <summary>
			/// The unit instance ID.
			/// </summary>
			public uint Id { get; set; }

			/// <summary>
			/// The unit ID.
			/// </summary>
			public ushort UnitId { get; set; }

			/// <summary>
			/// Unit state. Always 2.
			/// </summary>
			public byte State { get; set; }

			/// <summary>
			/// The initial rotation in radians.
			/// </summary>
			public float Rotation { get; set; }

			/// <summary>
			/// The initial animation frame ID.
			/// </summary>
			public ushort Frame { get; set; }

			/// <summary>
			/// The instance ID of the unit this unit is garrisoned in (else -1).
			/// </summary>
			public int GarrisonId { get; set; }

			#endregion

			#region Functions

			/// <summary>
			/// Reads the data element from the given buffer.
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
			public UnitEntry ReadData(RAMBuffer buffer)
			{
				PositionX = buffer.ReadFloat();
				PositionY = buffer.ReadFloat();
				PositionZ = buffer.ReadFloat();
				Id = buffer.ReadUInteger();
				UnitId = buffer.ReadUShort();
				State = buffer.ReadByte();
				Rotation = buffer.ReadFloat();
				Frame = buffer.ReadUShort();
				GarrisonId = buffer.ReadInteger();

				return this;
			}

			/// <summary>
			/// Writes the data element into the given buffer (at its current position).
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
			public void WriteData(RAMBuffer buffer)
			{
				buffer.WriteFloat(PositionX);
				buffer.WriteFloat(PositionY);
				buffer.WriteFloat(PositionZ);
				buffer.WriteUInteger(Id);
				buffer.WriteUShort(UnitId);
				buffer.WriteByte(State);
				buffer.WriteFloat(Rotation);
				buffer.WriteUShort(Frame);
				buffer.WriteInteger(GarrisonId);
			}

			#endregion
		}

		#endregion
	}
}