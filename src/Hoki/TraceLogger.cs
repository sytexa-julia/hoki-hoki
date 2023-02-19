using System;
using System.IO;
using System.Diagnostics;

namespace Hoki {
	public class TraceLogger : TraceListener {
		QuickWriter writer;

		public TraceLogger(string log) {
			writer=new QuickWriter(log);
		}

		public override void Write(string message) {
			writer.Write(message);
		}

		public override void Write(string message, string category) {
			writer.Write(message+" ("+category+")");
		}

		public override void Write(object o) {
			writer.Write(o.ToString());
		}

		public override void Write(object o, string category) {
			writer.Write(o.ToString()+" ("+category+")");
		}

		public override void WriteLine(string message) {
			writer.WriteLine(message);
		}

		public override void WriteLine(string message, string category) {
			writer.WriteLine(message+" ("+category+")");
		}

		public override void WriteLine(object o) {
			writer.WriteLine(o.ToString());
		}

		public override void WriteLine(object o, string category) {
			writer.WriteLine(o.ToString()+" ("+category+")");
		}

		protected override void WriteIndent() {
			writer.Write("\t");
		}

		public override void Fail(string message) {
			writer.WriteLine(message);
		}

		public override void Fail(string message, string detailMessage) {
			writer.WriteLine(message);
			writer.WriteLine(detailMessage);
		}

		private class QuickWriter {
			string path;
			
			public QuickWriter(string path) {
				this.path=path;
				WriteLine("----------");
				WriteLine(System.DateTime.Now.Year+"-"+System.DateTime.Now.Month+"-"+System.DateTime.Now.Day+" "+System.DateTime.Now.Hour+":"+System.DateTime.Now.Minute+":"+System.DateTime.Now.Second);
			}

			public void Write(string message) {
				StreamWriter writer=new StreamWriter(path,true);
				writer.Write(message);
				writer.Close();
			}

			public void WriteLine(string message) {
				Write(message+Environment.NewLine);
			}
		}
	}
}