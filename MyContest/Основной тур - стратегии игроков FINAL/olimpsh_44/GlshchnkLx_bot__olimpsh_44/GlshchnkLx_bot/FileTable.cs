using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace FileSystem
{
    class FileTable
    {
		private string _file_path;
		public string Path
		{
			get
			{
				return this._file_path;
			}

			set {
				this._file_path = value;
			}
		}

		private ArrayList _table;

		private uint _bytes;
		public uint GetBytes{
			get {
				return this._bytes;
			}
		}
		public double GetKBytes{
			get {
				return this._bytes / 1024.0;
			}
		}
		public double GetMBytes{
			get {
				return (this._bytes / 1024.0) / 1024.0;
			}
		}

        private uint _element_count;
        public uint GetElemCount
        {
            get
            {
                return this._element_count;
            }
        }

        public void load_file(char delimiter)
        {
            StreamReader file = new StreamReader(_file_path);
            string buff = file.ReadLine();

			_bytes = 0;
            while (buff != null)
            {
                ArrayList Str = new ArrayList();
                _element_count++;
                int sw = 0;
                for(int i = 0; i < buff.Length; i++)
                {					
					if (buff [i] == delimiter || i == buff.Length - 1) {
                        if (buff[i] == delimiter)
                        {
                            Str.Add(buff.Substring(sw, i - sw));
                            sw = i + 1;
                        }else
                        {
                            Str.Add(buff.Substring(sw, i - sw + 1));
                            sw = i + 1;
                        }						
					}
					_bytes++;
                }
				_table.Add (Str);
                buff = file.ReadLine();
            }
            file.Close();
        }

		public void ClearContent(){
            for (int i = 0; i < _table.Count; i++)
            {
                _table[i] = null;
          
            }
			_table.Clear ();
			_bytes = 0;
            _element_count = 0;
            GC.Collect();
        }

		public string get_data_from_table(int x, int y){
			ArrayList temp = (ArrayList)_table [x];
			return ((string)temp [y]);
		}
		public ArrayList get_data_from_table(int x){
			return (ArrayList)_table [x];
		}


		public int str2num (string data){
			int length = data.Length;

			int result = 0;

			for (int i = length - 1; i >= 0; i--) {
				if (data [i] == '-')
					result *= -1;
				else 
					result += (int)(Char.GetNumericValue (data [i]) * (int)Math.Pow (10, length - i - 1));
			}
			return result;
		}

        public FileTable ()
        {
            _table = new ArrayList();
			_bytes = 0;
            _element_count = 0;
            _file_path = "NULL";
        }
    }
}

// String[] words = buf.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);