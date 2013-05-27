using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Digbyswift.Utils.Extensions
{
    /// <summary>
    /// "Natural Sort" comparer for "human" sorting on strings with numbers i.e "X100" > "X2"
    /// </summary>
    /// <remarks>
    /// Kudos to Justin Jones: http://www.codeproject.com/KB/string/NaturalSortComparer.aspx
    /// </remarks>
    public class NaturalComparer : Comparer<string>, IDisposable
    {

    	private readonly bool _sortAscending = true;
		private Dictionary<string, string[]> _table;

		public NaturalComparer()
			: this(true)
		{
		}

    	public NaturalComparer(bool ascending)
    	{
    		_sortAscending = ascending;
			_table = new Dictionary<string, string[]>();
		}

		public override int Compare(string x, string y)
		{
			string workingX = x, workingY = y;

			if(!_sortAscending)
			{
				workingX = y;
				workingY = x;
			}

			if (workingX == workingY || workingX == null || workingY == null)
                return 0;

            string[] x1, y1;

			if (!_table.TryGetValue(workingX, out x1))
            {
				x1 = Regex.Split(workingX.Replace(" ", ""), "([0-9]+)");
                _table.Add(workingX, x1);
            }

			if (!_table.TryGetValue(workingY, out y1))
            {
				y1 = Regex.Split(workingY.Replace(" ", ""), "([0-9]+)");
				_table.Add(workingY, y1);
            }

            for (int i = 0; i < x1.Length && i < y1.Length; i++)
            {
                if (x1[i] != y1[i])
                    return PartCompare(x1[i], y1[i]);
            }

            if (y1.Length > x1.Length)
                return 1;
            
			if (x1.Length > y1.Length)
                return -1;

			return 0;
        }

        private static int PartCompare(string left, string right)
        {
            int x, y;
            if (!int.TryParse(left, out x))
                return left.CompareTo(right);

            if (!int.TryParse(right, out y))
                return left.CompareTo(right);

            return x.CompareTo(y);
        }

        public void Dispose()
        {
            _table.Clear();
            _table = null;
        }

    }

    
}
