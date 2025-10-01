using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Token: 0x020000AA RID: 170
public class CSV
{
	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06000859 RID: 2137 RVA: 0x0003AE29 File Offset: 0x00039029
	// (set) Token: 0x0600085A RID: 2138 RVA: 0x0003AE31 File Offset: 0x00039031
	public int ColumnCount { get; private set; }

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x0600085B RID: 2139 RVA: 0x0003AE3A File Offset: 0x0003903A
	// (set) Token: 0x0600085C RID: 2140 RVA: 0x0003AE42 File Offset: 0x00039042
	public int RowCount { get; private set; }

	// Token: 0x0600085D RID: 2141 RVA: 0x0003AE4C File Offset: 0x0003904C
	public CSV(string csvData)
	{
		this.Cells = new List<List<string>>();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = true;
		int num = 0;
		bool flag4 = false;
		bool flag5 = false;
		StringBuilder stringBuilder = new StringBuilder();
		if (!csvData.EndsWith("\n"))
		{
			csvData += "\n";
		}
		csvData = csvData.Replace("\r", "");
		for (int i = 0; i < csvData.Length; i++)
		{
			char c = csvData[i];
			if (flag)
			{
				if (c == '"')
				{
					if (flag2)
					{
						stringBuilder.Append('"');
					}
					flag2 = !flag2;
				}
				else if (flag2)
				{
					if (c == ',')
					{
						flag4 = true;
					}
					else
					{
						if (c != '\n')
						{
							throw new Exception("CSV Parse Error: unescaped quote character at " + i);
						}
						flag4 = true;
						flag5 = true;
					}
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			else if (c == ',')
			{
				flag4 = true;
			}
			else if (c == '\n')
			{
				flag4 = true;
				flag5 = true;
			}
			else if (c == '"' && flag3)
			{
				flag = true;
			}
			else
			{
				stringBuilder.Append(c);
			}
			if (flag4)
			{
				flag3 = true;
				flag2 = false;
				flag = false;
				flag4 = false;
				List<string> list;
				if (this.ColumnCount == 0)
				{
					list = new List<string>();
					this.Cells.Add(list);
				}
				else
				{
					list = this.Cells[num];
					num++;
				}
				list.Add(stringBuilder.ToString());
				stringBuilder.Clear();
				if (flag5)
				{
					flag5 = false;
					if (this.ColumnCount == 0)
					{
						this.ColumnCount = this.Cells.Count;
					}
					else if (num != this.ColumnCount)
					{
						throw new Exception("CSV Parse Error: incorrect # columns at character " + i);
					}
					num = 0;
				}
			}
			else
			{
				flag3 = false;
			}
		}
		if (this.ColumnCount > 0)
		{
			this.RowCount = this.Cells[0].Count;
		}
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x0003B01C File Offset: 0x0003921C
	public CSV(CSV csvToCopy, int[] columnsToKeep = null)
	{
		this.Cells = new List<List<string>>();
		for (int i = 0; i < csvToCopy.ColumnCount; i++)
		{
			if (columnsToKeep == null || columnsToKeep.Contains(i))
			{
				this.Cells.Add(new List<string>());
				int index = this.Cells.Count - 1;
				for (int j = 0; j < csvToCopy[i].Count; j++)
				{
					this.Cells[index].Add(csvToCopy[i][j]);
				}
			}
		}
		if (this.Cells.Count > 0)
		{
			this.ColumnCount = this.Cells.Count;
			this.RowCount = this.Cells[0].Count;
		}
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x0003B0E0 File Offset: 0x000392E0
	public new string ToString()
	{
		if (this.RowCount == 0 || this.ColumnCount == 0)
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < this.RowCount; i++)
		{
			for (int j = 0; j < this.ColumnCount; j++)
			{
				string text = this.Cells[j][i];
				if (text.Contains(',') || text.Contains('"') || text.Contains('\n'))
				{
					stringBuilder.Append('"');
					stringBuilder.Append(text.Replace("\"", "\"\""));
					stringBuilder.Append('"');
				}
				else
				{
					stringBuilder.Append(text);
				}
				if (j < this.ColumnCount - 1)
				{
					stringBuilder.Append(',');
				}
			}
			stringBuilder.Append('\n');
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x0003B1BC File Offset: 0x000393BC
	public int ColumnFromHeader(string header)
	{
		if (this.RowCount == 0 || this.ColumnCount == 0)
		{
			return -1;
		}
		for (int i = 0; i < this.ColumnCount; i++)
		{
			if (this.Cells[i][0] == header)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x0003B20C File Offset: 0x0003940C
	public int RowFromKey(string key)
	{
		if (this.RowCount == 0 || this.ColumnCount == 0)
		{
			return -1;
		}
		for (int i = 0; i < this.RowCount; i++)
		{
			if (this.Cells[0][i] == key)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x0003B25C File Offset: 0x0003945C
	public void RemoveColumn(int column)
	{
		if (column >= 0 && column < this.ColumnCount)
		{
			this.Cells.RemoveAt(column);
			int columnCount = this.ColumnCount;
			this.ColumnCount = columnCount - 1;
		}
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x0003B294 File Offset: 0x00039494
	public void RemoveRow(int row)
	{
		if (row >= 0 && row < this.RowCount)
		{
			for (int i = 0; i < this.ColumnCount; i++)
			{
				this.Cells[i].RemoveAt(row);
			}
			int rowCount = this.RowCount;
			this.RowCount = rowCount - 1;
		}
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x0003B2E4 File Offset: 0x000394E4
	public void InsertColumn(int column, List<string> data)
	{
		if (column >= 0 && column < this.ColumnCount)
		{
			this.Cells.Insert(column, data);
			int columnCount = this.ColumnCount;
			this.ColumnCount = columnCount + 1;
		}
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x0003B31C File Offset: 0x0003951C
	public void InsertRow(int row, List<string> data)
	{
		if (row >= 0 && row < this.RowCount)
		{
			for (int i = 0; i < this.ColumnCount; i++)
			{
				this.Cells[i].Insert(row, (i < data.Count) ? data[i] : "");
			}
			int rowCount = this.RowCount;
			this.RowCount = rowCount + 1;
		}
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x0003B380 File Offset: 0x00039580
	public void AddColumn(List<string> data)
	{
		this.Cells.Add(data);
		int columnCount = this.ColumnCount;
		this.ColumnCount = columnCount + 1;
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0003B3AC File Offset: 0x000395AC
	public void AddRow(List<string> data)
	{
		for (int i = 0; i < this.ColumnCount; i++)
		{
			this.Cells[i].Add((i < data.Count) ? data[i] : "");
		}
		int rowCount = this.RowCount;
		this.RowCount = rowCount + 1;
	}

	// Token: 0x1700019E RID: 414
	public List<string> this[int key]
	{
		get
		{
			return this.Cells[key];
		}
	}

	// Token: 0x040005E5 RID: 1509
	public List<List<string>> Cells;

	// Token: 0x040005E8 RID: 1512
	private const char separator = ',';

	// Token: 0x040005E9 RID: 1513
	private const char quote = '"';

	// Token: 0x040005EA RID: 1514
	private const string singleQuote = "\"";

	// Token: 0x040005EB RID: 1515
	private const string doubleQuote = "\"\"";
}
