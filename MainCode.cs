﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(textBox1.Text);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string filePath = e.Argument.ToString();
            byte[] buffer;
            int bytesRead;
            long size;
            long totalBytesRead = 0;

            using (Stream file = File.OpenRead(filePath))
            {
                size = file.Length;

                using (HashAlgorithm hash = MD5.Create())
                {
                    do
                    {
                        buffer = new byte[4096];
                        bytesRead = file.Read(buffer, 0, buffer.Length);
                        totalBytesRead += bytesRead;
                        hash.TransformBlock(buffer, 0, bytesRead, null, 0);
                        backgroundWorker1.ReportProgress((int)((double)totalBytesRead / size * 100));
                    }
                    while (bytesRead != 0);

                    hash.TransformFinalBlock(buffer, 0, 0);
                    e.Result = HashString(hash.Hash);
                }
            }
        }

        private static string HashString(byte[] bytes)
        {
            StringBuilder hash = new StringBuilder(32);
            foreach(byte b in bytes)
            
                hash.Append(b.ToString("X2").ToLower());
                return hash.ToString();
            
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(e.Result.ToString());
            progressBar1.Value = 0;
        }
    }
}
