using RazorEnhanced.UI;
using System.Drawing;
using System.Windows.Forms;

namespace Assistant
{
	/// <summary>
	/// Summary description for HueEntry.
	/// </summary>
	internal class HueEntry : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private RazorTextBox hueNum;
		private RazorButton inGame;
		private System.Windows.Forms.Label preview;
		private RazorButton okay;
		private RazorButton cancel;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private int m_Hue;

		internal delegate void HueEntryCallback(int hue);

		internal static HueEntryCallback Callback = null;

		internal int Hue { get { return m_Hue; } }

		internal HueEntry()
			: this(0)
		{
		}

		internal HueEntry(int hue)
		{
			m_Hue = hue;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.hueNum = new RazorTextBox();
			this.inGame = new RazorButton();
			this.preview = new System.Windows.Forms.Label();
			this.okay = new RazorButton();
			this.cancel = new RazorButton();
			this.SuspendLayout();
			//
			// label1
			//
			this.label1.Location = new System.Drawing.Point(10, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(86, 19);
			this.label1.TabIndex = 0;
			this.label1.Text = "Hue Number:";
			//
			// hueNum
			//
			this.hueNum.Location = new System.Drawing.Point(91, 5);
			this.hueNum.Name = "hueNum";
			this.hueNum.Padding = new System.Windows.Forms.Padding(1);
			this.hueNum.Size = new System.Drawing.Size(60, 22);
			this.hueNum.TabIndex = 1;
			this.hueNum.TextChanged += new System.EventHandler(this.hueNum_TextChanged);
			//
			// inGame
			//
			this.inGame.Location = new System.Drawing.Point(5, 32);
			this.inGame.Name = "inGame";
			this.inGame.Size = new System.Drawing.Size(149, 23);
			this.inGame.TabIndex = 2;
			this.inGame.Text = "Select in Game";
			this.inGame.Click += new System.EventHandler(this.inGame_Click);
			//
			// preview
			//
			this.preview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.preview.Location = new System.Drawing.Point(5, 60);
			this.preview.Name = "preview";
			this.preview.Size = new System.Drawing.Size(149, 23);
			this.preview.TabIndex = 3;
			this.preview.Text = "Preview";
			this.preview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// okay
			//
			this.okay.Location = new System.Drawing.Point(12, 92);
			this.okay.Name = "okay";
			this.okay.Size = new System.Drawing.Size(62, 23);
			this.okay.TabIndex = 4;
			this.okay.Text = "&Okay";
			this.okay.Click += new System.EventHandler(this.okay_Click);
			//
			// cancel
			//
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(84, 92);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(62, 23);
			this.cancel.TabIndex = 5;
			this.cancel.Text = "Cancel";
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			//
			// HueEntry
			//
			this.AcceptButton = this.okay;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(161, 122);
			this.ControlBox = false;
			this.Controls.Add(this.hueNum);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.okay);
			this.Controls.Add(this.preview);
			this.Controls.Add(this.inGame);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HueEntry";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select a Hue";
			this.Load += new System.EventHandler(this.HueEntry_Load);
			this.ResumeLayout(false);
		}

		#endregion Windows Form Designer generated code

		private void hueNum_TextChanged(object sender, System.EventArgs e)
		{
			SetPreview(Utility.ToInt32(hueNum.Text, 0) & 0x3FFF);
		}

		internal const int TextHueIDX = 30;

		private void SetPreview(int hue)
		{
			if (hue > 0 && hue < 3000)
				preview.BackColor = Ultima.Hues.GetHue(hue - 1).GetColor(TextHueIDX);
			else
				preview.BackColor = Color.Black;
			preview.ForeColor = (preview.BackColor.GetBrightness() < 0.35 ? Color.White : Color.Black);
		}

		private void HueResp(int hue)
		{
			hue &= 0x3FFF;
			SetPreview(hue);
			hueNum.Text = hue.ToString();
			Callback = null;

			//Engine.MainWindow.ShowMe();
			this.Hide();
			this.SendToBack();
			this.WindowState = FormWindowState.Normal;
			this.BringToFront();
			this.Show();
		}

		private void inGame_Click(object sender, System.EventArgs e)
		{
			if (World.Player == null)
				return;

			Callback = new HueEntryCallback(HueResp);
	 		Assistant.Client.Instance.SendToClient(new HuePicker());
			World.Player.SendMessage(MsgLevel.Force, LocString.SelHue);
		}

		private void okay_Click(object sender, System.EventArgs e)
		{
			m_Hue = Utility.ToInt32(hueNum.Text, 0);
			this.DialogResult = DialogResult.OK;
			this.Close();
			Callback = null;
		}

		private void cancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
			Callback = null;
		}

		private void HueEntry_Load(object sender, System.EventArgs e)
		{
			Language.LoadControlNames(this);

			SetPreview(m_Hue);
			hueNum.Text = m_Hue.ToString();
		}
	}
}