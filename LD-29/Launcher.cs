using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LD_29
{
	public partial class Launcher : Form
	{
		public Launcher()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			bool soundEnabled = SoundBox.Checked;
			float vol = (float)trackBar1.Value;
			new Thread(() =>
			{
				using (Game game = new Game("One Who Shall Not Be Named Yet", soundEnabled, vol)) game.Start();
			}).Start();
			Close();
		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			SoundBox.Text = "Sound (" + trackBar1.Value + "%)";
		}

        private void SoundBox_CheckedChanged(object sender, EventArgs e)
        {

        }
	}
}