using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IORAMHelper;

namespace ScenarioLibrary
{
	/// <summary>
	/// Represents a scenario file (SCX format).
	/// </summary>
	public class Scenario
	{
		#region Constants

		#endregion

		#region Fields

		/// <summary>
		/// The scenario file name and path.
		/// </summary>
		/// <remarks></remarks>
		public string _filename = "";

		/// <summary>
		/// The buffer used for I/O operations.
		/// </summary>
		/// <remarks></remarks>
		private RAMBuffer _buffer = null;

		#region Data elements

		/// <summary>
		/// The time when the scenario was modified last.
		/// </summary>
		public uint LastSaveTimestamp { get; set; }

		/// <summary>
		/// The scenario instruction text.
		/// </summary>
		public string ScenarioInstructions { get; set; }

		/// <summary>
		/// The player count.
		/// </summary>
		public uint PlayerCount { get; set; }

		/// <summary>
		/// The (compressed) header.
		/// </summary>
		public DataElements.Header Header { get; set; }

		/// <summary>
		/// The message strings, the background bitmap and cinematic file paths.
		/// </summary>
		public DataElements.MessagesCinematics MessagesCinematics { get; set; }

		/// <summary>
		/// Player AI names (and files), starting resources.
		/// </summary>
		public DataElements.PlayerAiResources PlayerAiResources { get; set; }

		/// <summary>
		/// Global victory conditions.
		/// </summary>
		public DataElements.GlobalVictory GlobalVictory { get; set; }

		/// <summary>
		/// Diplomacy settings.
		/// </summary>
		public DataElements.Diplomacy Diplomacy { get; set; }

		/// <summary>
		/// Disabled units and techs.
		/// </summary>
		public DataElements.Disables Disables { get; set; }

		/// <summary>
		/// Map data.
		/// </summary>
		public DataElements.Map Map { get; set; }

		/// <summary>
		/// Placed units and objects.
		/// </summary>
		public DataElements.Units Units { get; set; }

		/// <summary>
		/// Custom victory settings, and some unknown stuff.
		/// </summary>
		public DataElements.PlayerDiplomacyVarious PlayerDiplomacyVarious { get; set; }

		/// <summary>
		/// Trigger data.
		/// </summary>
		public DataElements.Triggers Triggers { get; set; }

		/// <summary>
		/// Included files.
		/// </summary>
		public DataElements.IncludedFiles IncludedFiles { get; set; }

		#endregion

		#endregion

		#region Functions

		/// <summary>
		/// Creates a new scenario objects and reads its data from the given file.
		/// </summary>
		/// <param name="filename">The path to the scx file to be loaded.</param>
		/// <remarks></remarks>
		public Scenario(string filename)
		{
			// Remember file name
			_filename = filename;

			// Create buffer and read file
			_buffer = new RAMBuffer(_filename);

			// Deserialize data
			ReadData();
		}

		/// <summary>
		/// Creates a new scenario objects and reads its data from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the serialized scenario data.</param>
		/// <remarks></remarks>
		public Scenario(RAMBuffer buffer)
		{
			// Use as internal buffer (will be copied anyway)
			_buffer = buffer;

			// Deserialize data
			ReadData();
		}

		/// <summary>
		/// Deserializes the data elements from the internal buffer.
		/// </summary>
		/// <remarks></remarks>
		private void ReadData()
		{
			// Read version
			string version = _buffer.ReadString(4);
			if(version != "1.21")
				throw new InvalidDataException($"Invalid primary file version: '{version}'");

			// Read remaining file header
			_buffer.ReadUInteger(); // Header length
			_buffer.ReadInteger(); // Unknown constant = 2
			LastSaveTimestamp = _buffer.ReadUInteger();
			int instructionsLength = _buffer.ReadInteger();
			ScenarioInstructions = _buffer.ReadString(instructionsLength);
			_buffer.ReadUInteger(); // Unknown constant = 0
			PlayerCount = _buffer.ReadUInteger();

			// Create memory stream for decompression
			using(MemoryStream output = new MemoryStream())
			using(MemoryStream input = _buffer.ToMemoryStream())
			{
				// Go to begin of uncompressed data
				input.Seek(_buffer.Position, SeekOrigin.Begin);

				// Create decompressor stream
				using(DeflateStream decompressor = new DeflateStream(input, CompressionMode.Decompress))
				{
					// Decompress
					decompressor.CopyTo(output);
					decompressor.Close();
				}

				// Save decompressed data into buffer
				_buffer = new RAMBuffer(output.ToArray());
			}
			
			// Read parts
			Header = new DataElements.Header().ReadData(_buffer);
			MessagesCinematics = new DataElements.MessagesCinematics().ReadData(_buffer);
			PlayerAiResources = new DataElements.PlayerAiResources().ReadData(_buffer);
			GlobalVictory = new DataElements.GlobalVictory().ReadData(_buffer);
			Diplomacy = new DataElements.Diplomacy().ReadData(_buffer);
			Disables = new DataElements.Disables().ReadData(_buffer);
			Map = new DataElements.Map().ReadData(_buffer);
			Units = new DataElements.Units().ReadData(_buffer);
			PlayerDiplomacyVarious = new DataElements.PlayerDiplomacyVarious().ReadData(_buffer);
			Triggers = new DataElements.Triggers().ReadData(_buffer);
			IncludedFiles = new DataElements.IncludedFiles().ReadData(_buffer);

			// Clear buffer to save memory
			_buffer.Clear();
		}

