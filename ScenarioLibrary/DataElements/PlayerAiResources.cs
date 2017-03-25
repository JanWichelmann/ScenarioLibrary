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
	/// 
	/// </summary>
	public class PlayerAiResources
	{
		#region Fields

		/// <summary>
		/// Unknown strings, two per player (?). Length: 32 entries.
		/// </summary>
		public List<string> UnknownStrings { get; set; }

		/// <summary>
		/// Player AI names. Length: 16 entries.
		/// </summary>
		public List<string> AiNames { get; set; }

		/// <summary>
		/// Player AI main *.per file contents. Length: 16 entries.
		/// </summary>
		public List<AiFile> AiFiles { get; set; }

		/// <summary>
		/// Player AI types. Custom = 0, Standard = 1, None = 2. Length: 16 entries.
		/// </summary>
		public List<byte> AiTypes { get; set; }

		/// <summary>
		/// Player starting resources. Length: 16 entries.
		/// </summary>
		public List<ResourceEntry> ResourceEntries { get; set; }

		// Resources

		#endregion

		#region Functions

		/// <summary>
		/// Reads the data element from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
		public PlayerAiResources ReadData(RAMBuffer buffer)
		{
			UnknownStrings = new List<string>(32);
			for(int i = 0; i < 32; ++i)
				UnknownStrings.Add(buffer.ReadString(buffer.ReadShort()));

			AiNames = new List<string>(16);
			for(int i = 0; i < 16; ++i)
				AiNames.Add(buffer.ReadString(buffer.ReadShort()));

			AiFiles = new List<AiFile>(16);
			for(int i = 0; i < 16; ++i)
				AiFiles.Add(new AiFile().ReadData(buffer));

			AiTypes = new List<byte>(16);
			for(int i = 0; i < 16; ++i)
				AiTypes.Add(buffer.ReadByte());

			// Separator
			ScenarioDataElementTools.AssertTrue(buffer.ReadUInteger() == 0xFFFFFF9D);

			ResourceEntries = new List<ResourceEntry>(16);
			for(int i = 0; i < 16; ++i)
				ResourceEntries.Add(new ResourceEntry().ReadData(buffer));

			return this;
		}

		/// <summary>
		/// Writes the data element into the given buffer (at its current position).
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
		public void WriteData(RAMBuffer buffer)
		{
			ScenarioDataElementTools.AssertListLength(UnknownStrings, 32);
			UnknownStrings.ForEach(s => { buffer.WriteShort((short)s.Length); buffer.WriteString(s); });

			ScenarioDataElementTools.AssertListLength(AiNames, 16);
			AiNames.ForEach(s => { buffer.WriteShort((short)s.Length); buffer.WriteString(s); });

			ScenarioDataElementTools.AssertListLength(AiFiles, 16);
			AiFiles.ForEach(f => f.WriteData(buffer));

			ScenarioDataElementTools.AssertListLength(AiTypes, 16);
			AiTypes.ForEach(t => buffer.WriteByte(t));

			buffer.WriteUInteger(0xFFFFFF9D);

			ScenarioDataElementTools.AssertListLength(ResourceEntries, 16);
			ResourceEntries.ForEach(e => e.WriteData(buffer));
		}

		#endregion

		#region Sub types

		/// <summary>
		/// Represents one player AI file entry.
		/// </summary>
		public class AiFile
		{
			#region Fields

			/// <summary>
			/// Unknown.
			/// </summary>
			public uint Unknown1 { get; set; }

			/// <summary>
			/// Unknown.
			/// </summary>
			public uint Unknown2 { get; set; }

			/// <summary>
			/// The content of the AI's main *.per file.
			/// </summary>
			public string AiPerFileContent { get; set; }

			#endregion

			#region Functions

			/// <summary>
			/// Reads the data element from the given buffer.
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
			public AiFile ReadData(RAMBuffer buffer)
			{
				Unknown1 = buffer.ReadUInteger();
				Unknown2 = buffer.ReadUInteger();
				AiPerFileContent = buffer.ReadString(buffer.ReadInteger());

				return this;
			}

			/// <summary>
			/// Writes the data element into the given buffer (at its current position).
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
			public void WriteData(RAMBuffer buffer)
			{
				buffer.WriteUInteger(Unknown1);
				buffer.WriteUInteger(Unknown2);
				buffer.WriteInteger(AiPerFileContent.Length);
				buffer.WriteString(AiPerFileContent);
			}

			#endregion
		}

		/// <summary>
		/// Represents one player atarting resource entry.
		/// </summary>
		public class ResourceEntry
		{
			#region Fields

			/// <summary>
			/// Gold amount.
			/// </summary>
			public uint Gold { get; set; }

			/// <summary>
			/// Wood amount.
			/// </summary>
			public uint Wood { get; set; }

			/// <summary>
			/// Food amount.
			/// </summary>
			public uint Food { get; set; }

			/// <summary>
			/// Stone amount.
			/// </summary>
			public uint Stone { get; set; }

			/// <summary>
			/// "Ore" amount (unused by default).
			/// </summary>
			public uint Ore { get; set; }

			/// <summary>
			/// Padding, always 0.
			/// </summary>
			public uint Padding { get; set; }

			#endregion

			#region Functions

			/// <summary>
			/// Reads the data element from the given buffer.
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
			public ResourceEntry ReadData(RAMBuffer buffer)
			{
				Gold = buffer.ReadUInteger();
				Wood = buffer.ReadUInteger();
				Food = buffer.ReadUInteger();
				Stone = buffer.ReadUInteger();
				Ore = buffer.ReadUInteger();
				Padding = buffer.ReadUInteger();

				return this;
			}

			/// <summary>
			/// Writes the data element into the given buffer (at its current position).
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
			public void WriteData(RAMBuffer buffer)
			{
				buffer.WriteUInteger(Gold);
				buffer.WriteUInteger(Wood);
				buffer.WriteUInteger(Food);
				buffer.WriteUInteger(Stone);
				buffer.WriteUInteger(Ore);
				buffer.WriteUInteger(Padding);
			}

			#endregion
		}

		#endregion
	}
}