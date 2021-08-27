//=======================================================================================
//
//  Purpose:    Send simple commands down the serial line to device waiting for commands.
//
//  Copyright (C) 2011 Mark Stevens
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software") to use
//  the Software in a non-commercial environment subject to the following conditions:
//
//  1.  The above copyright notice and this permission notice shall be included in
//      all copies or substantial portions of the Software.
//  2.  The original author shall be acknowledged in any software and documentation
//      including (but not restricted to) books, articles and online posts.
//
//  Use in a commercial environment is subject to the written consent of the author
//  and/or copyright holder.
//
//  The Software, documentation, instructions for use and associated materials
//  are provided "as is" without warranty of any kind express or implied.  The
//  author and copyright holders shall in no way be held liable for any claim
//  or damages arising as a result of the use of the software and/or documentation.
//
//=======================================================================================
using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace WindowsSerialCommander
{
    public partial class MainForm : Form
    {
        #region Constructor(s)

        /// <summary>
        /// Constructor for the application.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Initialise the application.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            com6 = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
            com6.Open();
        }

        /// <summary>
        /// Send a command to the device listening on the com port.
        /// </summary>
        private void btnLEDOn_Click(object sender, EventArgs e)
        {
            com6.Write("FLED\r");
        }

        /// <summary>
        /// Send a command to turn the LED off.
        /// </summary>
        private void btnLEDOff_Click(object sender, EventArgs e)
        {
            com6.Write("PLED\r");
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            com6.Write("Volume+\r");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            com6.Write("BAEP\r");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            com6.Write("Volume-\r");
        }

       
        

        

        private void button4_Click(object sender, EventArgs e)
        {
            com6.Write("Leftear\r");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            com6.Write("Rightear\r");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            com6.Write("Both\r");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            com6.Write("Leftvisual\r");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            com6.Write("Rightvisual\r");
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int vol = trackBar1.Value;
            vol.ToString();
            if (vol < 10)
            {
                com6.Write("vol0" + vol + "\r");

            }
            else
            {
                com6.Write("vol" + vol + "\r");
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            com6.Write("leftShock\r");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            com6.Write("rightShock\r");
        }


      

        
        
        
        
        
      

     

       
    }
}
