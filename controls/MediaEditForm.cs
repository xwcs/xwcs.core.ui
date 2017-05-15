using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Net;
using System.Collections.Specialized;
using System.IO;

namespace xwcs.core.ui.controls
{
    public partial class MediaEditForm : DevExpress.XtraGrid.Views.Grid.EditFormUserControl
    {
        public MediaEditForm()
        {
            InitializeComponent();

            SetBoundFieldName(textEdit_nmedia, "nmedia");
            SetBoundPropertyName(textEdit_nmedia, "EditValue");

			SetBoundFieldName(textEdit_id, "id_media");
			SetBoundPropertyName(textEdit_id, "EditValue");

			SetBoundFieldName(textEdit_name, "name_media");
			SetBoundPropertyName(textEdit_name, "EditValue");

			SetBoundFieldName(textEdit_type, "type_media");
			SetBoundPropertyName(textEdit_type, "EditValue");

			SetBoundFieldName(textEdit_extension, "extension_media");
			SetBoundPropertyName(textEdit_extension, "EditValue");

			SetBoundFieldName(textEdit_aka, "aka_media");
			SetBoundPropertyName(textEdit_aka, "EditValue");
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				WebClient client = new WebClient();
				NameValueCollection parameters = new NameValueCollection();
				parameters.Add("MAX_FILE_SIZE", "100000");			
				client.QueryString = parameters;
				client.Proxy = new WebProxy("localhost", 8888);

				try
				{
					var responseBytes = client.UploadFile("http://localhost:4854/attach/put?db=niter ", null, dialog.FileName);
					string response = Encoding.ASCII.GetString(responseBytes);
					textEdit_id.EditValue = response;
					textEdit_name.EditValue = response;
					textEdit_aka.EditValue = dialog.FileName;
					textEdit_extension.EditValue = Path.GetExtension(dialog.FileName);
					textEdit_type.EditValue = Path.GetExtension(dialog.FileName).Substring(1);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}
	}
}
