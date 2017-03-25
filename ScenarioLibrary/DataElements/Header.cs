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
	/// Represents the (compressed) scenario header.
	/// </summary>
	public class Header
	{
		#region Fields

		/// <summary>
		/// The instance ID of the next placed unit.
		/// </summary>
		public uint NextUnitIdToPlace { get; set; }

		/// <summary>
		/// The player names. Length: 16 players, each name has at most 256 characters.
		/// </summary>
		public List<string> PlayerNames { get; set; }

		/// <summary>
		/// The DLL IDs of the player names (optional). Length: 16 entries.
		/// </summary>
		public List<uint> PlayerNameDllIds { get; set; }

		/// <summary>
		/// Some basic information about the players. Length: 16 entries.
		/// </summary>
		public List<PlayerDataHeader> PlayerData { get; set; }

		/// <summary>
		/// Unknown, usually 1.
		/// </summary>
		public uint Unknown1 { get; set; }

		/// <summary>
		/// Unknown, always -1.
		/// </summary>
		public float Unknown2 { get; set; }

		/// <summary>
		/// The file name the scenario was originally saved to.
		/// </summary>
		public string OriginalFileName { get; set; }

		#endregion

		#region Functions

		/// <summary>
		/// Reads the data element from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
		public Header ReadData(RAMBuffer buffer)
		{
			NextUnitIdToPlace = buffer.ReadUInteger();

			float version = buffer.ReadFloat();
			if(version != 1.22f)
				throw new InvalidDataException($"Invalid secondary file version: '{version}'");

			PlayerNames = new List<string>(16);
			for(int i = 0; i < 16; ++i)
				PlayerNames.Add(buffer.ReadString(256));

			PlayerNameDllIds = new List<uint>(16);
			for(int i = 0; i < 16; ++i)
				PlayerNameDllIds.Add(buffer.ReadUInteger());

			PlayerData = new List<PlayerDataHeader>(16);
			for(int i = 0; i < 16; ++i)
				PlayerData.Add(new PlayerDataHeader().ReadData(buffer));

			Unknown1 = buffer.ReadUInteger();
			byte unknown = buffer.ReadByte(); // = 0
			Unknown2 = buffer.ReadFloat();

			short length = buffer.ReadShort();
			OriginalFileName = buffer.ReadString(length);

			return this;
		}

		/// <summary>
		/// Writes the data element into the given buffer (at its current position).
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
		public void WriteData(RAMBuffer buffer)
		{
			buffer.WriteUInteger(NextUnitIdToPlace);

			buffer.WriteFloat(1.22f);

			ScenarioDataElementTools.AssertListLength(PlayerNames, 16);
			PlayerNames.ForEach(p => buffer.WriteString(p, 256));

			ScenarioDataElementTools.AssertListLength(PlayerNameDllIds, 16);
			PlayerNameDllIds.ForEach(p => buffer.WriteUInteger(p));

			ScenarioDataElementTools.AssertListLength(PlayerData, 16);
			PlayerData.ForEach(p => p.WriteData(buffer));

			buffer.WriteUInteger(Unknown1);
			buffer.WriteByte(0);
			buffer.WriteFloat(Unknown2);

			buffer.WriteShort((short)OriginalFileName.Length);
			buffer.WriteString(OriginalFileName);
		}

		#endregion

		#region Sub types

		/// <summary>
		/// Represents one player data entry as stored in the header section.
		/// </summary>
		public class PlayerDataHeader
		{
			#region Fields

			/// <summary>
			/// Tells whether the player is enabled.
			/// </summary>
			public uint Active { get; set; }

			/// <summary>
			/// Tells whether the player is human.
			/// </summary>
			public uint Human { get; set; }

			/// <summary>
			/// The player civilization ID.
			/// </summary>
			public uint CivId { get; set; }

			/// <summary>
			/// Unknown, usually 4.
			/// </summary>
			public uint Unknown { get; set; }

			#endregion

			#region Functions

			/// <summary>
			/// Reads the data element from the given buffer.
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
			public PlayerDataHeader ReadData(RAMBuffer buffer)
			{
				Active = buffer.ReadUInteger();
				Human = buffer.ReadUInteger();
				CivId = buffer.ReadUInteger();
				Unknown = buffer.ReadUInteger();

				return this;
			}

			/// <summary>
			/// Writes the data element into the given buffer (at its current position).
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
			public void WriteData(RAMBuffer buffer)
			{
				buffer.WriteUInteger(Active);
				buffer.WriteUInteger(Human);
				buffer.WriteUInteger(CivId);
				buffer.WriteUInteger(Unknown);
			}

			#endregion
		}

		#endregion
	}
}