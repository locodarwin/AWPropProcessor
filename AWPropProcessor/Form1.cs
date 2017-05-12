using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AWPropProcessor
{
    public partial class Form1 : Form
    {
        private readonly BackgroundWorker worker;

        public Form1()
        {
            InitializeComponent();

            // Create the background worker object, enable it to report progress,
            // And then assign the mrhods that are called to do the work, handle the progress updates,
            // And provide tasks that run when the background worker completes.
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Process;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            Status("Ready to begin. Please follow steps 1 through 8.");

            textBox2.Text = Globals.FileOut;
            comboBox2.Enabled = false;
            textBox4.Enabled = false;
            comboBox3.Enabled = false;
            textBox5.Enabled = false;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox9.Enabled = false;
            textBox10.Enabled = false;
            textBox5.Enabled = false;
            checkBox8.Enabled = false;
            checkBox5.Enabled = false;
            checkBox7.Enabled = false;
            checkBox6.Checked = true;
            textBox6.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        public class Globals
        {
            public static string FileIn;
            public static string FileOut = @"C:\output.txt";


        }








        private void Status(string sStatusText)
        {
            listStatus.Items.Add(sStatusText);
        }


        // Browse button for the File Input field
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Browse Propdump Files";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "Propdump files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)

            {
                textBox1.Text = openFileDialog1.FileName;
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Check to see if file exists if it doesn't, msgbox
            string FileIn = textBox1.Text;
            if (!File.Exists(FileIn))
            {
                MessageBox.Show("Couldn't find the file! Try again.");
                return;
            }

            // Check the first line, to at least try and ensure it's a propdump (and to get propdump version)
            string line1 = File.ReadLines(FileIn).First();



            // Launch the BG worker process to do processing
            worker.RunWorkerAsync();
            button3.Enabled = false;


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                comboBox2.Enabled = true;
                textBox4.Enabled = true;
            }
            if (checkBox1.Checked == false)
            {
                comboBox2.Enabled = false;
                textBox4.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                comboBox3.Enabled = true;
                textBox5.Enabled = true;
            }
            if (checkBox4.Checked == false)
            {
                comboBox3.Enabled = false;
                textBox5.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
            }
            if (checkBox2.Checked == false)
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                checkBox8.Enabled = true;
                textBox7.Enabled = true;
                textBox8.Enabled = true;
                textBox9.Enabled = true;
                textBox10.Enabled = true;
            }
            if (checkBox3.Checked == false)
            {
                checkBox8.Enabled = false;
                textBox7.Enabled = false;
                textBox8.Enabled = false;
                textBox9.Enabled = false;
                textBox10.Enabled = false;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked == true)
            {
                button5.Enabled = true;
            }
            if (checkBox9.Checked == false)
            {
                button5.Enabled = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                checkBox5.Enabled = false;
                checkBox7.Enabled = false;
                textBox6.Enabled = false;
                button4.Enabled = false;
            }
            if (checkBox6.Checked == false)
            {
                checkBox5.Enabled = true;
                checkBox7.Enabled = true;
                textBox6.Enabled = true;
                button4.Enabled = true;
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            //lisStatus.Items.Add(e.UserState.ToString());
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button3.Enabled = true;
        }


    }












    /* Understanding different versions of propdump files
         * 
         * Propdump v5: http://wiki.activeworlds.com/index.php?title=Propdump
         * 
         * Propdump v3: http://www.aw-europe.com/help/aw36/admin_propdump.html
         * 
         * Was there a propdump v4? According to propdump tool by Andras, there was.
         


    */


    /*

    Propdump 5 file format

    359971 1167343043 -800 0 200 0 0 0 0 7 0 99 0 floor01create name gz1wat1, texture water1_top mask=semitrans10,solid no,move 0 0.1 0.5 time=2 smooth sync

    The 1st part (358257) is the citizen number of the owner of the object.
    The 2nd part (1167343043) is the timestamp that contains the date when the object had been created.
    The 3rd part (-800) is the X position of the object.
    The 4th part (0) is the Y position of the object.
    The 5th part (200) is the Z position of the object.
    The 6th part (0) is the YAW orientation of the object.
    The 7th part (0) is the Tilt orientation of the object.
    The 8th part (0) is the Roll orientation of the object.
    The 9th part (0) is the type of the object. (0: Object. 1: Camera. 2: Zone. 3: Particle Emitter. 4: Mover.).
    The 10th part (7) is the length of the model name (floor01).
    The 11th part (0) is the length of the description.
    The 12th part (99) is the length of the action (create name gz1wat1 [...]).
    The 13th part (0) is the length of the object data (for object types other than 0).
    The 14th part (all that remains) contains object information in the length described above.

Newline, or carriage return with line feed \r\n, is replaced with characters 0x80 0x7F (before 3.3: 0x0D 0x7F), and \n with character 0x7F. A solitary \r is not translated

    */


    /* 
    propdump version 3
    318855 1027876103 542 690 320 1795 2 1 12 0 0 walk004h.rwx
    318855 1027876480 544 686 169 1795 2 1 12 0 0 walk004h.rwx
    0318855 1034945277 542 790 320 1795 2 1 7 0 74 easle2pcreate picture http://gorsands.dns2go.com/paths/gorsands/pictures/Doc2.jpg
    318855 1027876086 534 690 1320 1795 2 1 12 0 0 walk004h.rwx
    318855 1027876480 535 686 1169 1795 2 1 12 0 0 walk004h.rwx 
     */





}