		/// <summary>
		/// Deserializes the data elements from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the serialized scenario data.</param>
		public void ReadData(RAMBuffer buffer)
		{
			// Use buffer as internal buffer and deserialize data
			_buffer = buffer;
			ReadData();
		}

		/// <summary>
		/// Serializes the data elements into the internal buffer.
		/// </summary>
		/// <remarks></remarks>
		private void WriteData()
		{
			// Initialize buffer
			if(_buffer == null)
				_buffer = new RAMBuffer();
			else if(_buffer.Length != 0)
				_buffer.Clear();

			// Write header
			_buffer.WriteString("1.21", 4);
			_buffer.WriteUInteger(4 + 4 + 4 + (uint)ScenarioInstructions.Length + 4 + 4);
			_buffer.WriteInteger(2);
			_buffer.WriteUInteger(LastSaveTimestamp);
			_buffer.WriteInteger(ScenarioInstructions.Length);
			_buffer.WriteString(ScenarioInstructions);
			_buffer.WriteUInteger(0);
			_buffer.WriteUInteger(PlayerCount);

			// Create buffer for compressed data elements
			RAMBuffer comprBuffer = new RAMBuffer();
			Header.WriteData(comprBuffer);
			MessagesCinematics.WriteData(comprBuffer);
			PlayerAiResources.WriteData(comprBuffer);
			GlobalVictory.WriteData(comprBuffer);
			Diplomacy.WriteData(comprBuffer);
			Disables.WriteData(comprBuffer);
			Map.WriteData(comprBuffer);
			Units.WriteData(comprBuffer);
			PlayerDiplomacyVarious.WriteData(comprBuffer);
			Triggers.WriteData(comprBuffer);
			IncludedFiles.WriteData(comprBuffer);

			// Compress data and copy to main buffer
			using(MemoryStream output = new MemoryStream())
			using(MemoryStream input = comprBuffer.ToMemoryStream())
			{
				// Create compressor stream
				using(DeflateStream compressor = new DeflateStream(output, CompressionMode.Compress))
				{
					// Compress
					input.CopyTo(compressor);
					input.Close();
				}

				// Save compressed data into main buffer
				_buffer.Write(output.ToArray());
			}
		}

		/// <summary>
		/// Serializes the data elements into the given file.
		/// </summary>
		/// <param name="destFile">The file where the scenario data shall be stored.</param>
		/// <remarks></remarks>
		public void WriteData(string destFile)
		{
			// Serialize data into buffer
			WriteData();

			// Save buffer
			_buffer.Save(destFile);
		}

		/// <summary>
		/// Serializes the data elements into the given stream.
		/// </summary>
		/// <param name="stream">The stream where the serialized data shall be copied to.</param>
		/// <remarks></remarks>
		public void WriteData(Stream stream)
		{
			// Serialize data into buffer
			WriteData();

			// Copy buffer into stream
			_buffer.ToMemoryStream().CopyTo(stream);
		}

		/// <summary>
		/// Serializes the data elements into the given buffer at its current position.
		/// </summary>
		/// <param name="buffer">The buffer where the scenario data shall be stored into.</param>
		public void WriteData(RAMBuffer buffer)
		{
			// Use buffer as internal buffer and serialize data
			_buffer = buffer;
			WriteData();
		}

		#endregion

		#region Static functions

		/*/// <summary>
		/// Komprimiert die gegebenen DAT-Daten (zlib-Kompression).
		/// </summary>
		/// <param name="dat">Die zu komprimierenden Daten.</param>
		/// <returns></returns>
		public static RAMBuffer CompressData(RAMBuffer dat)
		{
			// Ausgabe-Stream erstellen
			MemoryStream output = new MemoryStream();

			// Daten in Memory-Stream schreiben
			using(MemoryStream input = dat.ToMemoryStream())
			{
				// Kompressions-Stream erstellen
				using(DeflateStream compressor = new DeflateStream(output, CompressionMode.Compress))
				{
					// (De-)Komprimieren
					input.CopyTo(compressor);
					input.Close();
				}
			}

			// Ergebnis in Puffer schreiben
			return new RAMBuffer(output.ToArray());
		}

		/// <summary>
		/// Dekomprimiert die gegebenen DAT-Daten (zlib-Kompression).
		/// </summary>
		/// <param name="dat">Die zu dekomprimierenden Daten.</param>
		/// <returns></returns>
		public static RAMBuffer DecompressData(RAMBuffer dat)
		{
			// Ausgabe-Stream erstellen
			MemoryStream output = new MemoryStream();

			// Daten in Memory-Stream schreiben
			using(MemoryStream input = dat.ToMemoryStream())
			{
				// Kompressions-Stream erstellen
				using(DeflateStream decompressor = new DeflateStream(input, CompressionMode.Decompress))
				{
					// (De-)Komprimieren
					decompressor.CopyTo(output);
					decompressor.Close();
				}
			}

			// Ergebnis in Puffer schreiben
			return new RAMBuffer(output.ToArray());
		}*/

		#endregion

		#region Properties

		#endregion
	}
}