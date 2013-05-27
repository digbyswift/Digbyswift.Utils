using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Digbyswift.Utils.Extensions.Ado
{
	public static class DataTableExtensions
	{

		public static void ToCsv(this DataTable dataTable, string targetFile, bool includeHeaders, bool delimitWithQuotes)
		{
			ToDelimitedFile(dataTable, targetFile, includeHeaders, ',', false);
		}

		public static void ToDelimitedFile(this DataTable dataTable, string targetFile, bool includeHeaders, char valueDelimter, bool delimitWithQuotes)
		{
			if (targetFile == null)
				throw new ArgumentNullException("targetFile");

			if (dataTable.Rows == null)
				throw new ArgumentOutOfRangeException("dataTable", "Rows property is null");

			using (var writer = new StreamWriter(targetFile, false, Encoding.UTF8))
			{
				if (includeHeaders)
				{
					writer.WriteLine(String.Join(",", dataTable.Columns.Cast<string>().ToArray()));
				}
				
				foreach (DataRow row in dataTable.Rows)
				{
					writer.WriteLine(BuildDelimitedData(row.ItemArray, valueDelimter, delimitWithQuotes));
				}
			}
		}

		/// <summary>
		/// Populates a DataTable with the contents of a CSV.
		/// </summary>
		public static void FromCsv(this DataTable dataTable, string sourceFile, bool useFirstRowAsHeadings, bool delimitedWithQuotes, out int linesRead)
		{
			FromDelimitedFile(dataTable, sourceFile, useFirstRowAsHeadings, ',', delimitedWithQuotes, out linesRead);
		}

		/// <summary>
		/// Populates a DataTable with the contents of a CSV or other deilmited file format. Assumes 
		/// that the delimited values are not also delimited with double quotes and therefore will just
		/// perform a split upon the delmiting value.
		/// </summary>
		public static void FromDelimitedFile(this DataTable dataTable, string sourceFile, bool useFirstRowAsHeadings, char valueDelimter, out int linesRead)
		{
			FromDelimitedFile(dataTable, sourceFile, useFirstRowAsHeadings, valueDelimter, false, out linesRead);
		}

		/// <summary>
		/// Populates a DataTable with the contents of a CSV or other delmited file format.
		/// </summary>
		public static void FromDelimitedFile(this DataTable dataTable, string sourceFile, bool useFirstRowAsHeadings, char valueDelimter, bool delimitedWithQuotes, out int linesRead)
		{
			if (sourceFile == null)
				throw new ArgumentNullException("sourceFile");

			linesRead = 0;
			string[] csvData = File.ReadAllLines(sourceFile);

			if (csvData.Length == 0)
				return;

			#region Populate headers

			string[] headings = csvData[0].Split(',');
			for (int i = 0; i < headings.Length; i++)
			{
				string heading = useFirstRowAsHeadings
				                 	? headings[i]
				                 	: String.Format("Col{0:XX}", i);

				dataTable.Columns.Add(heading, typeof (string));
			}

			#endregion

			#region Populate rows

			string delmiter = String.Format("{0}{1}", delimitedWithQuotes ? "\"" : "", valueDelimter);

			int rowIndex = useFirstRowAsHeadings ? 1 : 0;
			for (int i = rowIndex; i < csvData.Length; i++)
			{
				DataRow row = dataTable.NewRow();
				string[] lineData = csvData[i].Split(new[] { delmiter }, StringSplitOptions.None);

				for (int j = 0; j < lineData.Length; j++)
				{
					row[j] = lineData[j].Trim(delimitedWithQuotes ? '\"' : '\0');
				}

				dataTable.Rows.Add(row);
				linesRead++;
			}

			#endregion

		}

		private static string BuildDelimitedData(IEnumerable<object> objects, char valueDelimter, bool delimitWithQuotes)
		{
			string delimiter = String.Format("{0}{1}{0}", valueDelimter, delimitWithQuotes ? "\"" : "");
			return String.Join(delimiter, objects.Select(c => c.ToString()).ToArray());
		}


	}
}