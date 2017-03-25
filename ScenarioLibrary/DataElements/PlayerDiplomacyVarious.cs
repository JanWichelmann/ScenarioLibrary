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
	/// Contains various player data.
	/// </summary>
	public class PlayerDiplomacyVarious
	{
		#region Fields

		/// <summary>
		/// UnknownPlayerCount (always 9).
		/// </summary>
		public uint UnknownPlayerCount { get; set; }

		/// <summary>
		/// The player data entries. Length: 8 entries.
		/// </summary>
		public List<PlayerDiplomacyVariousEntry> PlayerDiplomacyVariousEntries { get; set; }

		/// <summary>
		/// Unknown.
		/// </summary>
		public ulong Unknown { get; set; }

		#endregion

		#region Functions

		/// <summary>
		/// Reads the data element from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
		public PlayerDiplomacyVarious ReadData(RAMBuffer buffer)
		{
			UnknownPlayerCount = buffer.ReadUInteger();

			PlayerDiplomacyVariousEntries = new List<PlayerDiplomacyVariousEntry>(8);
			for(int i = 0; i < 8; i++)
				PlayerDiplomacyVariousEntries.Add(new PlayerDiplomacyVariousEntry().ReadData(buffer));

			Unknown = buffer.ReadULong();

			return this;
		}

		/// <summary>
		/// Writes the data element into the given buffer (at its current position).
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
		public void WriteData(RAMBuffer buffer)
		{
			buffer.WriteUInteger(UnknownPlayerCount);

			ScenarioDataElementTools.AssertListLength(PlayerDiplomacyVariousEntries, 8);
			PlayerDiplomacyVariousEntries.ForEach(e => e.WriteData(buffer));

			buffer.WriteULong(Unknown);
		}

		#endregion

		#region Sub types

		/// <summary>
		/// Contains various information for one player.
		/// </summary>
		public class PlayerDiplomacyVariousEntry
		{
			#region Fields

			/// <summary>
			/// The player name.
			/// </summary>
			public string PlayerName { get; set; }

			/// <summary>
			/// The player's initial camera X.
			/// </summary>
			public float InitialCameraX { get; set; }

			/// <summary>
			/// The player's initial camera Y.
			/// </summary>
			public float InitialCameraY { get; set; }

			/// <summary>
			/// Unknown.
			/// </summary>
			public short UnknownX { get; set; }

			/// <summary>
			/// Unknown.
			/// </summary>
			public short UnknownY { get; set; }

			/// <summary>
			/// The "allied victory" setting.
			/// </summary>
			public byte AlliedVictory { get; set; }

			/// <summary>
			/// Diplomacy settings. Length: Should always match the real player count.
			/// </summary>
			public List<DiplomacyTypes1> Diplomacy1 { get; set; }

			/// <summary>
			/// Diplomacy settings. Length: Should always match the real player count.
			/// </summary>
			public List<DiplomacyTypes2> Diplomacy2 { get; set; }

			/// <summary>
			/// Player color index.
			/// </summary>
			public uint Color { get; set; }

			/// <summary>
			/// Unknown.
			/// </summary>
			public float Unknown1;

			/// <summary>
			/// Unknown.
			/// </summary>
			public ushort Unknown3Count;

			/// <summary>
			/// Unknown. Only used if Unknown1 == 2.0f.
			/// </summary>
			public List<byte> Unknown2 { get; set; }

			/// <summary>
			/// Unknown. Length: count * 44 bytes.
			/// </summary>
			public List<byte> Unknown3 { get; set; }

			/// <summary>
			/// Unknown. Usually 0. Length: 7 entries.
			/// </summary>
			public List<byte> Unknown4 { get; set; }

			/// <summary>
			/// Unknown.
			/// </summary>
			public int Unknown5 { get; set; }

			#endregion

			#region Functions

			/// <summary>
			/// Reads the data element from the given buffer.
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
			public PlayerDiplomacyVariousEntry ReadData(RAMBuffer buffer)
			{
				PlayerName = buffer.ReadString(buffer.ReadUShort());
				InitialCameraX = buffer.ReadFloat();
				InitialCameraY = buffer.ReadFloat();
				UnknownX = buffer.ReadShort();
				UnknownY = buffer.ReadShort();
				AlliedVictory = buffer.ReadByte();

				ushort playerDiplomacyCount = buffer.ReadUShort();
				Diplomacy1 = new List<DiplomacyTypes1>(playerDiplomacyCount);
				for(int i = 0; i < playerDiplomacyCount; i++)
					Diplomacy1.Add((DiplomacyTypes1)buffer.ReadByte());
				Diplomacy2 = new List<DiplomacyTypes2>(playerDiplomacyCount);
				for(int i = 0; i < playerDiplomacyCount; i++)
					Diplomacy2.Add((DiplomacyTypes2)buffer.ReadUInteger());

				Color = buffer.ReadUInteger();
				Unknown1 = buffer.ReadFloat();
				Unknown3Count = buffer.ReadUShort();

				if(Unknown1 == 2)
				{
					Unknown2 = new List<byte>(8);
					for(int i = 0; i < 8; i++)
						Unknown2.Add(buffer.ReadByte());
				}

				Unknown3 = new List<byte>(Unknown3Count * 44);
				for(int i = 0; i < Unknown3Count * 44; i++)
					Unknown3.Add(buffer.ReadByte());

				Unknown4 = new List<byte>(7);
				for(int i = 0; i < 7; i++)
					Unknown4.Add(buffer.ReadByte());

				Unknown5 = buffer.ReadInteger();

				return this;
			}

			/// <summary>
			/// Writes the data element into the given buffer (at its current position).
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
			public void WriteData(RAMBuffer buffer)
			{
				buffer.WriteUShort((ushort)PlayerName.Length);
				buffer.WriteString(PlayerName);
				buffer.WriteFloat(InitialCameraX);
				buffer.WriteFloat(InitialCameraY);
				buffer.WriteShort(UnknownX);
				buffer.WriteShort(UnknownY);
				buffer.WriteByte(AlliedVictory);

				ScenarioDataElementTools.AssertTrue(Diplomacy1.Count == Diplomacy2.Count);
				buffer.WriteUShort((ushort)Diplomacy1.Count);
				Diplomacy1.ForEach(d => buffer.WriteByte((byte)d));
				Diplomacy2.ForEach(d => buffer.WriteUInteger((uint)d));

				buffer.WriteUInteger(Color);
				buffer.WriteFloat(Unknown1);
				buffer.WriteUShort(Unknown3Count);

				if(Unknown1==2)
				{
					ScenarioDataElementTools.AssertListLength(Unknown2, 8);
					Unknown2.ForEach(b => buffer.WriteByte(b));
				}

				ScenarioDataElementTools.AssertListLength(Unknown3, Unknown3Count * 44);
				Unknown3.ForEach(b => buffer.WriteByte(b));

				ScenarioDataElementTools.AssertListLength(Unknown4, 7);
				Unknown4.ForEach(b => buffer.WriteByte(b));

				buffer.WriteInteger(Unknown5);
			}

			#endregion
		}

		public enum DiplomacyTypes1 : byte
		{
			Allied = 0,
			Neutral = 1,
			Enemy = 3
		}

		public enum DiplomacyTypes2 : uint
		{
			Gaia = 0,
			Self = 1,
			Allied = 2,
			Neutral = 3,
			Enemy = 4
		}

		#endregion
	}
}