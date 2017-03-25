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
	/// Contains instructions, video file names and the scenario bitmap.
	/// </summary>
	public class IncludedFiles
	{
		#region Fields

		/// <summary>
		/// ES-only data included?
		/// </summary>
		public uint EsOnlyDataIncluded { get; set; }

		/// <summary>
		/// ES-only data. Length: 396, if flag is set.
		/// </summary>
		public List<byte> EsOnlyData { get; set; }

		/// <summary>
		/// The included *.per files.
		/// </summary>
		public List<IncludedFile> Files { get; set; }

		#endregion

		#region Functions

		/// <summary>
		/// Reads the data element from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
		public IncludedFiles ReadData(RAMBuffer buffer)
		{
			int filesIncluded = buffer.ReadInteger();

			EsOnlyDataIncluded = buffer.ReadUInteger();
			if(EsOnlyDataIncluded > 0)
			{
				EsOnlyData = new List<byte>(396);
				for(int i = 0; i < 396; ++i)
					EsOnlyData.Add(buffer.ReadByte());
			}

			if(filesIncluded > 0)
			{
				int fileCount = buffer.ReadInteger();
				Files = new List<IncludedFile>(fileCount);
				for(int i = 0; i < fileCount; i++)
					Files.Add(new IncludedFile().ReadData(buffer));
			}

			return this;
		}

		/// <summary>
		/// Writes the data element into the given buffer (at its current position).
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
		public void WriteData(RAMBuffer buffer)
		{
			buffer.WriteInteger(Files.Count > 0 ? 1 : 0);

			buffer.WriteUInteger(EsOnlyDataIncluded);
			if(EsOnlyDataIncluded > 0)
			{
				ScenarioDataElementTools.AssertListLength(EsOnlyData, 396);
				EsOnlyData.ForEach(b => buffer.WriteByte(b));
			}

			if(Files.Count > 0)
			{
				buffer.WriteInteger(Files.Count);
				Files.ForEach(f => f.WriteData(buffer));
			}
		}

		#endregion

		#region Sub types

		/// <summary>
		/// Contains the data of one included file.
		/// </summary>
		public class IncludedFile
		{
			#region Fields

			public string Title { get; set; }
			public string Content { get; set; }

			#endregion

			#region Functions

			/// <summary>
			/// Reads the data element from the given buffer.
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
			public IncludedFile ReadData(RAMBuffer buffer)
			{
				Title = buffer.ReadString(buffer.ReadInteger());
				Content = buffer.ReadString(buffer.ReadInteger());

				return this;
			}

			/// <summary>
			/// Writes the data element into the given buffer (at its current position).
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
			public void WriteData(RAMBuffer buffer)
			{
				buffer.WriteInteger(Title.Length);
				buffer.WriteString(Title);
				buffer.WriteInteger(Content.Length);
				buffer.WriteString(Content);
			}

			#endregion
		}

		#endregion
	}
}