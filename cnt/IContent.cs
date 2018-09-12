using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xwcs.core.ui.cnt
{
    public interface IContent : IDisposable
    {
		void LoadContent(string Path, bool force = false);
		void Next();
		void Prev();
		void Close();
		void First();
		void Last();

		System.Windows.Forms.DockStyle Dock { get; set; }
	}
}
