﻿namespace Lab_4
{
    partial class Childs
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.SuspendLayout();
            // 
            // Childs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Childs";
            this.Text = "Child";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Childs_FormClosed);
            this.Load += new System.EventHandler(this.Childs_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Childs_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Childs_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Childs_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Childs_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Childs_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}