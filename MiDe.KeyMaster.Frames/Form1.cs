using Microsoft.Extensions.Logging;
using MiDe.KeyMaster.App;
using System.Runtime.InteropServices;

namespace MiDe.KeyMaster.Frames
{
    public delegate void Notify();

    public partial class Form1 : Form
    {
        BorrowController borrowController;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        public Form1(string dbPath, ILogger logger)
        {   
            borrowController = new BorrowController(dbPath, logger);
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.ControlBox = false;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 10, 10));
            borrowController.ChangeToKey += BorrowController_ChangeToKey;
            borrowController.ChangeToPerson += BorrowController_ChangeToPerson;
            borrowController.DisplayStatusMessage += BorrowController_DisplayStatusMessage;
            borrowController.ClearKey += BorrowController_ClearInput;
            borrowController.ClearPerson += BorrowController_ClearInput;
            BorrowController_ChangeToKey(this, EventArgs.Empty);
            inputTxtBox.CharacterCasing = CharacterCasing.Upper;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_SHOWME)
            {
                ShowMe();
            }
            base.WndProc(ref m);
        }

        private void ShowMe()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            // get our current "TopMost" value (ours will always be false though)
            bool top = TopMost;
            // make our form jump to the top of everything
            TopMost = true;
            // set it back to whatever it was
            TopMost = top;
        }

        private void BorrowController_DisplayStatusMessage(object sender, MessageEventArgs e)
        {
            statusTextBox.Clear();
            statusTextBox.Text = e.Message;
        }

        private void BorrowController_ChangeToPerson(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(".\\pict\\person2.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            label.Text = "Scanati persoana";
            inputTxtBox.Clear();
            inputTxtBox.Focus();
        }

        private void BorrowController_ChangeToKey(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(".\\pict\\keys.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            label.Text = "Scanati cheia";
            inputTxtBox.Clear();
            inputTxtBox.Focus();
        }

        private void BorrowController_ClearInput(object sender, EventArgs e)
        {
            inputTxtBox.Clear();
        }

        private void inputTxtBox_Leave(object sender, EventArgs e)
        {
            borrowController.AddNewInput(inputTxtBox.Text.ToString());
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //if the form is minimized  
            //hide it from the task bar  
            //and show the system tray icon (represented by the NotifyIcon control)  
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                keyMasterTray.Visible = true;
            }
        }

        private void keyMasterTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            keyMasterTray.Visible = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void keyMasterTray_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show();
            }
            else
            {
                Show();
                this.WindowState = FormWindowState.Normal;
                keyMasterTray.Visible = false;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.X)       // Ctrl-S Save
            {
                this.Close();
            }
        }
    }
}